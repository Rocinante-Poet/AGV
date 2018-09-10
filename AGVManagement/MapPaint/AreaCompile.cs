using AGVManagement.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AGVManagement.MapPaint
{
    /// <summary>
    /// 区域编辑
    /// </summary>
    public class AreaCompile
    {
        /// <summary>
        /// 根据AreaID查询字典中的Label
        /// </summary>
        /// <returns></returns>
        public Label AreaSelct(int AreaID)
        {
            foreach (int item in MapInstrument.keyValuePairs.Keys)
            {
                if (item.Equals(AreaID))
                {
                    return MapInstrument.keyValuePairs[item];
                }
            }
            return null;
        }


        /// <summary>
        /// 区域删除
        /// </summary>
        /// <param name="AreaID"></param>
        /// <returns></returns>
        public void ArDelete(int AreaID, Canvas GetCanvas)
        {
            GetCanvas.Children.Remove(MapInstrument.keyValuePairs[AreaID]);
            MapInstrument.keyValuePairs.Remove(AreaID);
        }
   

        #region 获取/设置字体对齐方式

        /// <summary>
        /// 区域文字对齐方式
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public string aAlignment(Label label)
        {
            if (label.HorizontalContentAlignment.Equals(HorizontalAlignment.Center) && label.VerticalContentAlignment.Equals(VerticalAlignment.Center))
            {
                return "居中对齐";
            }
            else if (label.HorizontalContentAlignment.Equals(HorizontalAlignment.Left) && label.VerticalContentAlignment.Equals(VerticalAlignment.Center))
            {
                return "靠左对齐";
            }
            else if (label.HorizontalContentAlignment.Equals(HorizontalAlignment.Right) && label.VerticalContentAlignment.Equals(VerticalAlignment.Center))
            {
                return "靠右对齐";
            }
            else if (label.HorizontalContentAlignment.Equals(HorizontalAlignment.Center) && label.VerticalContentAlignment.Equals(VerticalAlignment.Top))
            {
                return "顶部对齐";
            }
            else if (label.HorizontalContentAlignment.Equals(HorizontalAlignment.Center) && label.VerticalContentAlignment.Equals(VerticalAlignment.Bottom))
            {
                return "底部对齐";
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 设置文字对齐方式
        /// </summary>
        /// <param name="alignment">对齐方式</param>
        /// <param name="label">控件</param>
        public void aAlignment(string alignment,Label label)
        {
            if (alignment.Equals("居中对齐"))
            {
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;
            }
            else if (alignment.Equals("靠左对齐"))
            {
                label.HorizontalContentAlignment = HorizontalAlignment.Left;
                label.VerticalContentAlignment = VerticalAlignment.Center;
            }
            else if (alignment.Equals("靠右对齐"))
            {
                label.HorizontalContentAlignment = HorizontalAlignment.Right;
                label.VerticalContentAlignment = VerticalAlignment.Center;
            }
            else if (alignment.Equals("顶部对齐"))
            {
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Top;
            }
            else if (alignment.Equals("底部对齐"))
            {
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Bottom;
            }
        }

        #endregion


        #region 获取/设置背景/字体颜色


        /// <summary>
        /// 区域背景/字体颜色
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public string AreaColor(string bgColor)
        {
            if (bgColor.Equals((Colors.White).ToString()))
            {
                return "白色";
            }
            else if (bgColor.Equals((Colors.Black).ToString()))
            {
                return "黑色";
            }
            else if (bgColor.Equals((Colors.Red).ToString()))
            {
                return "红色";
            }
            else if (bgColor.Equals((Colors.Orange).ToString()))
            {
                return "橙色";
            }
            else if (bgColor.Equals((Colors.Yellow).ToString()))
            {
                return "黄色";
            }
            else if (bgColor.Equals((Colors.Green).ToString()))
            {
                return "绿色";
            }
            else if (bgColor.Equals((Colors.Cyan).ToString()))
            {
                return "青色";
            }
            else if (bgColor.Equals((Colors.Blue).ToString()))
            {
                return "蓝色";
            }
            else if (bgColor.Equals((Colors.Violet).ToString()))
            {
                return "紫色";
            }
            return "";
        }

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="color">颜色名称</param>
        public void AreaColor(Control control, string color, Colortype existx)
        {
            if (color.Equals("白色"))
            {
                if (existx.Equals(Colortype.FontColor))
                {
                    control.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                }
                else if (existx.Equals(Colortype.BgColor))
                {
                    control.Background = new SolidColorBrush(Colors.WhiteSmoke);
                }
                else if(existx.Equals(Colortype.BrColor))
                {
                    control.BorderBrush = new SolidColorBrush(Colors.WhiteSmoke);
                }
            }
            else if (color.Equals("黑色"))
            {
                if (existx.Equals(Colortype.FontColor))
                {
                    control.Foreground = new SolidColorBrush(Colors.Black);
                }
                else if (existx.Equals(Colortype.BgColor))
                {
                    control.Background = new SolidColorBrush(Colors.Black);
                }
                else if (existx.Equals(Colortype.BrColor))
                {
                    control.BorderBrush = new SolidColorBrush(Colors.Black);
                }
            }
            else if (color.Equals("红色"))
            {
                if (existx.Equals(Colortype.FontColor))
                {
                    control.Foreground = new SolidColorBrush(Colors.Red);
                }
                else if (existx.Equals(Colortype.BgColor))
                {
                    control.Background = new SolidColorBrush(Colors.Red);
                }
                else if (existx.Equals(Colortype.BrColor))
                {
                    control.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }
            else if (color.Equals("橙色"))
            {
                if (existx.Equals(Colortype.FontColor))
                {
                    control.Foreground = new SolidColorBrush(Colors.Orange);
                }
                else if (existx.Equals(Colortype.BgColor))
                {
                    control.Background = new SolidColorBrush(Colors.Orange);
                }
                else if (existx.Equals(Colortype.BrColor))
                {
                    control.BorderBrush = new SolidColorBrush(Colors.Orange);
                }
            }
            else if (color.Equals("黄色"))
            {
                if (existx.Equals(Colortype.FontColor))
                {
                    control.Foreground = new SolidColorBrush(Colors.Yellow);
                }
                else if (existx.Equals(Colortype.BgColor))
                {
                    control.Background = new SolidColorBrush(Colors.Yellow);
                }
                else if (existx.Equals(Colortype.BrColor))
                {
                    control.BorderBrush = new SolidColorBrush(Colors.Yellow);
                }
            }
            else if (color.Equals("绿色"))
            {
                if (existx.Equals(Colortype.FontColor))
                {
                    control.Foreground = new SolidColorBrush(Colors.Green);
                }
                else if (existx.Equals(Colortype.BgColor))
                {
                    control.Background = new SolidColorBrush(Colors.Green);
                }
                else if (existx.Equals(Colortype.BrColor))
                {
                    control.BorderBrush = new SolidColorBrush(Colors.Green);
                }
            }
            else if (color.Equals("青色"))
            {
                if (existx.Equals(Colortype.FontColor))
                {
                    control.Foreground = new SolidColorBrush(Colors.Cyan);
                }
                else if (existx.Equals(Colortype.BgColor))
                {
                    control.Background = new SolidColorBrush(Colors.Cyan);
                }
                else if (existx.Equals(Colortype.BrColor))
                {
                    control.BorderBrush = new SolidColorBrush(Colors.Cyan);
                }
            }
            else if (color.Equals("蓝色"))
            {
                if (existx.Equals(Colortype.FontColor))
                {
                    control.Foreground = new SolidColorBrush(Colors.Blue);
                }
                else if (existx.Equals(Colortype.BgColor))
                {
                    control.Background = new SolidColorBrush(Colors.Blue);
                }
                else if (existx.Equals(Colortype.BrColor))
                {
                    control.BorderBrush = new SolidColorBrush(Colors.Blue);
                }
            }
            else if (color.Equals("紫色"))
            {
                if (existx.Equals(Colortype.FontColor))
                {
                    control.Foreground = new SolidColorBrush(Colors.Violet);
                }
                else if (existx.Equals(Colortype.BgColor))
                {
                    control.Background = new SolidColorBrush(Colors.Violet);
                }
                else if (existx.Equals(Colortype.BrColor))
                {
                    control.BorderBrush = new SolidColorBrush(Colors.Violet);
                }
            }
        }

        #endregion





    }
}
