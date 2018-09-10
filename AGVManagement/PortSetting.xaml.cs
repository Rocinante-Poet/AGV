using AGV.BLL;
using AGV.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
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
    /// PortSetting.xaml 的交互逻辑
    /// </summary>
    public partial class PortSetting : Window
    {

        MapMessageBLL mapMessage = new MapMessageBLL();
        DataTable dt = new DataTable();
        bool UpdateStatic = false; //是否为修改状态
        MapMessageBLL map = new MapMessageBLL();
        long Times;
        Main GetMap;
        public PortSetting(Window box)
        {
            InitializeComponent();
            GetMap = box as Main;
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



        public void portLoad(string Time)
        {

            if (Time != null)
            {
                Times = long.Parse(Time);
                DataTable PortData = mapMessage.LoadDeviceMap(long.Parse(Time));
                dt = new DataTable("PortInfo");
                dt.Columns.Add(new DataColumn("Com"));
                dt.Columns.Add(new DataColumn("Baud"));
                dt.Columns.Add(new DataColumn("Agv"));
                foreach (DataRow item in PortData.Rows)
                {
                    if (item["Agv"].ToString() == "Button")
                    {
                        dt.Rows.Add(new object[] { "COM" + item["Com"].ToString(), item["Baud"].ToString(), "按钮" });
                    }
                    else if (item["Agv"].ToString() == "Charge")
                    {
                        dt.Rows.Add(new object[] { "COM" + item["Com"].ToString(), item["Baud"].ToString(), "充电机" });
                    }
                    else
                    {
                        dt.Rows.Add(new object[] { "COM" + item["Com"].ToString(), item["Baud"].ToString(), item["Agv"].ToString() });
                    }
                }
                PortTable.ItemsSource = dt.DefaultView;
                PortTable.AutoGenerateColumns = false;

            }
            Com.Items.Clear();
            string[] polist = SerialPort.GetPortNames();
            for (int i = 0; i < polist.Length; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = polist[i];
                Com.Items.Add(item);
            }
        }

        private void PortTable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                if (PortTable.SelectedItems.Count > 0)
                {
                    ComLoad();
                    UpdateStatic = true;
                }
            }
        }
        public void ComLoad()
        {
            Com.Items.Clear();
            ComboBoxItem item = new ComboBoxItem();
            string ComName = ((DataRowView)PortTable.SelectedItem)[0].ToString();
            item.Content = ComName;
            Com.Items.Add(item);
            string[] polist = SerialPort.GetPortNames();
            for (int i = 0; i < polist.Length; i++)
            {
                if (!polist[i].Equals(ComName))
                {
                    ComboBoxItem ite = new ComboBoxItem();
                    ite.Content = polist[i];
                    Com.Items.Add(ite);
                }
            }
            Com.Text = ((DataRowView)PortTable.SelectedItem)[0].ToString();
            Baud.Text = ((DataRowView)PortTable.SelectedItem)[1].ToString();
            Agv.Text = ((DataRowView)PortTable.SelectedItem)[2].ToString();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortAdd_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatic = false;
            if (!Formverify())
                return;

            bool Comtyp = false;
            for (int i = 0; i < PortTable.Items.Count; i++)
            {
                if (Com.Text.Equals(((DataRowView)PortTable.Items[i])[0].ToString()))
                {
                    Comtyp = true;
                }
            }
            if (!Comtyp)
            {
                dt.Rows.Add(new object[] { Com.Text, Baud.Text, Agv.Text });
                Crear();
            }
            else
            {
                Crear();
                MessageBox.Show("此串口已存在！");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (PortTable.Items.Count > 0)
            {
                if (UpdateStatic)
                {
                    if (!Formverify())
                        return;
                    ((DataRowView)PortTable.SelectedItem)["Com"] = Com.Text;
                    ((DataRowView)PortTable.SelectedItem)["Baud"] = Baud.Text;
                    ((DataRowView)PortTable.SelectedItem)["Agv"] = Agv.Text;
                    Crear();
                }
            }
        }

        public void Crear()
        {
            Com.Text = "";
            Baud.Text = "";
            Agv.Text = "";
        }

        /// <summary>
        /// 表单验证
        /// </summary>
        public bool Formverify()
        {
            if (string.IsNullOrEmpty(Com.Text.Trim()) || string.IsNullOrEmpty(Baud.Text.Trim()) || string.IsNullOrEmpty(Agv.Text.Trim()))
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortDelete_Click(object sender, RoutedEventArgs e)
        {
            if (PortTable.Items.Count > 0)
            {
                if (UpdateStatic)
                {
                    if (!Formverify())
                        return;

                    dt.Rows.RemoveAt(PortTable.SelectedIndex);
                    Crear();
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            if (PortTable.Items.Count > 0)
            {

                PortInfo.AGVCom.Clear();
                PortInfo.Baud.Clear();
                PortInfo.agv.Clear();


                //PortInfo.buttonPort.Clear();
                PortInfo.buttonCom.Clear();
                PortInfo.buttonBaud.Clear();
                PortInfo.buttonStr.Clear();

                PortInfo.chargePort.Clear();
                PortInfo.chargeCom.Clear();
                PortInfo.chargeBaud.Clear();
                PortInfo.chargeStr.Clear();
                DataTable dr = new DataTable();
                for (int i = 0; i < PortTable.Columns.Count; i++)
                {
                    DataColumn dc = new DataColumn();
                    dr.Columns.Add(dc);
                }
                for (int i = 0; i < PortTable.Items.Count; i++)
                {
                    DataRow dt = dr.NewRow();
                    string type = ((DataRowView)PortTable.Items[i])[2].ToString().Trim();
                    if (type.Equals("按钮"))
                    {
                        dt[0] = ((DataRowView)PortTable.Items[i])[0].ToString().Trim().Substring(3);
                        dt[1] = ((DataRowView)PortTable.Items[i])[1].ToString().Trim();
                        dt[2] = "Button";
                        PortInfo.buttonCom.Add(Convert.ToInt32(((DataRowView)PortTable.Items[i])[0].ToString().Trim().Substring(3)));
                        PortInfo.buttonBaud.Add(Convert.ToInt32(((DataRowView)PortTable.Items[i])[1].ToString().Trim()));
                        PortInfo.buttonStr.Add("Button");
                    }
                    else if (type.Equals("充电机"))
                    {
                        dt[0] = ((DataRowView)PortTable.Items[i])[0].ToString().Trim().Substring(3);
                        dt[1] = ((DataRowView)PortTable.Items[i])[1].ToString().Trim();
                        dt[2] = "Charge";
                        PortInfo.chargeCom.Add(Convert.ToInt32(((DataRowView)PortTable.Items[i])[0].ToString().Trim().Substring(3)));
                        PortInfo.chargeBaud.Add(Convert.ToInt32(((DataRowView)PortTable.Items[i])[1].ToString().Trim()));
                        PortInfo.chargeStr.Add("Charge");
                    }
                    else
                    {
                        dt[0] = ((DataRowView)PortTable.Items[i])[0].ToString().Trim().Substring(3);
                        dt[1] = ((DataRowView)PortTable.Items[i])[1].ToString().Trim();
                        dt[2] = ((DataRowView)PortTable.Items[i])[2].ToString().Trim();
                        PortInfo.AGVCom.Add(Convert.ToInt32(((DataRowView)PortTable.Items[i])[0].ToString().Trim().Substring(3)));
                        PortInfo.Baud.Add(Convert.ToInt32(((DataRowView)PortTable.Items[i])[1].ToString().Trim()));
                        PortInfo.agv.Add(((DataRowView)PortTable.Items[i])[2].ToString().Trim());
                    }
                    dr.Rows.Add(dt);
                }
                if (map.InsertDeviceMap(Times, dr))
                {
                    MessageBox.Show("保存成功！");
                    GetMap.Maplist_SelectionChanged(null, null);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("保存失败！");
                }
            }
        }

        private void Maplistq_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem)Maplistq.SelectedItem).Tag != null)
            {
                portLoad(((ComboBoxItem)Maplistq.SelectedItem).Tag.ToString());
            }
        }
    }
}
