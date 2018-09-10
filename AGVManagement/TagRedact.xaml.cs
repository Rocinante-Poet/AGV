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
using System.Windows.Shapes;

namespace AGVManagement
{
    /// <summary>
    /// TagRedact.xaml 的交互逻辑
    /// </summary>
    public partial class TagRedact : Window
    {
        private int TagIndex;
        private Canvas Canvas;
        private Label label;
        private TagManag tagManag = new TagManag();
        public delegate void Movement(Point point, int TagID);
        public Movement GetMovement; //信标移动委托
        public TagRedact(int TagID, Canvas canvas)
        {
            InitializeComponent();
            this.TagIndex = TagID;
            this.Canvas = canvas;
            TagSelect();
        }

        /// <summary>
        /// 信标信息查询
        /// </summary>
        private void TagSelect()
        {
            label = tagManag.TagSelct(TagIndex);
            TagX.Text = (label.Margin.Left/ Painting.siseWin).ToString();
            TagY.Text = (label.Margin.Top/ Painting.siseWin).ToString();
        }



        /// <summary>
        /// 信标编辑提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tag_Submit_Click(object sender, RoutedEventArgs e)
        {
            foreach (WirePointArray item in MapInstrument.wirePointArrays)//查询所有关联线路
            {
                if (item.GetPoint.TagID.Equals(TagIndex) || item.GetWirePoint.TagID.Equals(TagIndex))//暂时移除拖动时关联线路
                {
                    Canvas.Children.Remove(item.GetPath);
                    if (item.Paths != null)
                    {
                        foreach (var ite in item.Paths)
                        {
                            Canvas.Children.Remove(ite);
                        }
                    }
                }
            }
            label.Margin = new Thickness(Convert.ToDouble(TagX.Text.Trim()) * Painting.siseWin, Convert.ToDouble(TagY.Text.Trim()) * Painting.siseWin, 0, 0);
            GetMovement(new Point() { X = label.Margin.Left + 19, Y = label.Margin.Top + 11.5 }, TagIndex);
            this.Close();
        }

        /// <summary>
        /// 信标删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TagDelete_Click(object sender, RoutedEventArgs e)
        {
            tagManag.TagDelete(TagIndex, Canvas);
            this.Close();
        }
    }
}
