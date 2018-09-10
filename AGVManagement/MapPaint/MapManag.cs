using AGV.BLL;
using AGVManagement.Enumeration;
using AGVManagement.instrument;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Threading;

namespace AGVManagement.MapPaint
{
    /// <summary>
    /// 地图加载
    /// </summary>
    public class MapManag
    {
        private List<WirePoint> Pairsarray = new List<WirePoint>();
        private CircuitType GetCircuitType;
        TagInfoBLL tagInfo = new TagInfoBLL();
        LineInfoBLL infoBLL = new LineInfoBLL();
        Painting painting = new Painting();
        widgetInfoBLL widgetInfos = new widgetInfoBLL();
        AreaCompile area = new AreaCompile();
        public double Sise = 15; //地图放大倍数
        private object _locker = new object();
        bool ther = true, theg = true, thms = true;
        public static List<WirePointArray> MainwirePoint = new List<WirePointArray>();//Main路线集合
        public static Dictionary<int, Label> MainvaluePairs = new Dictionary<int, Label>();//Main信标集合


        /// <summary>
        /// 载入地图数据
        /// </summary>
        /// <param name="Times"></param>
        /// <param name="MapIN"></param>
        public void SelectMap(long Times, Canvas MapIN, bool type)
        {
            if (ther != true || theg != true || thms != true)
            {
                return;
            }
            Thread thread = new Thread(() =>
            {
                lock (_locker)
                {
                    ther = false;
                    DataTable da = tagInfo.RataTable(Times.ToString());
                    foreach (DataRow item in da.Rows)
                    {
                        MapIN.Dispatcher.BeginInvoke(new Action<DataRow>(S =>
                        {
                            Label label = TagCreate(new Point() { X = (Convert.ToDouble(S["X"].ToString()) * Sise) - 19, Y = (Convert.ToDouble(S["Y"].ToString()) * Sise) - 11.5 }, Convert.ToInt32(S["TagName"].ToString()),false);
                            if (type)//编辑地图线路添加单击事件，否则不需要
                            {
                                label.MouseDown += Label_MouseDown;
                            }
                            MapIN.Children.Add(label);
                        }), item);
                    }
                    ther = true;
                }
            });
            thread.IsBackground = true;
            thread.Start();

            Thread th = new Thread(() =>
            {
                lock (_locker)
                {
                    theg = false;
                    DataTable dt = infoBLL.LinelistArrer(Times.ToString());
                    foreach (DataRow data in dt.Rows)
                    {
                        MapIN.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (Convert.ToInt32(data["LineStyel"].ToString()) == 1)
                            {
                                GetCircuitType = (CircuitType.Line);
                            }
                            else if (Convert.ToInt32(data["LineStyel"].ToString()) == 2)
                            {
                                GetCircuitType = (CircuitType.Broken);
                            }
                            else if (Convert.ToInt32(data["LineStyel"].ToString()) == 3)
                            {
                                GetCircuitType = (CircuitType.Semicircle);
                            }
                            Pairsarray.Add(new WirePoint() { TagID = Convert.ToInt32(data["Tag1"].ToString().Substring(2)), SetPoint = new Point() { X = Convert.ToDouble(data["StartX"].ToString()) * Sise, Y = Convert.ToDouble(data["StartY"].ToString()) * Sise } });
                            Pairsarray.Add(new WirePoint() { TagID = Convert.ToInt32(data["Tag2"].ToString().Substring(2)), SetPoint = new Point() { X = Convert.ToDouble(data["EndX"].ToString()) * Sise, Y = Convert.ToDouble(data["EndY"].ToString()) * Sise } });
                            AddLine(MapIN,false);
                            Pairsarray.Clear();
                        }));
                    }
                    theg = true;
                }
            });
            th.IsBackground = true;
            th.Start();

            Thread ts = new Thread(() =>
            {
                lock (_locker)
                {
                    thms = false;
                    DataTable ta = widgetInfos.WidgetLIst(Times.ToString());
                    foreach (DataRow tables in ta.Rows)
                    {
                        MapIN.Dispatcher.BeginInvoke(new Action<DataRow>(table =>
                        {
                            if (table["WidgetNo"].ToString().Substring(0, 2).Equals("AR"))
                            {
                                MapIN.Children.Add(NewArea(new Point() { X = (Convert.ToDouble(table["X"].ToString()) * Sise), Y = (Convert.ToDouble(table["Y"].ToString()) * Sise) }, table["Name"].ToString(), Convert.ToInt32(table["WidgetNo"].ToString().Substring(2)), table["BackColor"].ToString(), table["ForeColor"].ToString(), table["BorderColor"].ToString(), Convert.ToDouble(table["FontSize"].ToString()), Convert.ToDouble(table["Width"].ToString()), Convert.ToDouble(table["Height"].ToString()), table["FontPosition"].ToString()));
                            }
                            else
                            {
                                MapIN.Children.Add(FontTextNew(new Point() { X = (Convert.ToDouble(table["X"].ToString()) * Sise), Y = (Convert.ToDouble(table["Y"].ToString()) * Sise) }, table["Name"].ToString(), Convert.ToInt32(table["WidgetNo"].ToString().Substring(2)), Convert.ToDouble(table["FontSize"].ToString()), table["ForeColor"].ToString()));
                            }
                        }), tables);
                    }
                    thms = true;
                }
            });
            ts.IsBackground = true;
            ts.Start();
        }
        bool MapsTige = true, LineMaps = true;
        /// <summary>
        /// 载入Main地图数据
        /// </summary>
        /// <param name="Times"></param>
        /// <param name="MapIN"></param>
        public void SelectMapLOad(long Times, Canvas MapIN)
        {
            if (MapsTige != true || LineMaps != true)
            {
                return;
            }
            MapInstrument.keyValuePairs.Clear();
            MapInstrument.valuePairs.Clear();
            MapInstrument.wirePointArrays.Clear();
            MapInstrument.GetKeyValues.Clear();
            MainwirePoint.Clear();
            MainvaluePairs.Clear();
            Painting.siseWin = 1;
            Thread th = new Thread(() =>
            {
                lock (_locker)
                {
                    MapsTige = false;
                    DataTable dt = infoBLL.LinelistArrer(Times.ToString());
                    foreach (DataRow data in dt.Rows)
                    {
                        MapIN.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (Convert.ToInt32(data["LineStyel"].ToString()) == 1)
                            {
                                GetCircuitType = (CircuitType.Line);
                            }
                            else if (Convert.ToInt32(data["LineStyel"].ToString()) == 2)
                            {
                                GetCircuitType = (CircuitType.Broken);
                            }
                            else if (Convert.ToInt32(data["LineStyel"].ToString()) == 3)
                            {
                                GetCircuitType = (CircuitType.Semicircle);
                            }
                            Pairsarray.Add(new WirePoint() { TagID = Convert.ToInt32(data["Tag1"].ToString().Substring(2)), SetPoint = new Point() { X = Convert.ToDouble(data["StartX"].ToString()) * Sise, Y = Convert.ToDouble(data["StartY"].ToString()) * Sise } });
                            Pairsarray.Add(new WirePoint() { TagID = Convert.ToInt32(data["Tag2"].ToString().Substring(2)), SetPoint = new Point() { X = Convert.ToDouble(data["EndX"].ToString()) * Sise, Y = Convert.ToDouble(data["EndY"].ToString()) * Sise } });
                            AddLine(MapIN,true);
                            Pairsarray.Clear();
                        }));
                    }
                    MapsTige = true;
                }
            });
            th.IsBackground = true;
            th.Start();


            Thread thread = new Thread(() =>
            {
                lock (_locker)
                {
                    LineMaps = false;
                    DataTable da = tagInfo.RataTable(Times.ToString());
                    foreach (DataRow item in da.Rows)
                    {
                        MapIN.Dispatcher.BeginInvoke(new Action<DataRow>(S =>
                        {
                            TagCreate(new Point() { X = (Convert.ToDouble(S["X"].ToString()) * Sise) - 19, Y = (Convert.ToDouble(S["Y"].ToString()) * Sise) - 11.5 }, Convert.ToInt32(S["TagName"].ToString()),true);
                        }), item);
                    }
                    LineMaps = true; ;
                }
            });
            thread.IsBackground = true;
            thread.Start();

        }

        /// <summary>
        /// 生成区域
        /// </summary>
        /// <param name="point"></param>
        /// <param name="Text"></param>
        /// <param name="ArID"></param>
        /// <param name="bgColor"></param>
        /// <param name="FontColor"></param>
        /// <param name="BrColor"></param>
        /// <param name="FontSise"></param>
        /// <param name="MpWidth"></param>
        /// <param name="MpHeight"></param>
        /// <param name="FontPosition"></param>
        /// <returns></returns>
        public Label NewArea(Point point, string Text, int ArID, string bgColor, string FontColor, string BrColor, double FontSise, double MpWidth, double MpHeight, string FontPosition)
        {
            Label labelArea = new Label()
            {
                Content = Text,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + bgColor + "")),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + FontColor + "")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + BrColor + "")),
                BorderThickness = new Thickness(2, 2, 2, 2),
                FontSize = (FontSise / 10) * Sise,
                Width = MpWidth * Sise,
                Height = MpHeight * Sise,
                Margin = new Thickness(point.X, point.Y, 0, 0),
                Cursor = Cursors.Hand,
                Tag = ArID,
            };
            area.aAlignment(FontPosition, labelArea);
            MapInstrument.keyValuePairs.Add(ArID, labelArea);
            return labelArea;
        }

        /// <summary>
        /// 生成文字
        /// </summary>
        /// <param name="point"></param>
        /// <param name="Text"></param>
        /// <param name="TextInx"></param>
        /// <param name="FontSise"></param>
        /// <param name="fontColor"></param>
        /// <returns></returns>
        public Label FontTextNew(Point point, string Text, int TextInx, double FontSise, string fontColor)
        {
            Label labelText = new Label()
            {
                Content = Text,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + fontColor + "")),
                FontSize = FontSise * 2,
                Margin = new Thickness(point.X, point.Y, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Cursor = Cursors.Hand,
                Tag = TextInx,
            };
            MapInstrument.GetKeyValues.Add(TextInx, labelText);
            return labelText;
        }

        /// <summary>
        /// 生成线路
        /// </summary>
        /// <param name="MapIN"></param>
        public void AddLine(Canvas MapIN,bool type)
        {
            if (GetCircuitType.Equals(CircuitType.Line))//绘制直线
            {
                Path path = painting.DrawingLine(Pairsarray[0].SetPoint, Pairsarray[1].SetPoint, MapIN);//绘制直线
                MapInstrument.wirePointArrays.Add(new WirePointArray() { circuitType = CircuitType.Line, GetPath = path, GetPoint = Pairsarray[0], GetWirePoint = Pairsarray[1] });
                if (type)
                {
                    MainwirePoint.Add(new WirePointArray() { circuitType = CircuitType.Line, GetPath = path, GetPoint = Pairsarray[0], GetWirePoint = Pairsarray[1] });
                }
            }
            else if (GetCircuitType.Equals(CircuitType.Semicircle))
            {
                Path path = painting.DrawingSemicircle(Pairsarray[0].SetPoint, Pairsarray[1].SetPoint, MapIN);//绘制半圆
                MapInstrument.wirePointArrays.Add(new WirePointArray() { circuitType = CircuitType.Semicircle, GetPath = path, GetPoint = Pairsarray[0], GetWirePoint = Pairsarray[1] });
                if (type)
                {
                    MainwirePoint.Add(new WirePointArray() { circuitType = CircuitType.Semicircle, GetPath = path, GetPoint = Pairsarray[0], GetWirePoint = Pairsarray[1] });
                }
            }
            else if (GetCircuitType.Equals(CircuitType.Broken))//折线
            {
                List<Path> Pathr = painting.DrawingBroken(Pairsarray[0].SetPoint, Pairsarray[1].SetPoint, MapIN);
                MapInstrument.wirePointArrays.Add(new WirePointArray() { circuitType = CircuitType.Broken, Paths = Pathr, GetPoint = Pairsarray[0], GetWirePoint = Pairsarray[1] });
                if (type)
                {
                    MainwirePoint.Add(new WirePointArray() { circuitType = CircuitType.Broken, Paths = Pathr, GetPoint = Pairsarray[0], GetWirePoint = Pairsarray[1] });
                }
            }
        }

        /// <summary>
        /// 生成Tag
        /// </summary>
        /// <param name="point"></param>
        /// <param name="TagID"></param>
        /// <returns></returns>
        private Label TagCreate(Point point, int TagID,bool type)
        {
            Label labelStrn = new Label()
            {
                Content = TagID.ToString(),
                Background = new SolidColorBrush(Color.FromRgb(80, 150, 255)),
                Foreground = new SolidColorBrush(Colors.WhiteSmoke),
                Width = 38,
                Height = 23,
                Margin = new Thickness(point.X, point.Y, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Cursor = Cursors.Hand,
                Tag = TagID
            };
            Canvas.SetZIndex(labelStrn, 999999);
            MapInstrument.valuePairs.Add(TagID, labelStrn);
            if (type)
            {
                MainvaluePairs.Add(TagID, labelStrn);
            }
            return labelStrn;
        }


        public long Times;
        public DataGrid GetData;
        public DataTable table;
        public LineMap lineMap = new LineMap();
        public bool tagType = false;

        /// <summary>
        /// 清空Tag关联点
        /// </summary>
        public void TagCic()
        {
            lineMap.GetTags = null;
        }

        /// <summary>
        /// Tag单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (tagType)
            {
                Label tmp = (Label)sender;
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    lineMap.TagClick(Times, Convert.ToInt32(tmp.Tag), GetData, table, true);
                }
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    int s = 0;
                    List<object> list = null;
                    bool type = false;
                    int TagUnm = 0;
                    int Index = 0;
                    for (int i = 0; i < GetData.Items.Count; i++)
                    {
                        if (Convert.ToInt32(((DataRowView)GetData.Items[i])[0].ToString().Trim()).Equals(Convert.ToInt32(tmp.Tag)))
                        {
                            if (i.Equals(0))
                            {
                                list = ((DataRowView)GetData.Items[i]).Row.ItemArray.ToList();
                                TagUnm = Convert.ToInt32(tmp.Tag);
                                Index = i;
                                type = true;
                            }
                            else
                            {
                                list = ((DataRowView)GetData.Items[i]).Row.ItemArray.ToList();
                                TagUnm = Convert.ToInt32(((DataRowView)GetData.Items[i - 1])["Tag"].ToString());
                                Index = i;
                            }
                            s++;
                        }
                    }
                    if (s.Equals(1))//（判断Tag是否在表格中存在）存在重复Tag只可在表格修改
                    {
                        TagLine tag = new TagLine(list, Times, type, TagUnm, GetData, Index, table, lineMap);
                        tag.Show();
                    }
                }
            }
        }

        public void LineMapShow(List<object> list, bool type, int TagUnm, int Index)
        {
            TagLine tag = new TagLine(list, Times, type, TagUnm, GetData, Index, table, lineMap);
            tag.Show();
        }



    }
}
