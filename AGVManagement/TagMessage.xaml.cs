using AGV.BLL;
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
using System.Threading;
using System.Data;

namespace AGVManagement
{
    /// <summary>
    /// TagMessage.xaml 的交互逻辑
    /// </summary>
    public partial class TagMessage : Window
    {
        private List<object> arrlist;//选中数据
        private long Times;//关联时间
        private DataGrid dataGrid;
        private int index = 0;//选中索引
        private ScrollViewer GetScroll;
        MapInstrument map = new MapInstrument();
        public TagMessage(List<object> list,long Time,DataGrid grid, ScrollViewer scroll)
        {
            InitializeComponent();
            arrlist = list;
            Times = Time;
            dataGrid = grid;
            GetScroll = scroll;
            TagLoad();
            index = dataGrid.SelectedIndex;
        }

        /// <summary>
        /// 加载初始数据
        /// </summary>
        private void TagLoad()
        {
            map.TagFormer();//所有Tag还原为原色
            OperateDBBLL operate = new OperateDBBLL();
            Label label = MapInstrument.valuePairs[Convert.ToInt32(arrlist[0].ToString())];
            label.Background = new SolidColorBrush(Colors.Red);//选中Tag标记为红色
            string[] taglis = operate.SelectTagArr(Times, arrlist[0].ToString());//查询关联Tag
            for (int i = 0; i < taglis.Length; i++)//关联Tag标记绿色
            {
                if (!i.Equals(0))//排除第一个空值(N/A)
                {
                    MapInstrument.valuePairs[Convert.ToInt32(taglis[i].ToString())].Background = new SolidColorBrush(Colors.Green);
                }
            }

            #region 赋初始值
            TagName.Text ="Tag"+ arrlist[0].ToString();
            PreTagList.ItemsSource = taglis;
            PreLeftTagList.ItemsSource = taglis;
            PreRightTagList.ItemsSource = taglis;
            NextTagList.ItemsSource = taglis;
            NextLeftTagList.ItemsSource = taglis;
            NextRightTagList.ItemsSource = taglis;
            SpeedLIst.ItemsSource = TagCompile.agvSpeed;
            SpeedRevLis.ItemsSource = TagCompile.agvSpeed;
            StopTime.Text = arrlist[9].ToString();
            Pbslist.ItemsSource = TagCompile.agvPbs;
            PbsRevlist.ItemsSource = TagCompile.agvPbs;
            TagTerminal.Text = arrlist[12].ToString();
            #endregion


            #region 更新值
            PreTagList.Text = arrlist[1].ToString();
            PreLeftTagList.Text = arrlist[2].ToString();
            PreRightTagList.Text = arrlist[3].ToString();
            NextTagList.Text = arrlist[4].ToString();
            NextLeftTagList.Text = arrlist[5].ToString();
            NextRightTagList.Text = arrlist[6].ToString();
            SpeedLIst.Text = arrlist[7].ToString();
            SpeedRevLis.Text = arrlist[8].ToString();
            Pbslist.Text = arrlist[10].ToString();
            PbsRevlist.Text = arrlist[11].ToString();
            #endregion

            GetScroll.ScrollToHorizontalOffset(label.Margin.Left-600);//滚动条X轴跟随移动
            GetScroll.ScrollToVerticalOffset(label.Margin.Top-500); ///滚动条Y轴等随移动

        }


        #region ======编辑提交======
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            present();
            this.Close();
        }

        public void present()
        {
            ((DataRowView)dataGrid.SelectedItem)["PreTag"] = PreTagList.SelectedValue.ToString();

            ((DataRowView)dataGrid.SelectedItem)["PreLeftTag"] = PreLeftTagList.SelectedValue.ToString();

            ((DataRowView)dataGrid.SelectedItem)["PreRightTag"] = PreRightTagList.SelectedValue.ToString();

            ((DataRowView)dataGrid.SelectedItem)["NextTag"] = NextTagList.SelectedValue.ToString();

            ((DataRowView)dataGrid.SelectedItem)["NextLeftTag"] = NextLeftTagList.SelectedValue.ToString();

            ((DataRowView)dataGrid.SelectedItem)["NextRightTag"] = NextRightTagList.SelectedValue.ToString();

            ((DataRowView)dataGrid.SelectedItem)["Speed"] = SpeedLIst.SelectedValue.ToString();

            ((DataRowView)dataGrid.SelectedItem)["SpeedRev"] = SpeedRevLis.SelectedValue.ToString();

            ((DataRowView)dataGrid.SelectedItem)["StopTime"] = StopTime.Text.Trim().ToString();

            ((DataRowView)dataGrid.SelectedItem)["Pbs"] = Pbslist.SelectedValue.ToString();

            ((DataRowView)dataGrid.SelectedItem)["PbsRev"] = PbsRevlist.SelectedValue.ToString();

            ((DataRowView)dataGrid.SelectedItem)["TagTerminal"] = TagTerminal.Text.Trim().ToString();
        }
        #endregion


        #region ====更新选中数据====
        private void DataDrid()
        {
            List<object> Tagarr = new List<object>();
            Tagarr.Add(((DataRowView)dataGrid.SelectedItem)["TagName"].ToString());
            Tagarr.Add(((DataRowView)dataGrid.SelectedItem)["PreTag"].ToString());
            Tagarr.Add(((DataRowView)dataGrid.SelectedItem)["PreLeftTag"].ToString());
            Tagarr.Add(((DataRowView)dataGrid.SelectedItem)["PreRightTag"].ToString());
            Tagarr.Add(((DataRowView)dataGrid.SelectedItem)["NextTag"].ToString());
            Tagarr.Add(((DataRowView)dataGrid.SelectedItem)["NextLeftTag"].ToString());
            Tagarr.Add(((DataRowView)dataGrid.SelectedItem)["NextRightTag"].ToString());
            Tagarr.Add(((DataRowView)dataGrid.SelectedItem)["Speed"].ToString());
            Tagarr.Add(((DataRowView)dataGrid.SelectedItem)["SpeedRev"].ToString());
            Tagarr.Add(((DataRowView)dataGrid.SelectedItem)["StopTime"].ToString());
            Tagarr.Add(((DataRowView)dataGrid.SelectedItem)["Pbs"].ToString());
            Tagarr.Add(((DataRowView)dataGrid.SelectedItem)["PbsRev"].ToString());
            Tagarr.Add(((DataRowView)dataGrid.SelectedItem)["TagTerminal"].ToString());
            arrlist.Clear();
            arrlist = Tagarr;
        }
        #endregion



        /// <summary>
        /// 上一个
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void last_Click(object sender, RoutedEventArgs e)
        {
            present();
            index--;
            if (index < 0)
            {
                index = 0;
                MessageBox.Show("已是第一个");
            }
            dataGrid.SelectedIndex = index;
            DataDrid();
            TagLoad();
        }

       /// <summary>
       /// 下一个
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void next_Click(object sender, RoutedEventArgs e)
        {
            present();
            index++;
            if (index > dataGrid.Items.Count - 1)
            {
                index = dataGrid.Items.Count - 1;
                MessageBox.Show("已是最后一个");
            }
            dataGrid.SelectedIndex = index;
            DataDrid();
            TagLoad();
        }
    }
}
