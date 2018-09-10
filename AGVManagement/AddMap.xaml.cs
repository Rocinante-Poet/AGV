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
    /// AddMap.xaml 的交互逻辑
    /// </summary>
    public partial class AddMap : Window
    {
        public AddMap()
        {
            InitializeComponent();
        }

        private void OFFMp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region 验证
        private bool Validator()
        {
            if (MapSizeN.Text.Trim() == "")
            {
                return false;
            }
            else if (MapSizeW.Text.Trim() == "")
            {
                return false;
            }
            else if (MapSizeH.Text.Trim() == "")
            {
                return false;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 表单提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapSum_Click(object sender, RoutedEventArgs e)
        {
            if (!Validator())
                return;
            this.Close();
            long gs = 0;
            MainWindow main = new MainWindow(gs, null, MapSizeN.Text,Convert.ToDouble(MapSizeW.Text.Trim()), Convert.ToDouble(MapSizeH.Text.Trim()));
            main.ShowDialog();
            

        }
    }
}
