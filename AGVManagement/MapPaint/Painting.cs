using AGVManagement.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AGVManagement.MapPaint
{
    /// <summary>
    /// 地图管理
    /// </summary>
    public class Painting
    {
        public Canvas mainPan;
        private int x = 100; //X轴刻度大小
        private int y = 100; //Y轴刻度大小


        /// <summary>
        /// 绘制直线
        /// </summary>
        /// <param name="startPt">起始点</param>
        /// <param name="endPt">结束点</param>
        /// <param name="mainPanel">绘制容器</param>
        public Path DrawingLine(Point startPt, Point endPt, Canvas mainPanel)
        {
            LineGeometry myLineGeometry = new LineGeometry();//实例化一条直线
            myLineGeometry.StartPoint = startPt;//设置起点
            myLineGeometry.EndPoint = endPt;//设置终点
            

            Path myPath = new Path();
      
            myPath.Stroke = Brushes.Black;//设置颜色
            myPath.StrokeThickness = 1; //设置宽度
            myPath.Data = myLineGeometry;//设置绘制形状
            mainPanel.Children.Add(myPath);//绑定数据
            return myPath;
        }

        /// <summary>
        /// 绘制半圆线路
        /// </summary>
        /// <param name="mainPanel"></param>
        public Path DrawingSemicircle(Point startPt, Point endPt, Canvas mainPanel)
        {
            return GetPath(startPt, endPt, mainPanel, false, 50);
        }

        /// <summary>
        /// 半圆
        /// </summary>
        /// <param name="startPt"></param>
        /// <param name="endPt"></param>
        /// <param name="mainPanel"></param>
        /// <param name="Static"></param>
        /// <param name="Sise"></param>
        /// <returns></returns>
        public Path GetPath(Point startPt, Point endPt, Canvas mainPanel, bool Static, int Sise)
        {
            Path path = new Path();
            PathGeometry pathGeometry = new PathGeometry();
            ArcSegment arc = new ArcSegment(startPt, new Size(Sise, Sise), 0, false, Static ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, true);
            PathFigure figure = new PathFigure();
            figure.StartPoint = endPt;
            figure.Segments.Add(arc);
            pathGeometry.Figures.Add(figure);
            path.Data = pathGeometry;
            path.Stroke = Brushes.Black;
            mainPanel.Children.Add(path);
            return path;
        }

        /// <summary>
        /// 绘制折线线路
        /// </summary>
        /// <param name="startPt"></param>
        /// <param name="endPt"></param>
        /// <param name="mainPanel"></param>
        public List<Path> DrawingBroken(Point startPt, Point endPt, Canvas mainPanel)
        {
            List<Path> GetPatjs = new List<Path>();
            double drn = startPt.X - endPt.X;
            double hrn = startPt.Y - endPt.Y;

            if ((drn > 0 && hrn < 0) || (drn < 0 && hrn > 0))
            {
                double diff = startPt.Y - endPt.Y;
                if (diff < 0)
                {
                    Point TX = new Point() { X = endPt.X + 19, Y = startPt.Y };
                    Point TY = new Point() { X = endPt.X, Y = startPt.Y + 11.5 };
                    GetPatjs.Add(DrawingLine(endPt, TY, mainPanel));
                    GetPatjs.Add(DrawingLine(startPt, TX, mainPanel));
                    GetPatjs.Add(GetPath(TX, TY, mainPanel, true, 18));
                }
                else
                {
                    Point TX = new Point() { X = endPt.X - 19, Y = startPt.Y };
                    Point TY = new Point() { X = endPt.X, Y = startPt.Y - 11.5 };
                    GetPatjs.Add(DrawingLine(startPt, TX, mainPanel));
                    GetPatjs.Add(DrawingLine(endPt, TY, mainPanel));
                    GetPatjs.Add(GetPath(TX, TY, mainPanel, true, 18));
                }
            }
            else
            {
                double diff = startPt.Y - endPt.Y;
                if (diff < 0)
                {
                    Point TX = new Point() { X = endPt.X - 19, Y = startPt.Y };
                    Point TY = new Point() { X = endPt.X, Y = startPt.Y + 11.5 };
                    GetPatjs.Add(DrawingLine(startPt, TX, mainPanel));
                    GetPatjs.Add(DrawingLine(endPt, TY, mainPanel));
                    GetPatjs.Add(GetPath(TX, TY, mainPanel, false, 18));
                }
                else
                {
                    Point TX = new Point() { X = endPt.X + 19, Y = startPt.Y };
                    Point TY = new Point() { X = endPt.X, Y = startPt.Y - 11.5 };
                    GetPatjs.Add(DrawingLine(endPt, TY, mainPanel));
                    GetPatjs.Add(DrawingLine(startPt, TX, mainPanel));
                    GetPatjs.Add(GetPath(TX, TY, mainPanel, false, 18));
                }
            }

            return GetPatjs;
        }


        /// <summary>
        /// 绘制地图坐标系
        /// </summary>
        /// <param name="mainPanel"></param>
        public void Coordinate(Canvas mainPanel)
        {
            mainPan = mainPanel;
            for (int i = 0; i <= (mainPanel.Height / x); i++)
            {
                DrawingLine(new Point(0, x * i), new Point(mainPanel.Width, y * i), mainPanel);
            }
            for (int i = 0; i <= (mainPanel.Width / x); i++)
            {
                DrawingLine(new Point(x * i, 0), new Point(x * i, mainPanel.Height), mainPanel);
            }
        }

        /// <summary>
        /// 绘制X轴Y轴刻度
        /// </summary>
        /// <param name="mainPanel"></param>
        /// <param name="mainPane2"></param>
        public void CoordinateX(Canvas mainPanel, Canvas mainPane2)
        {

            for (int i = 0; i <= mainPanel.Width / x; i++)
            {
                //绘制X轴刻度
                TextBlock textX = new TextBlock();
                textX.Text = (i * 10).ToString();
                textX.Foreground = new SolidColorBrush(Colors.Black);
                if (i != (mainPanel.Width / x))
                {
                    Canvas.SetLeft(textX, (i * x) - 8);//10(X轴偏移量)
                }
                else
                {
                    Canvas.SetLeft(textX, (i * x) - 35);//10(X轴偏移量)
                }
                Canvas.SetTop(textX, 0);
                mainPanel.Children.Add(textX);


            }

            for (int i = 0; i <= (mainPane2.Height / x); i++)
            {
                //绘制Y轴刻度
                TextBlock textY = new TextBlock();
                if (!i.Equals(0))
                    textY.Text = (i * 10).ToString();
                textY.Foreground = new SolidColorBrush(Colors.Black);
                Canvas.SetLeft(textY, 0);
                if (i != (mainPane2.Height / x))
                {
                    Canvas.SetTop(textY, (i * x) - 7.5);//(-7.5Y轴偏移量)
                }
                else
                {
                    Canvas.SetTop(textY, (i * x) - 10);//(-7.5Y轴偏移量)
                }
                
                mainPane2.Children.Add(textY);
            }
        }

        public static int siseWin = 1;//上一次缩放大小
        /// <summary>
        /// 地图比例尺缩放
        /// </summary>
        /// <param name="Size"></param>
        /// <param name="mainPanel"></param>
        /// <param name="mainPane2"></param>
        public void Mapmagnify(int Size, Canvas mainPanel, Canvas mainPane2, Canvas mainPane3,double ms,double mp)
        {
            mainPane2.Children.Clear();
            mainPane3.Children.Clear();
            mainPanel.Children.Clear();
            if (Size.Equals(0))
            {
                x = 100;
                y = 100;
                mainPane3.Width = ms;
                mainPanel.Width = ms;
                mainPane2.Width = mp;
                Coordinate(mainPane3);
                CoordinateX(mainPanel, mainPane2);
                Zoom(1);
                siseWin = 1;
            }
            else
            {
                mainPane3.Width = Size * ms;
                mainPane3.Height = Size * mp;
                mainPanel.Width = Size * ms;
                mainPane2.Height = Size * mp;
                x = 100 * Size;
                y = 100 * Size;
                Coordinate(mainPane3);
                CoordinateX(mainPanel, mainPane2);
                Zoom(Size);
                siseWin = Size;
            }
        }

        /// <summary>
        /// 比例尺控件缩放
        /// </summary>
        /// <param name="Sise"></param>
        public void Zoom(int Sise)
        {
            foreach (WirePointArray item in MapInstrument.wirePointArrays)//线路缩放
            {
                Path path = null;
                List<Path> Kaths = null;
                Point Henpoint = item.GetPoint.SetPoint;
                Henpoint.X =( (Henpoint.X / siseWin) * Sise);
                Henpoint.Y = (((Henpoint.Y) / siseWin) * Sise);
                item.GetPoint.SetPoint = Henpoint;
                Point point = item.GetWirePoint.SetPoint;
                point.X = (((point.X) / siseWin)) * Sise ;
                point.Y = (((point.Y ) / siseWin) * Sise);
                item.GetWirePoint.SetPoint = point;
                if (item.circuitType.Equals(CircuitType.Line))//直线
                {
                    path = DrawingLine(item.GetPoint.SetPoint, item.GetWirePoint.SetPoint, mainPan);
                    path.StrokeThickness = item.GetPath.StrokeThickness;
                    path.Stroke = item.GetPath.Stroke;
                }
                else if (item.circuitType.Equals(CircuitType.Semicircle))//半圆
                {
                    path = DrawingSemicircle(item.GetPoint.SetPoint, item.GetWirePoint.SetPoint, mainPan);
                    path.StrokeThickness = item.GetPath.StrokeThickness;
                    path.Stroke = item.GetPath.Stroke;
                }
                else if (item.circuitType.Equals(CircuitType.Broken))//折线
                {
                    Kaths = DrawingBroken(item.GetPoint.SetPoint, item.GetWirePoint.SetPoint, mainPan);
                    foreach (var ite in Kaths)
                    {
                        foreach (var it in item.Paths)
                        {
                            ite.StrokeThickness = it.StrokeThickness;
                            ite.Stroke = it.Stroke;
                        }
                    }
                    
                }
                item.GetPath = path;
                item.Paths = Kaths;
            }
            foreach (int item in MapInstrument.valuePairs.Keys)//信标缩放
            {
                MapInstrument.valuePairs[item].Margin = new Thickness(((MapInstrument.valuePairs[item].Margin.Left+19) / siseWin) * Sise-19, ((MapInstrument.valuePairs[item].Margin.Top+11.5) / siseWin) * Sise-11.5, 0, 0);
                mainPan.Children.Add(MapInstrument.valuePairs[item]);
            }
            foreach (int item in MapInstrument.keyValuePairs.Keys)//区域缩放
            {
                MapInstrument.keyValuePairs[item].Margin = new Thickness((MapInstrument.keyValuePairs[item].Margin.Left / siseWin) * Sise, (MapInstrument.keyValuePairs[item].Margin.Top / siseWin) * Sise, 0, 0);
                //长宽计算
                MapInstrument.keyValuePairs[item].Width = MapInstrument.keyValuePairs[item].Width /siseWin* Sise ;
                MapInstrument.keyValuePairs[item].Height = MapInstrument.keyValuePairs[item].Height / siseWin * Sise;
                //字体计算
                MapInstrument.keyValuePairs[item].FontSize = MapInstrument.keyValuePairs[item].FontSize / siseWin * Sise;
                mainPan.Children.Add(MapInstrument.keyValuePairs[item]);
            }

            foreach (int item in MapInstrument.GetKeyValues.Keys)//文字缩放
            {
                MapInstrument.GetKeyValues[item].Margin = new Thickness((MapInstrument.GetKeyValues[item].Margin.Left / siseWin) * Sise, (MapInstrument.GetKeyValues[item].Margin.Top / siseWin) * Sise, 0, 0);
                MapInstrument.GetKeyValues[item].FontSize = MapInstrument.GetKeyValues[item].FontSize / siseWin * Sise;
                mainPan.Children.Add(MapInstrument.GetKeyValues[item]);
            }
        }
    }
}
