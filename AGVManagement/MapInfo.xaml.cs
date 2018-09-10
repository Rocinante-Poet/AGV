using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AGV.BLL;
using AGVManagement.Enumeration;
using AGVManagement.instrument;
using AGVManagement.MapPaint;
using MySql.Data.MySqlClient;
using System.Threading;

namespace AGVManagement
{
    /// <summary>
    /// Map.xaml 的交互逻辑
    /// </summary>
    public partial class Map : Window
    {
        MapMessageBLL GesMap = new MapMessageBLL();
        MapManag MapManag = new MapManag();
        private long MapTime;
        private string MapNa;
        public Map()
        {
            InitializeComponent();
            MapDataBinding(null);

        }


        /// <summary>
        /// 查询所有地图
        /// </summary>
        public void MapDataBinding(string MpName)
        {
            MapInstrument.keyValuePairs.Clear();
            MapInstrument.valuePairs.Clear();
            MapInstrument.wirePointArrays.Clear();
            MapInstrument.GetKeyValues.Clear();

            Thread thread = new Thread(() =>
            {
                DataTable dt = new DataTable("Map");
                dt.Columns.Add(new DataColumn("MapName"));
                dt.Columns.Add(new DataColumn("MapInfo"));
                DataTable ga = GesMap.GetMapData(MpName);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (ga != null)
                    {
                        foreach (DataRow item in ga.Rows)
                        {
                            dt.Rows.Add(new object[] { item["Name"].ToString(), (UTC.ConvertLongDateTime(long.Parse(item["CreateTime"].ToString())).ToString() + "," + item["Width"].ToString() + "," + item["Height"].ToString()) });
                        }

                        MapData.ItemsSource = dt.DefaultView;
                        MapData.AutoGenerateColumns = false;
                        MapData.SelectedIndex = 0;
                        if (ga.Rows.Count > 0)
                        {
                            MapShow();
                        }
                    }
                }));
            });
            thread.IsBackground = true;
            thread.Start();
        }


        /// <summary>
        /// 行点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapData_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                if (MapData.SelectedItems.Count > 0)
                {
                    MapShow();
                }
            }
        }


        /// <summary>
        /// 显示具体地图信息
        /// </summary>
        private void MapShow()
        {
            MapInstrument.keyValuePairs.Clear();
            MapInstrument.valuePairs.Clear();
            MapInstrument.wirePointArrays.Clear();
            MapInstrument.GetKeyValues.Clear();
            Painting.siseWin = 1;
            MapIN.Children.Clear();
            string[] arr = ((DataRowView)MapData.SelectedValue).Row.ItemArray[1].ToString().Split(',');
            if (arr.Count().Equals(3))
            {
                string Times = arr[0];
                string MpName = ((DataRowView)MapData.SelectedValue).Row.ItemArray[0].ToString();
                MapName.Content = "地图区域信息（" + MpName + "）";
                MapNa = MpName;
                MapIN.Width = double.Parse(arr[1]) * MapManag.Sise;
                MapIN.Height = double.Parse(arr[2]) * MapManag.Sise;
                MapTime = long.Parse(UTC.ConvertDateTimeLong(Convert.ToDateTime(Times)).ToString());
                MapManag.SelectMap(UTC.ConvertDateTimeLong(Convert.ToDateTime(Times)), MapIN,false);
            }
        }

        /// <summary>
        /// 搜索地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectMap_Click(object sender, RoutedEventArgs e)
        {
            MapDataBinding(SelectMap.Text.Trim());
        }

        /// <summary>
        /// 编辑地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompileMap_Click(object sender, RoutedEventArgs e)
        {
            if (MapData.Items.Count == 0)
            {
                MessageBox.Show("暂无地图");
                return;
            }
            MainWindow mapRedact = new MainWindow(MapTime, MapNa, null, 0, 0);
            mapRedact.ShowDialog();


        }

        private void AddMap_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            AddMap but = new AddMap();
            but.ShowDialog();

        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Channel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "地图信息文件|*.tll|所有文件|*.*";
            if (dlg.ShowDialog() == true)
            {
                string sqlText = File.ReadAllText(dlg.FileName);
                if (GesMap.MapToleadNumber(sqlText) == true)
                {
                    MessageBox.Show("导入成功！");
                    SelectMap_Click(null,null);
                }
                else
                {
                    MessageBox.Show("导入失败！");
                }
            }
        }
        OperateDBBLL operateDB = new OperateDBBLL();
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Derive_Click(object sender, RoutedEventArgs e)
        {
            if (MapData.Items.Count == 0)
            {
                MessageBox.Show("暂无地图");
                return;
            }
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "地图信息文件|*.tll";
            sfd.FileName = "(" + MapNa + ")" + DateTime.Now.ToString("yyyyMMdd");
            if (sfd.ShowDialog() == true)
            {
                string sql = operateDB.ExportSettings(MapTime, "agv") + operateDB.ExportMySqlTables("tag" + MapTime, "agv") + operateDB.ExportMySqlTables("line" + MapTime, "agv") + operateDB.ExportMySqlTables("device" + MapTime, "agv") + operateDB.ExportMySqlTables("widget" + MapTime, "agv") + operateDB.ExportMySqlTables("route" + MapTime, "agv");
                sql = sql + operateDB.ExportTableContents("map", "agv", MapTime.ToString());
                File.WriteAllText(sfd.FileName, sql);
                MessageBox.Show("导出成功!");
            }
        }

    }
}
