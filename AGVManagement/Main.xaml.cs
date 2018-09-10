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
using AGV.BLL;
using AGVManagement.MapPaint;
using System.Threading;
using System.IO.Ports;
using AGV.Models.Models;
using AGVDLL;
using System.Windows.Controls.Primitives;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace AGVManagement
{
    /// <summary>
    /// Main.xaml 的交互逻辑
    /// </summary>
    public partial class Main : Window
    {
        private OperateDBBLL dBBLL = new OperateDBBLL();
        private MapMessageBLL mapMessage = new MapMessageBLL();
        private bool mapSelect = false; //地图加载标志位
        private string selAgv = "1"; //默认显示AGV
        private MapManag manag = new MapManag();

        public Main()
        {
            InitializeComponent();
            AgvInfo();
            LoadMap();
        }

        #region ==========初始化数据库=========

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public void LoadDB()
        {
            dBBLL.CreateDBMap();
        }

        #endregion ==========初始化数据库=========

        #region ==========载入地图信息=========

        public void LoadMap()
        {
            Thread thread = new Thread(() =>
            { LoadDB(); MapLoad(); });
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 载入地图信息
        /// </summary>
        private void MapLoad()
        {
            MapMessageBLL messageBLL = new MapMessageBLL();
            DataTable da = messageBLL.GetMapData(null);
            string Times = mapMessage.SettingInfoMap();
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (da == null)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = "请选择";
                    Maplistq.Items.Add(item);
                    Maplistq.SelectedIndex = 0;
                }
                else
                {
                    int Index = 0;
                    int s = 0;
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = "请选择";
                    Maplistq.Items.Add(item);
                    foreach (DataRow data in da.Rows)
                    {
                        if (Times != null)
                        {
                            if (Times.Equals(data["CreateTime"].ToString()))
                            {
                                Index = s;
                            }
                        }
                        ComboBoxItem ite = new ComboBoxItem();
                        ite.Content = data["Name"].ToString();
                        ite.Tag = data["Width"].ToString() + "," + data["Height"].ToString() + "," + data["CreateTime"].ToString();
                        Maplistq.Items.Add(ite);
                        s++;
                    }
                    mapSelect = true;
                    Maplistq.SelectedIndex = Index + 1;
                }
            }));
        }

        #endregion ==========载入地图信息=========

        #region ==============菜单=============

        /// <summary>
        /// 线路编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Circui_Click(object sender, RoutedEventArgs e)
        {
            Circuitredact circuitredact = new Circuitredact();
            circuitredact.ShowDialog();
        }

        /// <summary>
        /// 串口设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortDern_Click(object sender, RoutedEventArgs e)
        {
            PortSetting port = new PortSetting(this);
            port.ShowDialog();
        }

        /// <summary>
        /// 添加地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Map_Add_Click(object sender, RoutedEventArgs e)
        {
            AddMap map = new AddMap();
            map.ShowDialog();
        }

        /// <summary>
        /// 编辑信标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeaconCR_Click(object sender, RoutedEventArgs e)
        {
            BeaconRedact beacon = new BeaconRedact();
            beacon.ShowDialog();
        }

        /// <summary>
        /// 编辑地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Map_btn_Click(object sender, RoutedEventArgs e)
        {
            Map mainWindow = new Map();
            mainWindow.ShowDialog();
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnset_Click(object sender, RoutedEventArgs e)
        {
            OpenPort();
        }

        #endregion ==============菜单=============

        #region =========载入AGV初始信息=======

        /// <summary>
        /// 载入所有串口信息
        /// </summary>
        /// <param name="Times"></param>
        private void ComInfo(long Times)
        {
            Empty();
            DataTable dt = mapMessage.LoadDeviceMap(Times);
            DataTable data = new DataTable("TabAgvInfo");
            data.Columns.Add("串口", typeof(String));
            data.Columns.Add("信息", typeof(String));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    data.Rows.Add(new object[] { "COM", "" + item["Com"].ToString() + "" });
                    data.Rows.Add(new object[] { "波特率", item["Baud"].ToString() });
                    if (item["Agv"].ToString() == "Button")
                    {
                        data.Rows.Add(new object[] { "AGV / 其他", "按钮" });
                        //PortInfo.buttonPort.Add(new SerialPort());
                        PortInfo.buttonCom.Add(Convert.ToInt32(item["Com"].ToString()));
                        PortInfo.buttonBaud.Add(Convert.ToInt32(item["Baud"].ToString()));
                        PortInfo.buttonStr.Add("Button");
                    }
                    else if (item["Agv"].ToString() == "Charge")
                    {
                        data.Rows.Add(new object[] { "AGV / 其他", "充电机" });
                        //PortInfo.chargePort.Add(new SerialPort());
                        PortInfo.chargeCom.Add(Convert.ToInt32(item["Com"].ToString()));
                        PortInfo.chargeBaud.Add(Convert.ToInt32(item["Baud"].ToString()));
                        PortInfo.chargeStr.Add("Charge");
                    }
                    else
                    {
                        data.Rows.Add(new object[] { "AGV / 其他", item["Agv"].ToString() });
                        PortInfo.AGVCom.Add(Convert.ToInt32(item["Com"].ToString()));
                        PortInfo.Baud.Add(Convert.ToInt32(item["Baud"].ToString()));
                        PortInfo.agv.Add((item["Agv"].ToString()));
                    }
                    data.Rows.Add(new object[] { "状态", "关闭" });
                }
            }
            else
            {
                data.Rows.Add(new object[] { "COM", "" });
                data.Rows.Add(new object[] { "波特率", "" });
                data.Rows.Add(new object[] { "AGV / 其他", "" });
                data.Rows.Add(new object[] { "状态", "" });
            }
            TabSerialPortData.ItemsSource = data.DefaultView;
            TabSerialPortData.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
            TabSerialPortData.HeadersVisibility = DataGridHeadersVisibility.None;
        }

        /// <summary>
        /// 显示所有AGV初始信息
        /// </summary>
        /// <param name="Time"></param>
        public void TabAgvMoveInfo(long Time)
        {
            List<string> Agvlist = dBBLL.AgvNumListMap(Time);
            DataTable dt = new DataTable("TabAgvMoveInfo");
            dt.Columns.Add(new DataColumn("type"));
            dt.Columns.Add(new DataColumn("TagName"));
            dt.Columns.Add(new DataColumn("Speed"));
            dt.Columns.Add(new DataColumn("turn"));
            dt.Columns.Add(new DataColumn("Dir"));
            dt.Columns.Add(new DataColumn("Hook"));
            dt.Columns.Add(new DataColumn("Rfid"));
            dt.Columns.Add(new DataColumn("Program"));
            dt.Columns.Add(new DataColumn("Step"));

            for (int i = 0; i < Agvlist.Count; i++)
            {
                dt.Rows.Add(new object[] { "离线", Agvlist[i], "", "", "", "", "", "" });
                MainInfo.agvNo.Add(Agvlist[i]);
            }
            if (Agvlist.Count > 0)
            {
                selAgv = Agvlist[0];
            }
            TabAgvMoveData.DataContext = dt.DefaultView;
            TabAgvMoveData.AutoGenerateColumns = false;
            Open.IsEnabled = true;
        }

        /// <summary>
        ///加载单一agv初始信息
        /// </summary>
        public void AgvInfo()
        {
            DataTable AgvData = new DataTable("TabAgvInfo");
            AgvData.Columns.Add("Agv", typeof(String));
            AgvData.Columns.Add("信息", typeof(String));
            AgvData.Rows.Add(new object[] { "AGV", "" });
            AgvData.Rows.Add(new object[] { "网络状态", "" });
            AgvData.Rows.Add(new object[] { "运行状态", "" });
            AgvData.Rows.Add(new object[] { "运行准备", "" });
            AgvData.Rows.Add(new object[] { "驱动下降", "" });
            AgvData.Rows.Add(new object[] { "脱轨", "" });
            AgvData.Rows.Add(new object[] { "扫描区域", "" });
            AgvData.Rows.Add(new object[] { "电压", "" });
            AgvData.Rows.Add(new object[] { "报警", "" });
            AgvData.Rows.Add(new object[] { "报警信息", "" });
            TabAgvData.ItemsSource = AgvData.DefaultView;
            TabAgvData.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
            TabAgvData.HeadersVisibility = DataGridHeadersVisibility.None;
        }

        #endregion =========载入AGV初始信息=======

        #region ===========地图选项更改========

        /// <summary>
        /// 地图选项
        /// </summary>
        public void Maplist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mapSelect)
            {
                if (!((ComboBoxItem)Maplistq.SelectedItem).Content.Equals("请选择") && ((ComboBoxItem)Maplistq.SelectedItem).Tag != null)
                {
                    MapIN.Children.Clear();
                    manag.Sise = 16;
                    Open.IsEnabled = false;
                    string ls = ((ComboBoxItem)Maplistq.SelectedItem).Tag.ToString();
                    string[] arr = ls.Split(',');
                    double mpWidth = Convert.ToDouble(arr[0]) * manag.Sise;
                    double mpHeight = Convert.ToDouble(arr[1]) * manag.Sise;
                    MapIN.Width = mpWidth;
                    MapIN.Height = mpHeight;
                    long Times = long.Parse(arr[2]);
                    ComInfo(Times);
                    manag.SelectMapLOad(long.Parse(arr[2]), MapIN);
                    TabAgvMoveInfo(long.Parse(arr[2]));
                }
            }
        }

        #endregion ===========地图选项更改========

        #region =======清空所有AGV信息=========

        /// <summary>
        /// 清空所有AGV信息
        /// </summary>
        public void DataGridCrear()
        {
            for (int i = 0; i < TabAgvMoveData.Items.Count; i++)
            {
                for (int s = 0; s < TabAgvMoveData.Columns.Count; s++)
                {
                    if (s.Equals(0) || s.Equals(1))
                    {
                        if (s.Equals(0))
                        {
                            ((DataRowView)TabAgvMoveData.Items[i])[s] = "离线";
                            DataGridTemplateColumn tempColumn = this.TabAgvMoveData.Columns[0] as DataGridTemplateColumn;
                            FrameworkElement element = this.TabAgvMoveData.Columns[0].GetCellContent(this.TabAgvMoveData.Items[i]);
                            if (element != null)
                            {
                                CheckBox ck = tempColumn.CellTemplate.FindName("CheckBoxDN", element) as CheckBox;
                                ck.Foreground = Brushes.Red;
                                Style btn_style = (Style)this.FindResource("checkbox has-error");
                                ck.Style = btn_style;
                            }
                        }
                        continue;
                    }
                    ((DataRowView)TabAgvMoveData.Items[i])[s] = "";
                }
            }
        }

        /// <summary>
        /// 清空AGV信息
        /// </summary>
        public void AgvCror()
        {
            ((DataRowView)TabAgvData.Items[0])[1] = "";
            ((DataRowView)TabAgvData.Items[1])[1] = "";
            ((DataRowView)TabAgvData.Items[2])[1] = "";
            ((DataRowView)TabAgvData.Items[3])[1] = "";
            ((DataRowView)TabAgvData.Items[4])[1] = "";
            ((DataRowView)TabAgvData.Items[5])[1] = "";
            ((DataRowView)TabAgvData.Items[6])[1] = "";
            ((DataRowView)TabAgvData.Items[7])[1] = "";
            ((DataRowView)TabAgvData.Items[8])[1] = "";
            ((DataRowView)TabAgvData.Items[9])[1] = "";
        }

        #endregion =======清空所有AGV信息=========

        #region ===========AGV状态更新=========

        /// <summary>
        /// 单一AGV状态更新
        /// </summary>
        /// <param name="i"></param>
        private void AgvDgvInfo(int i)
        {
            if (Convert.ToInt32(selAgv) == i)
            {
                if (MainInfo.carStatusList[i].errorCode == 0 && MainInfo.carStatusList[i].carNum == 0)
                {
                    ((DataRowView)TabAgvData.Items[0])[1] = selAgv;
                    ((DataRowView)TabAgvData.Items[1])[1] = "连接中";
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        (TabAgvData.Columns[1].GetCellContent(TabAgvData.Items[1]) as TextBlock).Foreground = Brushes.Green;
                    }));
                    ((DataRowView)TabAgvData.Items[2])[1] = "";
                    ((DataRowView)TabAgvData.Items[3])[1] = "";
                    ((DataRowView)TabAgvData.Items[4])[1] = "";
                    ((DataRowView)TabAgvData.Items[5])[1] = "";
                    ((DataRowView)TabAgvData.Items[6])[1] = "";
                    ((DataRowView)TabAgvData.Items[7])[1] = "";
                    ((DataRowView)TabAgvData.Items[8])[1] = "";
                    ((DataRowView)TabAgvData.Items[9])[1] = "";
                }
                else if (MainInfo.carStatusList[i].errorCode == 205)
                {
                    ((DataRowView)TabAgvData.Items[0])[1] = selAgv;
                    ((DataRowView)TabAgvData.Items[1])[1] = "离线！！！";
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        (TabAgvData.Columns[1].GetCellContent(TabAgvData.Items[1]) as TextBlock).Foreground = Brushes.Red;
                    }));
                    ((DataRowView)TabAgvData.Items[2])[1] = "";
                    ((DataRowView)TabAgvData.Items[3])[1] = "";
                    ((DataRowView)TabAgvData.Items[4])[1] = "";
                    ((DataRowView)TabAgvData.Items[5])[1] = "";
                    ((DataRowView)TabAgvData.Items[6])[1] = "";
                    ((DataRowView)TabAgvData.Items[7])[1] = "";
                    ((DataRowView)TabAgvData.Items[8])[1] = "";
                    ((DataRowView)TabAgvData.Items[9])[1] = "";
                }
                else
                {
                    ((DataRowView)TabAgvData.Items[0])[1] = MainInfo.carStatusList[i].carNum;
                    ((DataRowView)TabAgvData.Items[1])[1] = "在线";
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        (TabAgvData.Columns[1].GetCellContent(TabAgvData.Items[1]) as TextBlock).Foreground = Brushes.Green;
                    }));
                    if (MainInfo.carStatusList[i].IsRunning)
                    {
                        ((DataRowView)TabAgvData.Items[2])[1] = "行进中";
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            (TabAgvData.Columns[1].GetCellContent(TabAgvData.Items[2]) as TextBlock).Foreground = Brushes.Green;
                        }));
                    }
                    else
                    {
                        ((DataRowView)TabAgvData.Items[2])[1] = "停止";
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            (TabAgvData.Columns[1].GetCellContent(TabAgvData.Items[2]) as TextBlock).Foreground = Brushes.Red;
                        }));
                    }

                    if (MainInfo.carStatusList[i].agvRunReady)
                    {
                        ((DataRowView)TabAgvData.Items[3])[1] = "On";
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            (TabAgvData.Columns[1].GetCellContent(TabAgvData.Items[3]) as TextBlock).Foreground = Brushes.Green;
                        }));
                    }
                    else
                    {
                        ((DataRowView)TabAgvData.Items[3])[1] = "Off";
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            (TabAgvData.Columns[1].GetCellContent(TabAgvData.Items[3]) as TextBlock).Foreground = Brushes.Red;
                        }));
                    }

                    if (MainInfo.carStatusList[i].agvDriverDown)
                    {
                        ((DataRowView)TabAgvData.Items[4])[1] = "驱动下降";
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            (TabAgvData.Columns[1].GetCellContent(TabAgvData.Items[4]) as TextBlock).Foreground = Brushes.Green;
                        }));
                    }
                    else
                    {
                        ((DataRowView)TabAgvData.Items[4])[1] = "驱动上升";
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            (TabAgvData.Columns[1].GetCellContent(TabAgvData.Items[4]) as TextBlock).Foreground = Brushes.Red;
                        }));
                    }
                    if (MainInfo.carStatusList[i].agvLineRead)
                    {
                        ((DataRowView)TabAgvData.Items[5])[1] = "正常";
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            (TabAgvData.Columns[1].GetCellContent(TabAgvData.Items[5]) as TextBlock).Foreground = Brushes.Green;
                        }));
                    }
                    else
                    {
                        ((DataRowView)TabAgvData.Items[5])[1] = "脱轨";
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            (TabAgvData.Columns[1].GetCellContent(TabAgvData.Items[5]) as TextBlock).Foreground = Brushes.Red;
                        }));
                    }

                    ((DataRowView)TabAgvData.Items[6])[1] = MainInfo.carStatusList[i].pbsArea;
                    ((DataRowView)TabAgvData.Items[7])[1] = MainInfo.carStatusList[i].powerCurrentF + "V";
                    if (MainInfo.carStatusList[i].errorSwitch == false)
                    {
                        ((DataRowView)TabAgvData.Items[8])[1] = "正常";
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            (TabAgvData.Columns[1].GetCellContent(TabAgvData.Items[8]) as TextBlock).Foreground = Brushes.Green;
                        }));
                    }
                    else
                    {
                        ((DataRowView)TabAgvData.Items[8])[1] = "报警！！！";
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            (TabAgvData.Columns[1].GetCellContent(TabAgvData.Items[8]) as TextBlock).Foreground = Brushes.Red;
                        }));
                    }
                   ((DataRowView)TabAgvData.Items[9])[1] = Error.errorStr(MainInfo.carStatusList[i].errorCode);
                }
            }
        }

        /// <summary>
        /// 所有AGV状态更新
        /// </summary>
        public void AgvMessg()
        {
            while (MainInfo.AgvStaticMessg)
            {
                PortMessg();
                for (int i = 0; i < TabAgvMoveData.Items.Count; i++)
                {
                    if (MainInfo.carStatusList.Count > 0)
                    {
                        CarStatus car = AgcCarExists(Convert.ToInt32(((DataRowView)TabAgvMoveData.Items[i])[1]));
                        if (car != null)
                        {
                            AgvDgvInfo(Convert.ToInt32(((DataRowView)TabAgvMoveData.Items[i])[1]));
                            if (car.errorCode == 0 && car.carNum == 0)
                            {
                                ((DataRowView)TabAgvMoveData.Items[i])[0] = "连接中";
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                    DataGridTemplateColumn tempColumn = this.TabAgvMoveData.Columns[0] as DataGridTemplateColumn;
                                    //然后获取DataGridTemplateColumn单元格元素
                                    FrameworkElement element = this.TabAgvMoveData.Columns[0].GetCellContent(this.TabAgvMoveData.Items[i]);
                                    if (element != null)
                                    { //把单元格元素转换为相应的控件，再从该控件中取值
                                        CheckBox ck = tempColumn.CellTemplate.FindName("CheckBoxDN", element) as CheckBox;
                                        ck.Foreground = Brushes.Red;
                                        Style btn_style = (Style)this.FindResource("checkbox has-error");
                                        ck.Style = btn_style;
                                    }
                                }));
                            }
                            else if (car.errorCode == 205)
                            {
                                ((DataRowView)TabAgvMoveData.Items[i])[0] = "离线";
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                    DataGridTemplateColumn tempColumn = this.TabAgvMoveData.Columns[0] as DataGridTemplateColumn;
                                    FrameworkElement element = this.TabAgvMoveData.Columns[0].GetCellContent(this.TabAgvMoveData.Items[i]);
                                    if (element != null)
                                    {
                                        CheckBox ck = tempColumn.CellTemplate.FindName("CheckBoxDN", element) as CheckBox;
                                        ck.Foreground = Brushes.Red;
                                        Style btn_style = (Style)this.FindResource("checkbox has-error");
                                        ck.Style = btn_style;
                                    }
                                }));
                            }
                            else
                            {
                                ((DataRowView)TabAgvMoveData.Items[i])[0] = "在线";
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                    DataGridTemplateColumn tempColumn = this.TabAgvMoveData.Columns[0] as DataGridTemplateColumn;
                                    FrameworkElement element = this.TabAgvMoveData.Columns[0].GetCellContent(this.TabAgvMoveData.Items[i]);
                                    if (element != null)
                                    {
                                        CheckBox ck = tempColumn.CellTemplate.FindName("CheckBoxDN", element) as CheckBox;
                                        ck.Foreground = Brushes.Green;
                                        Style btn_style = (Style)this.FindResource("checkbox has-success");
                                        ck.Style = btn_style;
                                    }
                                }));
                            }
                            if (car.errorCode != 205 && car.carNum != 0)
                            {
                                ((DataRowView)TabAgvMoveData.Items[i])[2] = TagCompile.agvSpeed[car.speedNo] + "米/分钟";
                                if (car.agvRunRight.Equals(true))
                                {
                                    ((DataRowView)TabAgvMoveData.Items[i])[3] = "右转中";
                                }
                                else if (car.agvRunLeft.Equals(true))
                                {
                                    ((DataRowView)TabAgvMoveData.Items[i])[3] = "左转中";
                                }
                                else
                                {
                                    ((DataRowView)TabAgvMoveData.Items[i])[3] = "直行";
                                }
                                ((DataRowView)TabAgvMoveData.Items[i])[4] = car.agvRunDirection ? "正向" : "反向";
                                if (car.agvHookUP.Equals(true))
                                {
                                    ((DataRowView)TabAgvMoveData.Items[i])[5] = "上升";
                                }
                                else
                                {
                                    ((DataRowView)TabAgvMoveData.Items[i])[5] = "下降";
                                }
                                ((DataRowView)TabAgvMoveData.Items[i])[6] = string.IsNullOrEmpty(car.rfidStatus) ? "无" : car.rfidStatus;
                                ((DataRowView)TabAgvMoveData.Items[i])[7] = car.programNo;
                                ((DataRowView)TabAgvMoveData.Items[i])[8] = car.stepNo;
                            }
                        }
                    }
                }
                Thread.Sleep(200);
            }
        }

        /// <summary>
        /// 串口状态更新
        /// </summary>
        public void PortMessg()
        {
            int Index = 0;
            for (int i = 0; i < PortInfo.AGVCom.Count; i++)
            {
                ((DataRowView)TabSerialPortData.Items[Index])[1] = PortInfo.AGVCom[i];
                Index++;
                ((DataRowView)TabSerialPortData.Items[Index])[1] = PortInfo.Baud[i];
                Index++;
                ((DataRowView)TabSerialPortData.Items[Index])[1] = PortInfo.agv[i];
                Index++;
                ((DataRowView)TabSerialPortData.Items[Index])[1] = MainInfo.listPtr[i].ToInt32().Equals(0) ? "关闭" : "打开";
                Index++;
            }
            for (int i = 0; i < PortInfo.buttonCom.Count; i++)
            {
                ((DataRowView)TabSerialPortData.Items[Index])[1] = PortInfo.buttonCom[i];
                Index++;
                ((DataRowView)TabSerialPortData.Items[Index])[1] = PortInfo.buttonBaud[i];
                Index++;
                ((DataRowView)TabSerialPortData.Items[Index])[1] = PortInfo.buttonStr[i];
                Index++;
                ((DataRowView)TabSerialPortData.Items[Index])[1] = "关闭";
                this.Dispatcher.Invoke(new Action(() =>
                {
                    (TabSerialPortData.Columns[1].GetCellContent(TabSerialPortData.Items[Index]) as TextBlock).Foreground = Brushes.Red;
                }));
                Index++;
            }
            for (int i = 0; i < PortInfo.chargeCom.Count; i++)
            {
                ((DataRowView)TabSerialPortData.Items[Index])[1] = PortInfo.AGVCom[i];
                Index++;
                ((DataRowView)TabSerialPortData.Items[Index])[1] = PortInfo.chargeBaud[i];
                Index++;
                ((DataRowView)TabSerialPortData.Items[Index])[1] = PortInfo.chargeStr[i];
                Index++;
                ((DataRowView)TabSerialPortData.Items[Index])[1] = "关闭";
                this.Dispatcher.Invoke(new Action(() =>
                {
                    (TabSerialPortData.Columns[1].GetCellContent(TabSerialPortData.Items[Index]) as TextBlock).Foreground = Brushes.Red;
                }));
            }
        }

        #endregion ===========AGV状态更新=========

        #region ===========AGV状态回读=========

        /// <summary>
        /// AGV状态回读
        /// </summary>
        public void AgvBackward(object groupNo)
        {
            int groupNumber = Convert.ToInt32(groupNo);
            int Index = groupNumber - 1;
            string[] comAgv = PortInfo.agv[Index].Split(',');
            for (int i = 0; i < comAgv.Length; i++)
            {
                MainInfo.carStatusList.Add(Convert.ToInt32(comAgv[i]), new CarStatus());
            }
            while (MainInfo.agvThState)
            {
                MainInfo.listAgvDll[Index].agvPortClearCache(groupNumber);//清除agv串口缓存
                for (int i = 0; i < comAgv.Length; i++)
                {
                    CarStatus carStatus = new CarStatus();
                    carStatus = MainInfo.listAgvDll[Index].read(MainInfo.listPtr[Index], groupNumber, Convert.ToInt32(comAgv[i]));
                    MainInfo.carStatusList[Convert.ToInt32(comAgv[i])] = carStatus;
                }
                Thread.Sleep(200);
            }
        }

        #endregion ===========AGV状态回读=========

        #region ==========串口打开关闭=========

        /// <summary>
        /// 打开串口
        /// </summary>
        public void OpenPort()
        {
            AGVClear();
            int ConNum = PortInfo.AGVCom.Count;
            bool openStatic = true;//打开状态
            for (int i = 0; i < ConNum; i++)
            {
                AGVDLL.AGVDLL agvDLL = new AGVDLL.AGVDLL();
                agvDLL.dllName = i.ToString();
                int groupNo = i + 1;
                IntPtr result = agvDLL.openPort(groupNo, PortInfo.AGVCom[i], PortInfo.Baud[i], MainInfo.prity, MainInfo.stopBits);
                int a = result.ToInt32();
                if (result.ToInt32() == 0)
                {
                    openStatic = false;
                    MessageBox.Show("打开串口：COM" + PortInfo.AGVCom[i] + "失败！");
                    break;
                }
                else
                {
                    MainInfo.listAgvDll.Add(agvDLL);
                    MainInfo.listPtr.Add(result);
                    Thread thread = new Thread(new ParameterizedThreadStart(AgvBackward));
                    thread.IsBackground = true;
                    thread.Start(groupNo);
                    MainInfo.GetThreads.Add(thread);
                    MainInfo.agvThState = true;
                }
            }
            if (openStatic)//判断串口是否打开成功
            {
                Btnswitch.IsEnabled = true;//设置关闭串口按钮可用
                Open.IsEnabled = false; //设置打开串口按钮不可用
                Maplistq.IsEnabled = false;//设置地图Combox不可用
                SwitchText.Content = "串口状态：开";
                SwitchImg.Source = new BitmapImage(new Uri(@"Images/电子元器件绿.png", UriKind.Relative));

                Thread thread = new Thread(AgvMessg);
                thread.IsBackground = true;
                thread.Start();
                MainInfo.AgvStaticMessg = true;
                Menu.IsEnabled = false; //导航菜单不可用
                MainInfo.GetThreads.Add(thread);
                MessageBox.Show("打开串口成功！");
            }
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnswitch_Click(object sender, RoutedEventArgs e)
        {
            int comCount = PortInfo.AGVCom.Count;
            bool closePort = true;
            for (int i = 0; i < comCount; i++)
            {
                int groupNo = i + 1;
                if (MainInfo.listAgvDll[i].closePort(groupNo) == 1)
                {
                    MainInfo.listPtr[i] = new IntPtr(0);
                }
                else
                {
                    MessageBox.Show("关闭串口COM" + PortInfo.agv[i].ToString() + "失败！");
                }
            }
            if (closePort)
            {
                PortMessg(); //更新串口状态
                MainInfo.agvThState = false;
                MainInfo.AgvStaticMessg = false;
                Maplistq.IsEnabled = true;//设置地图Combox还原
                Open.IsEnabled = true; //设置打开串口按钮还原
                Btnswitch.IsEnabled = false; //关闭串口按钮不可用
                Menu.IsEnabled = true; //导航菜单还原

                AGVClear(); //清空数据
                DataGridCrear();
                AgvCror();
                SwitchText.Content = "串口状态：关";
                SwitchImg.Source = new BitmapImage(new Uri(@"Images/电子元器件红.png", UriKind.Relative));
                MessageBox.Show("关闭串口成功！");
            }
        }

        public void AGVClear()
        {
            foreach (Thread item in MainInfo.GetThreads)
            {
                item.Abort();
            }
            MainInfo.GetThreads.Clear();

            MainInfo.listAgvDll.Clear(); //清空AGV管理对象
            MainInfo.listPtr.Clear();  //清空串口返回数据
            MainInfo.carStatusList.Clear(); //清空所有AGV状态对象
            MainInfo.agvNo.Clear();  //清空所有AGV
        }

        /// <summary>
        /// 清空所有串口数据
        /// </summary>
        public void Empty()
        {
            PortInfo.buttonCom.Clear();
            PortInfo.buttonBaud.Clear();
            PortInfo.buttonStr.Clear();

            PortInfo.chargeCom.Clear();
            PortInfo.chargeBaud.Clear();
            PortInfo.chargeStr.Clear();

            PortInfo.AGVCom.Clear();
            PortInfo.Baud.Clear();
            PortInfo.agv.Clear();
        }

        /// <summary>
        /// 判断AGV状态是否存在
        /// </summary>
        /// <param name="AgvNum"></param>
        /// <returns></returns>
        public CarStatus AgcCarExists(int AgvNum)
        {
            foreach (int item in MainInfo.carStatusList.Keys.ToArray())
            {
                if (AgvNum.Equals(item))
                {
                    return MainInfo.carStatusList[item];
                }
            }
            return null;
        }

        /// <summary>
        /// AGV信息选择查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabAgvMoveData_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                if (TabAgvMoveData.SelectedItems.Count > 0)
                {
                    selAgv = ((DataRowView)TabAgvMoveData.SelectedItem)[1].ToString();
                }
            }
        }

        #endregion ==========串口打开关闭=========

        public void LogWrite(string msg)
        {
            this.Dispatcher.Invoke(new Action<string>(s =>
            {
                Log.Text += (s + "\n");
            }), msg);
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            AGVClear();
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 取消窗体关闭
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// 运行设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenMap_Click(object sender, RoutedEventArgs e)
        {
            Operation operation = new Operation();
            operation.ShowDialog();
        }
    }
}