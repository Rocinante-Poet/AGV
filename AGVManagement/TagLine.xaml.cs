using AGV.BLL;
using AGVManagement.MapPaint;
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
    /// TagLine.xaml 的交互逻辑
    /// </summary>
    public partial class TagLine : Window
    {
        private DataGrid Getgrid;
        private int TagN;
        private int GrIndex;
        private DataTable GetData;
        private string[] taglis;
        private LineMap lineMapMs;
        public long TimeMp;
        public TagLine(List<object> list, long Time, bool type, int TagNum, DataGrid grid, int Indx, DataTable data, LineMap Mp)
        {
            InitializeComponent();
            Getgrid = grid;
            TagN = TagNum;
            GrIndex = Indx;
            GetData = data;
            lineMapMs = Mp;
            TimeMp = Time;
            LoadLineTag(list, Time, type, TagNum);
        }

        public void LoadLineTag(List<object> lst, long Times, bool typ, int TagUnm)
        {
            OperateDBBLL operate = new OperateDBBLL();
            if (!typ)
            {
                taglis = operate.SelectTagArr(Times, TagUnm.ToString()).Where(x => x != "N/A").ToArray();//查询关联Tag
                TagNum.ItemsSource = taglis;
                TagNum.Text = lst[0].ToString();
            }
            else
            {
                List<string> ls = new List<string>();
                foreach (int item in MapInstrument.valuePairs.Keys)
                {
                    ls.Add(item.ToString());
                }
                TagNum.ItemsSource = ls;
                TagNum.Text = lst[0].ToString();
            }
            speed.ItemsSource = TagCompile.agvSpeed;
            PBS.ItemsSource = TagCompile.agvPbs;
            Turn.ItemsSource = TagCompile.agvTurn;
            Direction.ItemsSource = TagCompile.agvDire;
            Hook.ItemsSource = TagCompile.agvHook;
            Time.ItemsSource = TagCompile.agvTime;


            speed.Text = lst[1].ToString();
            PBS.Text = lst[2].ToString();
            Turn.Text = lst[3].ToString();
            Direction.Text = lst[4].ToString();
            Hook.Text = lst[5].ToString();
            Time.Text = lst[6].ToString();
            ChangeProgram.Text = lst[7].ToString();

        }


        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {

            ((DataRowView)Getgrid.Items[GrIndex])["Speed"] = speed.SelectedValue.ToString();
            ((DataRowView)Getgrid.Items[GrIndex])["Pbs"] = PBS.SelectedValue.ToString();
            ((DataRowView)Getgrid.Items[GrIndex])["Turn"] = Turn.SelectedValue.ToString();
            ((DataRowView)Getgrid.Items[GrIndex])["Direction"] = Direction.SelectedValue.ToString();
            ((DataRowView)Getgrid.Items[GrIndex])["Hook"] = Hook.SelectedValue.ToString();
            ((DataRowView)Getgrid.Items[GrIndex])["Stop"] = Time.SelectedValue.ToString();
            ((DataRowView)Getgrid.Items[GrIndex])["ChangeProgram"] = ChangeProgram.Text.ToString();
            if (Turn.SelectedValue.ToString().Equals("左转") || Turn.SelectedValue.ToString().Equals("右转"))
            {
                if (GrIndex != Getgrid.Items.Count - 1)
                {
                    ((DataRowView)Getgrid.Items[GrIndex + 1])["Turn"] = "取消转弯";
                }
            }
            if (((DataRowView)Getgrid.Items[GrIndex])["Tag"].ToString() != (TagNum.SelectedValue.ToString()))
            {
                if (taglis != null)
                {
                    if (GrIndex != Getgrid.Items.Count)
                    {
                        int a = Getgrid.Items.Count;

                        for (int i = 0; i < a; i++)
                        {
                            if (i > GrIndex)
                            {
                                GetData.Rows.Remove(GetData.Rows[GrIndex + 1]);
                            }
                        }
                    }
                }
                else
                {
                    int a = Getgrid.Items.Count;
                    for (int i = 0; i < a; i++)
                    {
                        if (i != 0)
                        {
                            GetData.Rows[0].Delete();
                        }

                    }
                }
               ((DataRowView)Getgrid.Items[GrIndex])["Tag"] = TagNum.SelectedValue.ToString();
                Getgrid.ItemsSource = GetData.DefaultView;
                Getgrid.AutoGenerateColumns = false;
                LineRest();
                lineMapMs.GetTags = null;
                lineMapMs.TagClick(TimeMp, Convert.ToInt32(((DataRowView)Getgrid.Items[GrIndex])["Tag"]), Getgrid, GetData, false);
            }
            ((DataRowView)Getgrid.Items[GrIndex])["Tag"] = TagNum.SelectedValue.ToString();
            Getgrid.SelectedIndex = GrIndex;
            this.Close();
        }

        /// <summary>
        /// 线路还原
        /// </summary>
        public void LineRest()
        {
            MapInstrument map = new MapInstrument();
            map.TagFormer();//所有Tag还原为原色
            foreach (var item in MapInstrument.wirePointArrays)
            {
                if (item.GetPath != null)
                {
                    item.GetPath.Stroke = Brushes.Black;
                    item.GetPath.StrokeThickness = 1;
                }
                List<Path> paths = item.Paths;
                if (paths != null)
                {
                    foreach (Path it in paths)
                    {
                        it.Stroke = Brushes.Black;
                        it.StrokeThickness = 1;
                    }
                }
            }
        }
    }
}
