using AGV.BLL;
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

namespace AGVManagement
{
    /// <summary>
    /// Operation.xaml 的交互逻辑
    /// </summary>
    public partial class Operation : Window
    {
        MapMessageBLL mapMessage = new MapMessageBLL();
        public Operation()
        {
            InitializeComponent();
            mapLoad();
        }
        public void mapLoad()
        {
            int Index = 0;
            int s = 0;
            string Time = mapMessage.SettingInfoMap();
            DataTable da = mapMessage.GetMapData(null);
            if (da != null)
            {
                foreach (DataRow data in da.Rows)
                {
                    if (Time != null)
                    {

                        if (Time.Equals(data["CreateTime"].ToString()))
                        {
                            Index = s;
                        }
                    }
                    ComboBoxItem ite = new ComboBoxItem();
                    ite.Content = data["Name"].ToString();
                    ite.Tag = data["CreateTime"].ToString();
                    Maplistq.Items.Add(ite);
                    s++;
                }
                Maplistq.SelectedIndex = Index;
            }
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            if (((ComboBoxItem)Maplistq.SelectedItem).Tag != null)
            {
                if (mapMessage.UpdateSettingMap(Convert.ToInt32(((ComboBoxItem)Maplistq.SelectedItem).Tag), 1))
                {
                    MessageBox.Show("保存成功");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("保存失败");
                }
            }
        }
    }
}
