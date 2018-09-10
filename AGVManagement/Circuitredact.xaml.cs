using AGV.BLL;
using AGVManagement.instrument;
using AGVManagement.MapPaint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AGVManagement
{
    /// <summary>
    /// Circuitredact.xaml 的交互逻辑
    /// </summary>
    public partial class Circuitredact : Window
    {
        private MapManag manag = new MapManag();
        private TagCompile tag = new TagCompile();
        private OperateDBBLL operate = new OperateDBBLL();
        private double mpWidth, mpHeight;
        private long Times;
        private DataTable dtRoute = new DataTable();
        private MapMessageBLL messageBLL = new MapMessageBLL();
        private int edid = 0;
        public Circuitredact()
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
            SliMax.Value = 0;
            SubmitPro.IsEnabled = false;
            DelPro.IsEnabled = false;
            MapMessageBLL messageBLL = new MapMessageBLL();
            DataTable da = messageBLL.GetMapData(null);
            if (da == null)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = "请选择";
                maplist.Items.Add(item);
                SubmitPro.IsEnabled = false;
                DelPro.IsEnabled = false;
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
        /// 地图选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Maplist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem)maplist.SelectedItem).Tag == null)
            {
                return;
            }
            SliMax.Value = 0;
            SubmitPro.IsEnabled = true;
            DelPro.IsEnabled = true;
            EditlineData.ItemsSource = new DataTable().DefaultView;
            lineRo.Items.Clear();
            MapMessageBLL messageBLL = new MapMessageBLL();
            string ls = ((ComboBoxItem)maplist.SelectedItem).Tag.ToString();
            string[] arr = ls.Split(',');
            dtRoute = messageBLL.BLLMapRoute(arr[2]);
            if (dtRoute.Rows.Count == 0)
            {
                ComboBoxItem item = new ComboBoxItem { Content = "请选择" };
                lineRo.Items.Add(item);
                SubmitPro.IsEnabled = false;
                DelPro.IsEnabled = false;
            }
            else
            {
                ComboBoxItem item = new ComboBoxItem { Content = "请选择" };
                lineRo.Items.Add(item);
                foreach (DataRow data in dtRoute.Rows)
                {
                    ComboBoxItem ite = new ComboBoxItem();
                    ite.Content = data["Name"].ToString();
                    ite.Tag = data["Program"].ToString();
                    lineRo.Items.Add(ite);
                }
            }
            lineRo.SelectedIndex = 0;

            if (!maplist.Text.Equals("请选择") && ((ComboBoxItem)maplist.SelectedItem).Tag.ToString().Split(',').Count().Equals(3))
            {
                //线路
                MapInstrument.keyValuePairs.Clear();
                MapInstrument.valuePairs.Clear();
                MapInstrument.wirePointArrays.Clear();
                MapInstrument.GetKeyValues.Clear();
                Painting.siseWin = 1;
                int six = Convert.ToInt32(SliMax.Value.ToString("G3"));
                Painting.siseWin = six.Equals(0) ? 1 : six;
                MapIN.Children.Clear();
                string lss = ((ComboBoxItem)maplist.SelectedItem).Tag.ToString();
                string[] aarr = lss.Split(',');
                mpWidth = Convert.ToDouble(aarr[0]) * manag.Sise;
                mpHeight = Convert.ToDouble(aarr[1]) * manag.Sise;
                MapIN.Width = mpWidth;
                MapIN.Height = mpHeight;
                Times = long.Parse(aarr[2]);
                manag.Times = Times;
                manag.GetData = EditlineData;
                manag.SelectMap(long.Parse(aarr[2]), MapIN,true);
            }
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
        /// line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Line_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                LineRest();
                if (!(lineRo.SelectedItem as ComboBoxItem).Content.ToString().Equals("请选择") && lineRo.Items.Count - 1 > 0)
                {
                    SubmitPro.IsEnabled = true;
                    DelPro.IsEnabled = true;
                    ProgramNO.IsEnabled = false;
                    ProgramNO.Text = ((ComboBoxItem)lineRo.SelectedItem).Tag.ToString();
                    edid = 1;
                    ProgramName.Text = ((ComboBoxItem)lineRo.SelectedItem).Content.ToString();
                    TagLine(lineRo.SelectedIndex - 1, (lineRo.SelectedItem as ComboBoxItem).Content.ToString());
                }
                else
                {
                    manag.tagType = false;
                    SubmitPro.IsEnabled = false;
                    DelPro.IsEnabled = false;
                    ProgramName.Text = "";
                    EditlineData.ItemsSource = new DataTable().DefaultView;
                    ProgramNO.Text = "0";
                }
            }
            catch
            { ProgramNO.Text = "0"; }
        }

        /// <summary>
        /// 线路还原
        /// </summary>
        public void LineRest()
        {
            MapInstrument map = new MapInstrument();
            map.TagFormer();//所有Tag还原为原色
            foreach (var item in MapInstrument.wirePointArrays)
            {
                if (item.GetPath != null)
                {
                    item.GetPath.Stroke = Brushes.Black;
                    item.GetPath.StrokeThickness = 1;
                }
                List<Path> paths = item.Paths;
                if (paths != null)
                {
                    foreach (Path it in paths)
                    {
                        it.Stroke = Brushes.Black;
                        it.StrokeThickness = 1;
                    }
                }
            }
        }

        /// <summary>
        /// 显示线路信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="LeName"></param>
        public void TagLine(int index, string LeName)
        {
           
            string strTag = dtRoute.Rows[index]["Tag"].ToString();
            string[] Tagar = strTag.Split(',');

            string strSpeed = dtRoute.Rows[index]["Speed"].ToString();
            string[] Speedar = strSpeed.Split(',');

            string stPbs = dtRoute.Rows[index]["Pbs"].ToString();
            string[] Pbsar = stPbs.Split(',');

            string strTurn = dtRoute.Rows[index]["Turn"].ToString();
            string[] Turnar = strTurn.Split(',');

            string strDirection = dtRoute.Rows[index]["Direction"].ToString();
            string[] Directionar = strDirection.Split(',');

            string strHook = dtRoute.Rows[index]["Hook"].ToString();
            string[] Hookar = strHook.Split(',');

            string strStop = dtRoute.Rows[index]["Stop"].ToString();
            string[] Stopar = strStop.Split(',');

            string strChangeProgram = dtRoute.Rows[index]["ChangeProgram"].ToString();
            string[] ChangeProgramar = strChangeProgram.Split(',');

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Tag"));
            dt.Columns.Add(new DataColumn("Speed"));
            dt.Columns.Add(new DataColumn("Pbs"));
            dt.Columns.Add(new DataColumn("Turn"));
            dt.Columns.Add(new DataColumn("Direction"));
            dt.Columns.Add(new DataColumn("Hook"));
            dt.Columns.Add(new DataColumn("Stop"));
            dt.Columns.Add(new DataColumn("ChangeProgram"));
            for (int i = 0; i < Tagar.Length; i++)
            {
                dt.Rows.Add(new object[] { Tagar[i], TagCompile.agvSpeed[Convert.ToInt32(Speedar[i])], TagCompile.agvPbs[Convert.ToInt32(Pbsar[i])], TagCompile.agvTurn[Convert.ToInt32(Turnar[i])], TagCompile.agvDire[Convert.ToInt32(Directionar[i])], TagCompile.agvHook[Convert.ToInt32(Hookar[i])], Stopar[i], ChangeProgramar[i] });
            }
            if (Tagar.Count() > 0)
            {
                GetScroll.ScrollToHorizontalOffset(MapInstrument.valuePairs[Convert.ToInt32(Tagar[0])].Margin.Left - 600);//滚动条X轴跟随移动
                GetScroll.ScrollToVerticalOffset(MapInstrument.valuePairs[Convert.ToInt32(Tagar[0])].Margin.Top - 600); ///滚动条Y轴等随移动
            }
            EditlineData.ItemsSource = dt.DefaultView;
            EditlineData.AutoGenerateColumns = false;
            manag.table = dt;
            manag.tagType = true;
            manag.TagCic();
            manag.lineMap.TagClick(Times, Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Tag"]), EditlineData, dt,false);
        }

        /// <summary>
        /// 表格点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditlineData_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                if (EditlineData.SelectedItems.Count > 0)
                {
                    List<object> arr = ((DataRowView)EditlineData.SelectedValue).Row.ItemArray.ToList();
                    manag.LineMapShow(arr, (EditlineData.SelectedIndex == 0 ? true : false), (EditlineData.SelectedIndex == 0 ? Convert.ToInt32(((DataRowView)EditlineData.SelectedItem)["Tag"]) : Convert.ToInt32(((DataRowView)EditlineData.Items[EditlineData.SelectedIndex - 1])["Tag"])), EditlineData.SelectedIndex);
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelPro_Click(object sender, RoutedEventArgs e)
        {
            if (!edid.Equals(0))
            {
                MessageBoxResult confirmToDel = MessageBox.Show("确认要删除线路吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirmToDel == MessageBoxResult.Yes)
                {
                    if (messageBLL.DelRouteMap(Times, Convert.ToInt32(ProgramNO.Text.Trim())))
                    {
                        MessageBox.Show("删除成功");
                        AddPro_Click(null, null);
                        Maplist_SelectionChanged(null, null);
                    }
                    else
                    {
                        MessageBox.Show("删除失败");
                    }
                }
            }
            else
            {
                AddPro_Click(null, null);
            }
           
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPro_Click(object sender, RoutedEventArgs e)
        {
            LineRest();
            SubmitPro.IsEnabled = true;
            ProgramNO.IsEnabled = true;
            DelPro.IsEnabled = true;
            ProgramName.Text = "";
            ProgramNO.Text = "0";
            edid = 0;
            DataTable dr = new DataTable();
            EditlineData.ItemsSource = dr.DefaultView;
            dr.Columns.Add(new DataColumn("Tag"));
            dr.Columns.Add(new DataColumn("Speed"));
            dr.Columns.Add(new DataColumn("Pbs"));
            dr.Columns.Add(new DataColumn("Turn"));
            dr.Columns.Add(new DataColumn("Direction"));
            dr.Columns.Add(new DataColumn("Hook"));
            dr.Columns.Add(new DataColumn("Stop"));
            dr.Columns.Add(new DataColumn("ChangeProgram"));
            manag.table = dr;
            manag.tagType = true;
            manag.TagCic();
        }

        /// <summary>
        /// 匹配是否为数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsFloat(string str)
        {
            string regextext = @"^(-?\d+)(\.\d+)?$";
            Regex regex = new Regex(regextext, RegexOptions.None);
            return regex.IsMatch(str.Trim());
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitPro_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ProgramNO.Text) || string.IsNullOrEmpty(Convert.ToString(ProgramName.Text)))
            {
                MessageBox.Show("请输入线路名称及Program号");
                return;
            }
            if (!IsFloat(ProgramNO.Text.Trim()))
            {
                MessageBox.Show("Program号只能为数字");
                return;
            }
            else if (EditlineData.Items.Count == 0)
            {
                MessageBox.Show("未编辑线路");
                return;
            }
            StringBuilder sbTag = new StringBuilder();
            StringBuilder sbSpeed = new StringBuilder();
            StringBuilder sbStop = new StringBuilder();
            StringBuilder sbTurn = new StringBuilder();
            StringBuilder sbDirection = new StringBuilder();
            StringBuilder sbPbs = new StringBuilder();
            StringBuilder sbHook = new StringBuilder();
            StringBuilder sbProgram = new StringBuilder();

            for (int i = 0; i < EditlineData.Items.Count; i++)
            {
                sbTag.Append(((DataRowView)EditlineData.Items[i])[0]);
                sbTag.Append(",");
                sbSpeed.Append(tag.agvSpeedIndex(((DataRowView)EditlineData.Items[i])[1].ToString()));
                sbSpeed.Append(",");
                sbStop.Append(((DataRowView)EditlineData.Items[i])[6]);
                sbStop.Append(",");
                sbTurn.Append(tag.agvTurnIndex(((DataRowView)EditlineData.Items[i])[3].ToString()));
                sbTurn.Append(",");
                sbDirection.Append(tag.agvDireIndex(((DataRowView)EditlineData.Items[i])[4].ToString()));
                sbDirection.Append(",");
                sbPbs.Append(tag.agvPbsIndex(((DataRowView)EditlineData.Items[i])[2].ToString()));
                sbPbs.Append(",");
                sbHook.Append(tag.agvHookIndex(((DataRowView)EditlineData.Items[i])[5].ToString()));
                sbHook.Append(",");
                sbProgram.Append(((DataRowView)EditlineData.Items[i])[7]);
                sbProgram.Append(",");
            }

            sbTag.Remove(sbTag.Length - 1, 1);
            sbSpeed.Remove(sbSpeed.Length - 1, 1);
            sbStop.Remove(sbStop.Length - 1, 1);
            sbTurn.Remove(sbTurn.Length - 1, 1);
            sbDirection.Remove(sbDirection.Length - 1, 1);
            sbPbs.Remove(sbPbs.Length - 1, 1);
            sbHook.Remove(sbHook.Length - 1, 1);
            sbProgram.Remove(sbProgram.Length - 1, 1);

            string tagStr = sbTag.ToString();
            string speedStr = sbSpeed.ToString();
            string stopStr = sbStop.ToString();
            string turnStr = sbTurn.ToString();
            string direStr = sbDirection.ToString();
            string pbsStr = sbPbs.ToString();
            string hookStr = sbHook.ToString();
            string programStr = sbProgram.ToString();

            string agvStr = "";//地图上不用注册agv，为保证程序正常运行保留字段。
            if (edid.Equals(0))
            {
                if (messageBLL.Program(ProgramNO.Text.Trim(), Times))
                {
                    MessageBox.Show("Program已存在，请重新输入Program");
                    return;
                }
                else
                {
                    if (messageBLL.InsertRouteMap(ProgramNO.Text.Trim(), ProgramName.Text.Trim(), UTC.ConvertDateTimeLong(DateTime.Now), Times, tagStr, speedStr, stopStr, turnStr, direStr, pbsStr, hookStr, agvStr, programStr))
                    {
                        MessageBox.Show("保存成功");
                        Maplist_SelectionChanged(null, null);
                    }
                    else
                    {
                        MessageBox.Show("保存失败");
                    }
                }
            }
            else if (edid.Equals(1))
            {
                if (messageBLL.UpdateRouteMap(Times, Convert.ToInt32(ProgramNO.Text.Trim()), ProgramName.Text.Trim(), tagStr, speedStr, stopStr, turnStr, direStr, pbsStr, hookStr, agvStr, programStr))
                {
                    MessageBox.Show("保存成功");
                    Maplist_SelectionChanged(null, null);
                }
                else
                {
                    MessageBox.Show("保存失败");
                }
            }
        }
    }
}
