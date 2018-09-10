using AGVManagement.Enumeration;
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
    /// MapRedact.xaml 的交互逻辑
    /// </summary>
    public partial class MapRedact : Window
    {
        private int AreaID;
        private Label label;
        private Canvas GetCanva;
        AreaCompile areaCompile = new AreaCompile();
        public MapRedact(int AreaNum, Canvas canvas)
        {
            InitializeComponent();
            this.AreaID = AreaNum;
            this.GetCanva = canvas;
            AreaShow();
        }

        /// <summary>
        /// 显示区域信息
        /// </summary>
        private void AreaShow()
        {
            label = areaCompile.AreaSelct(AreaID);
            MpName.Text = label.Content.ToString();
            Fontsize.Text = (label.FontSize/ Painting.siseWin).ToString();
            DisX.Text = (label.Margin.Left/ Painting.siseWin).ToString();
            Algcetion.Text = areaCompile.aAlignment(label);
            DisY.Text= (label.Margin.Top / Painting.siseWin).ToString();
            FontColor.Text = areaCompile.AreaColor(label.Foreground.ToString());
            ArWidth.Text = label.Width.ToString();
            BgColor.Text = areaCompile.AreaColor(label.Background.ToString());
            ArHeight.Text = label.Height.ToString();
            BrColor.Text= areaCompile.AreaColor(label.BorderBrush.ToString());
            Brwidth.Text = label.BorderThickness.Top.ToString();
        }

        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            if (!Formverify())
                return;

            label.Content = MpName.Text.Trim();
            label.FontSize = Convert.ToDouble(Fontsize.Text.Trim()) * Painting.siseWin;
            label.Margin = new Thickness(Convert.ToDouble(DisX.Text.Trim()) * Painting.siseWin, Convert.ToDouble(DisY.Text.Trim()) * Painting.siseWin, 0, 0);
            areaCompile.aAlignment(Algcetion.Text.Trim(), label);
            areaCompile.AreaColor(label, FontColor.Text, Colortype.FontColor);
            areaCompile.AreaColor(label, BgColor.Text, Colortype.BgColor);
            areaCompile.AreaColor(label, BrColor.Text, Colortype.BrColor);
            label.Width = Convert.ToDouble(ArWidth.Text.Trim());
            label.Height = Convert.ToDouble(ArHeight.Text.Trim());
            double da = Convert.ToDouble(Brwidth.Text.Trim());
            label.BorderThickness = new Thickness(da,da,da,da);
            this.Close();
        }

        #region 表单验证

        /// <summary>
        /// 表单验证
        /// </summary>
        /// <returns></returns>
        private bool Formverify()
        {
            if (MpName.Text.ToString().Trim().Equals(""))
            {
                MessageBox.Show("区域名称不能为空");
                return false;
            }
            if (Fontsize.Text.ToString().Trim().Equals(""))
            {
                MessageBox.Show("字体大小不能为空");
                return false;
            }
            if (DisX.Text.ToString().Trim().Equals(""))
            {
                MessageBox.Show("X轴距离不能为空");
                return false; 
            }
            if (DisY.Text.ToString().Trim().Equals(""))
            {
                MessageBox.Show("Y轴距离不能为空");
                return false;
            }
            if (ArWidth.Text.ToString().Trim().Equals(""))
            {
                MessageBox.Show("区域长度不能为空");
                return false;
            }
            if (ArHeight.Text.ToString().Trim().Equals(""))
            {
                MessageBox.Show("区域宽度不能为空");
                return false;
            }
            if (Brwidth.Text.ToString().Trim().Equals(""))
            {
                MessageBox.Show("边框宽度不能为空");
                return false;
            }
            return true;
        }

        #endregion


        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            areaCompile.ArDelete(AreaID, GetCanva);
            this.Close();
        }
    }
}
