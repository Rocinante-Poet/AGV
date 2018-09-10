using AGV.BLL;
using AGVManagement.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Data;
using MySql.Data.MySqlClient;

namespace AGVManagement.MapPaint
{
    /// <summary>
    /// 工具栏控件管理
    /// </summary>
    public class MapInstrument
    {
        private int s = 1;//信标起始索引
        private int index = 1;//区域起始索引
        private int TextInx = 1;//文字索引
        private Point pos;//记录移动时Tag位置
        private Point jos;//记录移动时工作区位置
        private bool PathStatic = false;//绘制状态（true:绘制状态 false 否可拖动）
        private CircuitType GetCircuitType;//绘制线路类型 （直线，折线，曲线）
        private List<WirePoint> Pairsarray = new List<WirePoint>();//（暂时存放绘制线路两点位置）
        public static List<WirePointArray> wirePointArrays = new List<WirePointArray>();//路线集合
        public static Dictionary<int, Label> keyValuePairs = new Dictionary<int, Label>();//区域集合
        public static Dictionary<int, Label> valuePairs = new Dictionary<int, Label>();//信标集合
        public static Dictionary<int, Label> GetKeyValues = new Dictionary<int, Label>();//文字集合
        public Dictionary<int, WirePointArray> wires = new Dictionary<int, WirePointArray>();//暂时存放拖动时所用关联线路（线路起始点和结束点存在共用情况需做判断）
        private Canvas GetCanvas; //绘制容器
        Painting painting = new Painting();//地图绘制

        #region =========动态生成信标===========

        /// <summary>
        /// 生成信标
        /// </summary>
        /// <param name="mainPanel"></param>
        public void TagNew(Canvas mainPanel, Point point)
        {

            GetCanvas = mainPanel;
            Label labelStrn = TagCreate(point, s, false);

            #region 所有Tag改为原色
            PathStatic = false;
            TagFormer();
            #endregion
            valuePairs.Add(s, labelStrn);
            mainPanel.Children.Add(labelStrn);
            s++;
        }

        public Label TagCreate(Point point, int TagID, bool type)
        {
            Label labelStrn = new Label()
            {
                Content = TagID.ToString(),
                Background = new SolidColorBrush(Color.FromRgb(80, 150, 255)),
                Foreground = new SolidColorBrush(Colors.WhiteSmoke),
                Width = 38,
                Height = 23,
                Margin = new Thickness(point.X + (type.Equals(false) ? 120 : 0), point.Y + (type.Equals(false) ? 120 : 0), 0, 0),//120(位置偏移量)
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Cursor = Cursors.Hand,
                Tag = TagID
            };
            Canvas.SetZIndex(labelStrn, 999999);
            labelStrn.MouseDown += LabelStrn_MouseDown;
            labelStrn.MouseMove += LabelStrn_MouseMove;
            labelStrn.MouseUp += LabelStrn_MouseUp;
            return labelStrn;
        }

        /// <summary>
        /// 释放鼠标发生（信标）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelStrn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Label tmp = (Label)sender;
            tmp.ReleaseMouseCapture();
            Point point = new Point { X = tmp.Margin.Left + 19, Y = tmp.Margin.Top + 11.5 }; //19X轴偏移位置,11.5Y轴偏移位置（信标中心点）
            if (PathStatic)
            {
                AddCircuit(Convert.ToInt32(tmp.Tag), point);
            }
            else
            {
                MouseMove(point, Convert.ToInt32(tmp.Tag));//信标移动
            }
        }


        #region ==========添加线路==========
        public void AddCircuit(int tmp, Point point)
        {
            #region 判断位置是否存在
            bool TagExistx = false; //判断位置是否存在
            foreach (WirePoint item in Pairsarray)
            {
                if (tmp.Equals(item.TagID))//如果TagID相同则存在
                    TagExistx = true;
            }
            #endregion

            #region 所有Tag改为原色
            foreach (int item in valuePairs.Keys)
            {
                valuePairs[item].Background = new SolidColorBrush(Color.FromRgb(80, 150, 255));
            }
            #endregion

            if (!TagExistx) //位置不存在则添加，防止重复添加
            {
                Pairsarray.Add(new WirePoint { TagID = tmp, SetPoint = point });
            }

            #region 线路绘制点改为红色
            foreach (int item in valuePairs.Keys)
            {
                foreach (WirePoint itms in Pairsarray)
                {
                    if (itms.TagID.Equals(item))
                    {
                        valuePairs[item].Background = new SolidColorBrush(Colors.Red);
                    }
                }
            }
            #endregion

            if (Pairsarray.Count.Equals(2))//起始位置存在则开始绘制
            {
                if (!GetCircuitType.Equals(CircuitType.Clear))
                {
                    #region 判断线路是否存在
                    bool lineExistx = false; //判断线路是否存在
                    double tr = Pairsarray[0].SetPoint.X - Pairsarray[1].SetPoint.X; //大于0下方上方小于0上方
                    foreach (WirePointArray item in wirePointArrays)
                    {
                        if (GetCircuitType.Equals(CircuitType.Semicircle))
                        {
                            //如果（线路起始位置和结束位置一一对应）则视为线路存在
                            if (((item.GetPoint.SetPoint.Equals(Pairsarray[0].SetPoint) && item.GetWirePoint.SetPoint.Equals(Pairsarray[1].SetPoint)) || (item.GetPoint.SetPoint.Equals(Pairsarray[1].SetPoint) && item.GetWirePoint.SetPoint.Equals(Pairsarray[0].SetPoint))) && item.circuitType.Equals(GetCircuitType) && item.Direction.Equals(tr > 0 ? 1 : 0))
                                lineExistx = true;
                        }
                        else if (GetCircuitType.Equals(CircuitType.Broken))
                        {

                            double diff = Pairsarray[0].SetPoint.Y - Pairsarray[1].SetPoint.Y;
                            //如果（线路起始位置和结束位置一一对应）则视为线路存在
                            if (((item.GetPoint.SetPoint.Equals(Pairsarray[0].SetPoint) && item.GetWirePoint.SetPoint.Equals(Pairsarray[1].SetPoint)) || (item.GetPoint.SetPoint.Equals(Pairsarray[1].SetPoint) && item.GetWirePoint.SetPoint.Equals(Pairsarray[0].SetPoint))) && item.circuitType.Equals(GetCircuitType) && item.Direction.Equals(diff < 0 ? 0 : 1))
                                lineExistx = true;
                        }
                        else
                        {
                            //如果（线路起始位置和结束位置一一对应）或（线路起始位置和结束位置相同，结束位置和起始位置相同）则视为线路存在
                            if (((item.GetPoint.SetPoint.Equals(Pairsarray[0].SetPoint) && item.GetWirePoint.SetPoint.Equals(Pairsarray[1].SetPoint)) || (item.GetPoint.SetPoint.Equals(Pairsarray[1].SetPoint) && item.GetWirePoint.SetPoint.Equals(Pairsarray[0].SetPoint))) && item.circuitType.Equals(GetCircuitType))
                                lineExistx = true;
                        }
                    }
                    #endregion

                    if (!lineExistx)//线路不存在则绘制线路
                    {
                        AddLine();
                        foreach (WirePoint item in Pairsarray)//重新添加Tag 
                        {
                            GetCanvas.Children.Add(valuePairs[item.TagID]);
                        }

                    }
                    Pairsarray.Clear();//移除暂存起始结束点
                }
                else
                {
                    ClearCircuit();//清除路线
                }
            }
        }
        #endregion


        public void AddLine()
        {
            foreach (WirePoint item in Pairsarray)//移除Tag （否则会出现线路遮挡Tag问题）
            {
                GetCanvas.Children.Remove(valuePairs[item.TagID]);
            }
            if (GetCircuitType.Equals(CircuitType.Line))//绘制直线
            {
                Path path = painting.DrawingLine(Pairsarray[0].SetPoint, Pairsarray[1].SetPoint, GetCanvas);//绘制直线
                wirePointArrays.Add(new WirePointArray { GetPoint = Pairsarray[0], GetWirePoint = Pairsarray[1], GetPath = path, circuitType = CircuitType.Line });//将绘制的线路添加到线路集合中
            }
            else if (GetCircuitType.Equals(CircuitType.Semicircle))
            {
                double ey = Pairsarray[0].SetPoint.X - Pairsarray[1].SetPoint.X; //大于0下方上方小于0上方
                Path path = painting.DrawingSemicircle(Pairsarray[0].SetPoint, Pairsarray[1].SetPoint, GetCanvas);//绘制半圆
                if (ey > 0)
                {
                    wirePointArrays.Add(new WirePointArray { GetPoint = Pairsarray[0], GetWirePoint = Pairsarray[1], GetPath = path, circuitType = CircuitType.Semicircle, Direction = 1 });//将绘制的线路添加到线路集合中
                }
                else
                {
                    wirePointArrays.Add(new WirePointArray { GetPoint = Pairsarray[0], GetWirePoint = Pairsarray[1], GetPath = path, circuitType = CircuitType.Semicircle, Direction = 0 });//将绘制的线路添加到线路集合中
                }
            }
            else if (GetCircuitType.Equals(CircuitType.Broken))//折线
            {
                List<Path> Pathr = painting.DrawingBroken(Pairsarray[0].SetPoint, Pairsarray[1].SetPoint, GetCanvas);
                double drn = Pairsarray[0].SetPoint.X - Pairsarray[1].SetPoint.X;
                double hrn = Pairsarray[0].SetPoint.Y - Pairsarray[1].SetPoint.Y;

                if ((drn > 0 && hrn < 0) || (drn < 0 && hrn > 0))
                {
                    double diff = Pairsarray[0].SetPoint.Y - Pairsarray[1].SetPoint.Y;
                    if (diff < 0)
                    {
                        wirePointArrays.Add(new WirePointArray { GetPoint = Pairsarray[0], GetWirePoint = Pairsarray[1], Paths = Pathr, circuitType = CircuitType.Broken, Direction = 0 });//将绘制的线路添加到线路集合中
                    }
                    else
                    {
                        wirePointArrays.Add(new WirePointArray { GetPoint = Pairsarray[0], GetWirePoint = Pairsarray[1], Paths = Pathr, circuitType = CircuitType.Broken, Direction = 1 });//将绘制的线路添加到线路集合中
                    }
                }
                else
                {
                    double diff = Pairsarray[0].SetPoint.Y - Pairsarray[1].SetPoint.Y;
                    if (diff < 0)
                    {
                        wirePointArrays.Add(new WirePointArray { GetPoint = Pairsarray[0], GetWirePoint = Pairsarray[1], Paths = Pathr, circuitType = CircuitType.Broken, Direction = 0 });//将绘制的线路添加到线路集合中
                    }
                    else
                    {
                        wirePointArrays.Add(new WirePointArray { GetPoint = Pairsarray[0], GetWirePoint = Pairsarray[1], Paths = Pathr, circuitType = CircuitType.Broken, Direction = 1 });//将绘制的线路添加到线路集合中
                    }
                }
            }
        }


        /// <summary>
        /// 信标移动
        /// </summary>
        /// <param name="point"></param>
        /// <param name="TagID"></param>
        public void MouseMove(Point point, int TagID)
        {
            #region 线路移动
            wires.Clear();//清空集合
            for (int i = 0; i < wirePointArrays.Count; i++)//查找拖动时所有关联线路
            {
                if (wirePointArrays[i].GetPoint.TagID.Equals(TagID))//如果起始位置相同则线路和拖动Tag存在关联
                {
                    wirePointArrays[i].GetPoint.SetPoint = point; //更新线路起始点位置
                    wires.Add(i, wirePointArrays[i]);//暂时存放线路
                    GetCanvas.Children.Remove(valuePairs[wirePointArrays[i].GetPoint.TagID]); //移除关联线路起始位置Tag
                    GetCanvas.Children.Remove(valuePairs[wirePointArrays[i].GetWirePoint.TagID]);//移除关联线路结束位置Tag
                }
                else if (wirePointArrays[i].GetWirePoint.TagID.Equals(TagID))// 如果结束位置相同则线路和拖动Tag存在关联
                {
                    GetCanvas.Children.Remove(valuePairs[wirePointArrays[i].GetPoint.TagID]);//移除关联线路起始位置Tag
                    GetCanvas.Children.Remove(valuePairs[wirePointArrays[i].GetWirePoint.TagID]);//移除关联线路结束位置Tag
                    wirePointArrays[i].GetWirePoint.SetPoint = point; //更新线路结束点位置
                    wires.Add(i, wirePointArrays[i]);//暂时存放线路
                }
            }
            List<Label> labels = new List<Label>(); //存放拖动时所有关联Tag
            foreach (int item in wires.Keys)//重新绘制所有关联线路
            {
                Path path = null;
                List<Path> Genpaths = null;
                if (wires[item].circuitType.Equals(CircuitType.Line))//直线
                {
                    path = painting.DrawingLine(wires[item].GetPoint.SetPoint, wires[item].GetWirePoint.SetPoint, GetCanvas);
                }
                else if (wires[item].circuitType.Equals(CircuitType.Semicircle))//半圆
                {
                    path = painting.DrawingSemicircle(wires[item].GetPoint.SetPoint, wires[item].GetWirePoint.SetPoint, GetCanvas);
                }
                else if (wires[item].circuitType.Equals(CircuitType.Broken))//折线
                {
                    Genpaths = painting.DrawingBroken(wires[item].GetPoint.SetPoint, wires[item].GetWirePoint.SetPoint, GetCanvas);
                }
                wirePointArrays[item].GetPath = path;//更新线路对象
                wirePointArrays[item].Paths = Genpaths;
            }
            foreach (int item in wires.Keys)//因为线路关联Tag存在重复，为防止重复添加需做判断（及线路起始点和结束点Tag存在共用情况）
            {
                bool lableExistx = true;//判断起始点Tag是否存在
                foreach (Label irn in labels)
                {
                    if (wires[item].GetPoint.TagID == Convert.ToInt32(irn.Tag))//ID相同则存在
                    {
                        lableExistx = false;
                    }
                }
                if (lableExistx)//不存在则添加起始点Tag
                {
                    GetCanvas.Children.Add(valuePairs[wires[item].GetPoint.TagID]);//绘制起始点Tag
                    labels.Add(valuePairs[wires[item].GetPoint.TagID]);//将起始点Tag添加至集合
                }
                bool lableExi = true;//判断结束点Tag是否存在
                foreach (Label irn in labels)
                {
                    if (wires[item].GetWirePoint.TagID == Convert.ToInt32(irn.Tag))//ID相同则存在
                    {
                        lableExi = false;
                    }
                }
                if (lableExi)//不存在则添加结束点Tag
                {
                    GetCanvas.Children.Add(valuePairs[wires[item].GetWirePoint.TagID]);//绘制结束点Tag
                    labels.Add(valuePairs[wires[item].GetWirePoint.TagID]);//将结束点Tag添加至集合
                }
            }
            #endregion
        }

        /// <summary>
        /// 移动鼠标发生（信标）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelStrn_MouseMove(object sender, MouseEventArgs e)
        {
            if (PathStatic)
                return;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Label tmp = (Label)sender;
                double dx = e.GetPosition(null).X - pos.X + tmp.Margin.Left;
                double dy = e.GetPosition(null).Y - pos.Y + tmp.Margin.Top;
                if (dx > 0 && dy > 0)
                {
                    tmp.Margin = new Thickness(dx, dy, 0, 0);
                }
                pos = e.GetPosition(null);
                foreach (WirePointArray item in wirePointArrays)//查询所有关联线路
                {
                    if (item.GetPoint.TagID.Equals(Convert.ToInt32(tmp.Tag)) || item.GetWirePoint.TagID.Equals(Convert.ToInt32(tmp.Tag)))//暂时移除拖动时关联线路
                    {
                        GetCanvas.Children.Remove(item.GetPath);
                        if (item.Paths != null)
                        {
                            foreach (Path it in item.Paths)
                            {
                                GetCanvas.Children.Remove(it);
                            }
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 按下鼠标发生（信标）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelStrn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label tmp = (Label)sender;
            pos = e.GetPosition(null);
            tmp.CaptureMouse();
            tmp.Cursor = Cursors.Hand;
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (PathStatic)
                    return;
                TagRedact mapRedact = new TagRedact(Convert.ToInt32(tmp.Tag), GetCanvas);
                mapRedact.GetMovement += MouseMove;
                mapRedact.ShowDialog();
            }
        }
        #endregion

        #region ==========动态生成工作区========

        /// <summary>
        /// 动态生成工作区
        /// </summary>
        /// <param name="mainPanel"></param>
        /// <param name="point"></param>
        public void MapAreaNew(Canvas mainPanel, Point point)
        {
            GetCanvas = mainPanel;
            Label labelArea = NewArea(point, null, index, "000000", "FFFFFF", "000000", 21, 100, 100, false, "居中对齐");
            mainPanel.Children.Add(labelArea);
            keyValuePairs.Add(index, labelArea);
            index++;
        }
        AreaCompile area = new AreaCompile();
        public Label NewArea(Point point, string Text, int ArID, string bgColor, string FontColor, string BrColor, double FontSise, double MpWidth, double MpHeight, bool type, string FontPosition)
        {
            Label labelArea = new Label()
            {
                Content = type.Equals(false) ? "工作区" + index.ToString() : Text,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + bgColor + "")),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + FontColor + "")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + BrColor + "")),
                BorderThickness = new Thickness(2, 2, 2, 2),
                FontSize = FontSise * Painting.siseWin,
                Width = MpWidth * Painting.siseWin,
                Height = MpHeight * Painting.siseWin,
                Margin = new Thickness(point.X + (type.Equals(false) ? 120 : 0), point.Y + (type.Equals(false) ? 120 : 0), 0, 0),//120(位置偏移量)
                Cursor = Cursors.Hand,
                Tag = ArID,
            };
            area.aAlignment(FontPosition, labelArea);
            labelArea.MouseDown += LabelArea_MouseDown;
            labelArea.MouseMove += LabelArea_MouseMove;
            labelArea.MouseUp += LabelArea_MouseUp;
            return labelArea;
        }


        /// <summary>
        /// 释放鼠标发送（工作区）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelArea_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Label tmp = (Label)sender;
            tmp.ReleaseMouseCapture();
        }

        /// <summary>
        /// 移动鼠标发生（工作区）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Label tmp = (Label)sender;
                double dx = e.GetPosition(null).X - jos.X + tmp.Margin.Left;
                double dy = e.GetPosition(null).Y - jos.Y + tmp.Margin.Top;
                if (dx > 0 && dy > 0)
                {
                    tmp.Margin = new Thickness(dx, dy, 0, 0);
                }
                jos = e.GetPosition(null);
            }
        }

        /// <summary>
        /// 按下鼠标发生（工作区）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label tmp = (Label)sender;
            jos = e.GetPosition(null);
            tmp.CaptureMouse();
            tmp.Cursor = Cursors.Hand;
            if (e.RightButton == MouseButtonState.Pressed)
            {
                MapRedact mapRedact = new MapRedact(Convert.ToInt32(tmp.Tag), GetCanvas);
                mapRedact.ShowDialog();
            }
        }

        #endregion

        #region ========所有Tag改为原色=========
        public void TagFormer()
        {
            #region 所有Tag改为原色
            Pairsarray.Clear();//移除暂存起始点
            foreach (int item in valuePairs.Keys)
            {
                valuePairs[item].Background = new SolidColorBrush(Color.FromRgb(80, 150, 255));
            }
            #endregion
        }
        #endregion

        #region ==========动态生成文字==========

        /// <summary>
        /// 生成文字
        /// </summary>
        public void TextNew(Canvas mainPanel, Point point)
        {
            Label labelText = FontNew(point, null, TextInx, 35, "000000", false);
            GetKeyValues.Add(TextInx, labelText);
            mainPanel.Children.Add(labelText);
            GetCanvas = mainPanel;
            TextInx++;
        }


        public Label FontNew(Point point, string Text, int TextInx, double FontSise, string fontColor, bool type)
        {
            Label labelText = new Label()
            {
                Content = type.Equals(false) ? "文字" + TextInx.ToString() : Text,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + fontColor + "")),
                FontSize = FontSise * Painting.siseWin,
                Margin = new Thickness(point.X + (type.Equals(false) ? 120 : 0), point.Y + (type.Equals(false) ? 120 : 0), 0, 0),//120(位置偏移量)
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Cursor = Cursors.Hand,
                Tag = TextInx,
            };
            labelText.MouseDown += LabelText_MouseDown;
            labelText.MouseMove += LabelText_MouseMove;
            labelText.MouseUp += LabelText_MouseUp;
            return labelText;
        }

        private void LabelText_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Label tmp = (Label)sender;
            tmp.ReleaseMouseCapture();
        }

        private void LabelText_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Label tmp = (Label)sender;
                double dx = e.GetPosition(null).X - jos.X + tmp.Margin.Left;
                double dy = e.GetPosition(null).Y - jos.Y + tmp.Margin.Top;
                if (dx > 0 && dy > 0)
                {
                    tmp.Margin = new Thickness(dx, dy, 0, 0);
                }
                jos = e.GetPosition(null);
            }
        }

        private void LabelText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label tmp = (Label)sender;
            jos = e.GetPosition(null);
            tmp.CaptureMouse();
            tmp.Cursor = Cursors.Hand;
            if (e.RightButton == MouseButtonState.Pressed)
            {
                TextRedact mapRedact = new TextRedact(Convert.ToInt32(tmp.Tag), GetCanvas);
                mapRedact.ShowDialog();
            }
        }




        #endregion

        TagInfoBLL tagInfo = new TagInfoBLL();
        LineInfoBLL infoBLL = new LineInfoBLL();
        widgetManagementBLL managementBLL = new widgetManagementBLL();
        public void LoadDataInfo(Canvas mainPanel, long Time)
        {
            LoadTag(mainPanel, Time);
            LoadLine(Time);
            WidgetLoad(Time);
            painting.mainPan = mainPanel;
            foreach (var item in wirePointArrays)
            {
                mainPanel.Children.Remove(item.GetPath);
                if (item.Paths != null)
                {
                    foreach (Path it in item.Paths)
                    {
                        GetCanvas.Children.Remove(it);
                    }
                }
            }
            painting.Zoom(1);

        }

        /// <summary>
        /// 加载所有Tag
        /// </summary>
        /// <param name="painti"></param>
        public void LoadTag(Canvas mainPanel, long Times)
        {
            DataTable items = tagInfo.RataTable(Times.ToString());
            int ite = items.Rows.Count;

            foreach (DataRow item in items.Rows)
            {
                int id = Convert.ToInt32(item["TagName"].ToString());
                Console.WriteLine(Convert.ToInt32(item["TagName"].ToString()));
                valuePairs.Add(Convert.ToInt32(item["TagName"].ToString()), TagCreate(new Point() { X = (Convert.ToDouble(item["X"].ToString()) * 10) - 19, Y = (Convert.ToDouble(item["Y"].ToString()) * 10) - 11.5 }, Convert.ToInt32(item["TagName"].ToString()), true));
                s = Convert.ToInt32(item["TagName"].ToString());
            }
            s++;
            GetCanvas = mainPanel;

        }

        /// <summary>
        /// 载入线路
        /// </summary>
        /// <param name="Times"></param>
        public void LoadLine(long Times)
        {
            DataTable data = infoBLL.LinelistArrer(Times.ToString());
            foreach (DataRow item in data.Rows)
            {
                if (Convert.ToInt32(item["LineStyel"].ToString()) == 1)
                {
                    GetCircuitType = (CircuitType.Line);
                }
                else if (Convert.ToInt32(item["LineStyel"].ToString()) == 2)
                {
                    GetCircuitType = (CircuitType.Broken);
                }
                else
                {
                    GetCircuitType = (CircuitType.Semicircle);
                }
                Pairsarray.Add(new WirePoint() { TagID = Convert.ToInt32(item["Tag1"].ToString().Substring(2)), SetPoint = new Point() { X = Convert.ToDouble(item["StartX"].ToString()) * 10, Y = Convert.ToDouble(item["StartY"].ToString()) * 10 } });
                Pairsarray.Add(new WirePoint() { TagID = Convert.ToInt32(item["Tag2"].ToString().Substring(2)), SetPoint = new Point() { X = Convert.ToDouble(item["EndX"].ToString()) * 10, Y = Convert.ToDouble(item["EndY"].ToString()) * 10 } });
                AddLine();
                Pairsarray.Clear();
            }
        }

        /// <summary>
        /// 载入区域和文字
        /// </summary>
        /// <param name="Times"></param>
        public void WidgetLoad(long Times)
        {
            DataTable da = managementBLL.GetWidget(Times.ToString());
            foreach (DataRow item in da.Rows)
            {
                if (item["WidgetNo"].ToString().Substring(0, 2).Equals("AR"))
                {
                    keyValuePairs.Add(Convert.ToInt32(item["WidgetNo"].ToString().Substring(2)), NewArea((new Point() { X = Convert.ToDouble(item["X"].ToString()) * 10, Y = Convert.ToDouble(item["Y"].ToString()) * 10 }), item["Name"].ToString(), Convert.ToInt32(item["WidgetNo"].ToString().Substring(2)), item["BackColor"].ToString(), item["ForeColor"].ToString(), item["BorderColor"].ToString(), Convert.ToDouble(item["FontSize"].ToString()), Convert.ToDouble(item["Width"].ToString()) * 10, Convert.ToDouble(item["Height"].ToString()) * 10, true, item["FontPosition"].ToString()));
                    index = Convert.ToInt32(item["WidgetNo"].ToString().Substring(2));
                }
                else if (item["WidgetNo"].ToString().Substring(0, 2).Equals("TE"))
                {
                    GetKeyValues.Add(Convert.ToInt32(item["WidgetNo"].ToString().Substring(2)), FontNew((new Point() { X = Convert.ToDouble(item["X"].ToString()) * 10, Y = Convert.ToDouble(item["Y"].ToString()) * 10 }), item["Name"].ToString(), Convert.ToInt32(item["WidgetNo"].ToString().Substring(2)), Convert.ToDouble(item["FontSize"].ToString()), item["ForeColor"].ToString(), true));
                    TextInx = Convert.ToInt32(item["WidgetNo"].ToString().Substring(2));
                }
            }
            index++;
            TextInx++;
        }


        /// <summary>
        /// 直线线路点击
        /// </summary>
        public void Mapstraight()
        {
            PathStatic = true;
            GetCircuitType = CircuitType.Line;//绘制直线
            TagFormer();
        }

        /// <summary>
        /// 鼠标点击
        /// </summary>
        public void mouseStatic()
        {
            PathStatic = false;
            TagFormer();
        }

        /// <summary>
        /// 半圆线路
        /// </summary>
        public void Semicircles()
        {
            PathStatic = true;
            GetCircuitType = CircuitType.Semicircle;//绘制半圆
            TagFormer();
        }

        /// <summary>
        /// 折线
        /// </summary>
        public void Brokene()
        {
            PathStatic = true;
            GetCircuitType = CircuitType.Broken;//折线绘制
            TagFormer();
        }

        #region 清除线路

        /// <summary>
        /// 清除线路
        /// </summary>
        public void ClearCircuit()
        {
            //double ey = Pairsarray[0].SetPoint.X - Pairsarray[1].SetPoint.X; //大于0下方上方小于0上方
            foreach (WirePointArray item in wirePointArrays.ToArray())
            {
                if ((item.GetPoint.TagID.Equals(Pairsarray[0].TagID) && item.GetWirePoint.TagID.Equals(Pairsarray[1].TagID)) || (item.GetPoint.TagID.Equals(Pairsarray[1].TagID) && item.GetWirePoint.TagID.Equals(Pairsarray[0].TagID)))
                {
                    if (item.circuitType.Equals(CircuitType.Broken))
                    {
                        CrearS(item);
                        break;
                    }
                    else if (item.circuitType.Equals(CircuitType.Semicircle))
                    {
                        CrearS(item);
                        break;
                    }
                    else
                    {
                        CrearS(item);
                        break;
                    }
                }
            }
            Pairsarray.Clear();
        }

        public void CrearS(WirePointArray item)
        {
            GetCanvas.Children.Remove(item.GetPath);
            if (item.Paths != null)
            {
                foreach (var it in item.Paths)
                {
                    GetCanvas.Children.Remove(it);
                }
            }
            wirePointArrays.Remove(item);
        }



        /// <summary>
        /// 清除
        /// </summary>
        public void ClearTen()
        {
            PathStatic = true;
            GetCircuitType = CircuitType.Clear;//清除
            TagFormer();
        }

        #endregion


    }
    public class WirePoint
    {
        /// <summary>
        /// Tag编号
        /// </summary>
        public int TagID { get; set; }

        /// <summary>
        /// 对应中心点
        /// </summary>
        public Point SetPoint { get; set; }
    }

    public class WirePointArray
    {
        /// <summary>
        /// 线路起始位置信息
        /// </summary>
        public WirePoint GetPoint { get; set; }

        /// <summary>
        /// 线路结束点位置信息
        /// </summary>
        public WirePoint GetWirePoint { get; set; }

        /// <summary>
        /// 直线对象
        /// </summary>
        public Path GetPath { get; set; }

        /// <summary>
        /// 线路类型
        /// </summary>
        public CircuitType circuitType { get; set; }

        /// <summary>
        /// 绘制方向(顺时针/逆时针)
        /// </summary>
        public int Direction { get; set; }//0,顺时针,1,逆时针

        /// <summary>
        /// 折线对象
        /// </summary>
        public List<Path> Paths { get; set; }//三部分（两直线，一半圆）
    }
}
