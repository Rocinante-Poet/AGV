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
using AGVManagement.Enumeration;
using AGVManagement.MapPaint;

namespace AGVManagement
{
    /// <summary>
    /// TextRedact.xaml 的交互逻辑
    /// </summary>
    public partial class TextRedact : Window
    {
        private int TageIndex;
        private Canvas  GetCanvas;
        private TextManag tagManag = new TextManag();
        private AreaCompile areaCompile = new AreaCompile();
        Label label;
        public TextRedact(int AreaNum, Canvas canvas)
        {
            InitializeComponent();
            TageIndex = AreaNum;
            GetCanvas = canvas;
            TextSelect();
        }

        /// <summary>
        /// 信息查询
        /// </summary>
        public void TextSelect()
        {
            label = tagManag.TextSelct(TageIndex);
            TextName.Text = label.Content.ToString();
            Fontsize.Text = (label.FontSize / Painting.siseWin).ToString();
            DisX.Text = (label.Margin.Left/Painting.siseWin).ToString();
            DisY.Text = (label.Margin.Top / Painting.siseWin).ToString();
            FontColor.Text = areaCompile.AreaColor(label.Foreground.ToString());
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            label.Content = TextName.Text;
            label.FontSize = Convert.ToInt32(Fontsize.Text) * Painting.siseWin;
            label.Margin = new Thickness(Convert.ToInt32(DisX.Text)* Painting.siseWin,Convert.ToInt32(DisY.Text) * Painting.siseWin, 0,0);
            areaCompile.AreaColor(label, FontColor.Text, Colortype.FontColor);
            this.Close();
        }

        /// <summary>
        /// 文字删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            tagManag.TextDelete(TageIndex, GetCanvas);
            this.Close();
        }
    }
}
