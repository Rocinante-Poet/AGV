using AGV.BLL;
using AGVManagement.instrument;
using AGVManagement.MapPaint;
using AGVManagement.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Threading;

namespace AGVManagement
{
    /// <summary>
    /// BeaconRedact.xaml 的交互逻辑
    /// </summary>
    public partial class BeaconRedact : Window
    {
        MapManag manag = new MapManag();
        TagCompile tag = new TagCompile();
        OperateDBBLL operate = new OperateDBBLL();
        private double mpWidth, mpHeight;
        private long Times;
        public BeaconRedact()
        {
            InitializeComponent();
            MapLoad();
        }

        /// <summary>
        /// 载入地图信息
        /// </summary>
        private void MapLoad()
        {
            MapInstrument.keyValuePairs.Clear();
            MapInstrument.valuePairs.Clear();
            MapInstrument.wirePointArrays.Clear();
            MapInstrument.GetKeyValues.Clear();
            Painting.siseWin = 1;
            MapMessageBLL messageBLL = new MapMessageBLL();
            DataTable da = messageBLL.GetMapData(null);
            if (da == null)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = "请选择";
                maplist.Items.Add(item);
            }
            else
            {
                foreach (DataRow data in da.Rows)
                {
                    ComboBoxItem ite = new ComboBoxItem();
                    ite.Content = data["Name"].ToString();
                    ite.Tag = data["Width"].ToString() + "," + data["Height"].ToString() + "," + data["CreateTime"].ToString();
                    maplist.Items.Add(ite);
                }
            }
            maplist.SelectedIndex = 0;
        }

        /// <summary>
        /// 地图缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SliMax_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Painting painting = new Painting();
            painting.mainPan = MapIN;
            int sis = Convert.ToInt32(e.NewValue);
            MapIN.Children.Clear();
            if (sis.Equals(0))
            {
                MapIN.Width = mpWidth * 1;
                MapIN.Height = mpHeight * 1;
                painting.Zoom(1);
                Painting.siseWin = 1;
            }
            else
            {
                MapIN.Width = mpWidth * sis;
                MapIN.Height = mpHeight * sis;
                painting.Zoom(sis);
                Painting.siseWin = sis;
            }
        }

        /// <summary>
        /// 表格点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Beacon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                if (Beacon.SelectedItems.Count > 0)
                {
                    List<object> arr = ((DataRowView)Beacon.SelectedValue).Row.ItemArray.ToList();
                    TagMessage message = new TagMessage(arr, Times,Beacon, ScrollMap);
                    message.Show();
                }
            }

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            OperateDBBLL operate = new OperateDBBLL();

            DataTable dr = new DataTable();
            for (int i = 0; i < Beacon.Columns.Count; i++)
            {
                DataColumn dc = new DataColumn();
                dr.Columns.Add(dc);
            }
            for (int i = 0; i < Beacon.Items.Count; i++)
            {
                DataRow dt = dr.NewRow();
                for (int j = 0; j < Beacon.Columns.Count; j++)
                {
                    if (j == 7 || j == 8)
                    {
                        dt[j] = tag.agvSpeedIndex(((DataRowView)Beacon.Items[i])[j].ToString().Trim());
                    }
                    else if (j == 10 || j == 11)
                    {
                        dt[j] = tag.agvPbsIndex(((DataRowView)Beacon.Items[i])[j].ToString().Trim());
                    }
                    else
                    {
                        dt[j] = ((DataRowView)Beacon.Items[i])[j].ToString().Trim();
                    }     
                }
                dr.Rows.Add(dt);
            }
            if (operate.UpdateTagDtat(Times, dr))
            {
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("保存失败");
            }
        }

        /// <summary>
        /// 地图选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void maplist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!maplist.Text.Equals("请选择") && ((ComboBoxItem)maplist.SelectedItem).Tag!=null)
            {
                MapInstrument.keyValuePairs.Clear();
                MapInstrument.valuePairs.Clear();
                MapInstrument.wirePointArrays.Clear();
                MapInstrument.GetKeyValues.Clear();
                Painting.siseWin = 1;
                SliMax.Value = 0;
                MapIN.Children.Clear();
                string ls = ((ComboBoxItem)maplist.SelectedItem).Tag.ToString();
                string[] arr = ls.Split(',');
                mpWidth = Convert.ToDouble(arr[0]) * manag.Sise;
                mpHeight = Convert.ToDouble(arr[1]) * manag.Sise;
                MapIN.Width = mpWidth;
                MapIN.Height = mpHeight;
                Times = long.Parse(arr[2]);
                manag.SelectMap(long.Parse(arr[2]), MapIN,false);
                TagCompile tag = new TagCompile();
                tag.TagManagement(Beacon, arr[2]);
            }
        }
    }
}
