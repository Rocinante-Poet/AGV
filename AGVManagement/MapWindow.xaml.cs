using AGVManagement.MapPaint;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using AGV.BLL;
using System.Data;
using AGVManagement.instrument;
using MySql.Data.MySqlClient;

namespace AGVManagement
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private Point point;//记录滚动条位置动态调整生成控件位置
        Painting painting = new Painting();
        MapInstrument instrument = new MapInstrument();
        MapMessageBLL mapMessage = new MapMessageBLL();
        double GrnWidth, GrnMpHeight;//默认宽高
        private long Time;
        public MainWindow(long Times, string MapNa, string MapNs, double Width, double HeMap)
        {
            InitializeComponent();
            mainPanel.Children.Clear();
            Painting.siseWin = 1;
            MapInstrument.keyValuePairs.Clear();
            MapInstrument.valuePairs.Clear();
            MapInstrument.wirePointArrays.Clear();
            MapInstrument.GetKeyValues.Clear();
            Time = Times;
            LoadMs(Times, MapNa, MapNs, Width, HeMap);
        }

        private void LoadMs(long Times, string MapNa, string MapNs, double Width, double HeMap)
        {
            if (!Times.Equals(0))
            {
                MapInfo(Times, MapNa);
            }
            else
            {
                CanvasMp(Width, HeMap, MapNs, Width, HeMap);
            }
        }



        private void MapInfo(long Times, string MapN)
        {
            DataTable da = mapMessage.MapParray(MapN);
            if (da != null)
            {
                foreach (DataRow data in da.Rows)
                {
                    double Width = Convert.ToDouble(data["Width"].ToString());
                    double Height = Convert.ToDouble(data["Height"].ToString());
                    GrnWidth = Width * 10;
                    GrnMpHeight = Height * 10;

                    int MPType = Convert.ToInt32(data["Type"].ToString());
                    if (MPType.Equals(0))
                    {
                        TypeMp.SelectedIndex = 1;
                    }
                    else if (MPType.Equals(1))
                    {
                        TypeMp.SelectedIndex = 0;
                    }
                    else
                    {
                        TypeMp.SelectedIndex = 2;
                    }
                    CanvasMp(Width, Height, data["Name"].ToString(), Width, Height);
                    instrument.LoadDataInfo(mainPanel, Times);
                }
            }
            else
            {
                MessageBox.Show("地图丢失");

            }
        }
        private void CanvasMp(double Width, double Height,string TX,double WP,double Wh)
        {
            MP.Text = TX;
            MpWidth.Content = WP + "米";
            MpHeight.Content = Wh + "米";
            GrnWidth = Width * 10;
            GrnMpHeight = Height * 10; 
            mainPanel.Width = Width * 10;
            mainPanel.Height = Height * 10;
            TopX.Width = Width * 10;
            TopY.Height = Height * 10;
            painting.Coordinate(mainPanel);
            painting.CoordinateX(TopX, TopY);
        }
        SaveMap map = new SaveMap();

        /// <summary>
        /// 保存地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            int TypeID =0;
            if (TypeMp.SelectedIndex.Equals(0))
            {
                TypeID = 1;
            }
            else if (TypeMp.SelectedIndex.Equals(1))
            {
                TypeID = 0;

            }
            else
            {
                TypeID = 2;
            }
            bool mp = map.SaveAtlas((!Time.Equals(0) ? Time.ToString() : UTC.ConvertDateTimeLong(DateTime.Now).ToString()), !Time.Equals(0) ? false : true, MP.Text, (GrnWidth / 10), (GrnMpHeight / 10),"0", TypeID);
            if (mp)
            {
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("保存失败");
            }
        }

        /// <summary>
        /// 导出地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Export_Click(object sender, RoutedEventArgs e)
        {

        }


        /// <summary>
        /// 添加信标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tags_Click(object sender, RoutedEventArgs e)
        {
            BgColors(Tags);
            instrument.TagNew(mainPanel, point);
        }


        /// <summary>
        /// 滚动条滚动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SrcCount_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            point.X = e.HorizontalOffset;
            point.Y = e.VerticalOffset;
            SrcX.ScrollToHorizontalOffset(e.HorizontalOffset);//X轴标尺跟随移动
            SrcY.ScrollToVerticalOffset(e.VerticalOffset); //Y轴标尺等随移动
        }

        /// <summary>
        /// 比例尺滑动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SliMax_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            painting.Mapmagnify(Convert.ToInt32(e.NewValue), TopX, TopY, mainPanel, GrnWidth, GrnMpHeight);//地图比例尺缩放
        }

        /// <summary>
        /// 添加区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Area_Click(object sender, RoutedEventArgs e)
        {
            BgColors(Area);
            instrument.MapAreaNew(mainPanel, point);
        }

        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertImg_Click(object sender, RoutedEventArgs e)
        {
            BgColors(InsertImg);
        }

        /// <summary>
        /// 绘制直线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Straight_Click(object sender, RoutedEventArgs e)
        {
            BgColors(Straight);
            instrument.Mapstraight();
        }

        /// <summary>
        /// 鼠标点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_mouse_Click(object sender, RoutedEventArgs e)
        {
            BgColors(btn_mouse);
            instrument.mouseStatic();
        }

        private void BgColors(Button button)
        {
            Bgbtn(DeleteCircuit);
            Bgbtn(Btn_cren);
            Bgbtn(btn_mouse);
            Bgbtn(Straight);
            Bgbtn(InsertImg);
            Bgbtn(Area);
            Bgbtn(Tags);
            Bgbtn(Btn_Text);
            Bgbtn(Broken);
            button.Background = new SolidColorBrush(Color.FromRgb(249, 92, 38));
            button.Foreground = new SolidColorBrush(Colors.White);
        }
        private void Bgbtn(Button button)
        {
            button.Background = new SolidColorBrush(Colors.White);
            button.Foreground = new SolidColorBrush(Colors.Black);
        }

        /// <summary>
        /// 折线线路
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Broken_Click(object sender, RoutedEventArgs e)
        {
            instrument.Brokene();
            BgColors(Broken);
        }

        /// <summary>
        /// 半圆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_cren_Click(object sender, RoutedEventArgs e)
        {
            instrument.Semicircles();
            BgColors(Btn_cren);
        }

        /// <summary>
        /// 清除线路
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteCircuit_Click(object sender, RoutedEventArgs e)
        {
            instrument.ClearTen();
            BgColors(DeleteCircuit);
        }

        /// <summary>
        /// 文字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Text_Click(object sender, RoutedEventArgs e)
        {
            BgColors(Btn_Text);
            instrument.TextNew(mainPanel, point);
        }
    }
}
