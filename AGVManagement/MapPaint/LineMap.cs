using AGV.BLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AGVManagement.MapPaint
{
    public class LineMap
    {
        MapInstrument map = new MapInstrument();
        public string[] GetTags = null;

        /// <summary>
        /// 查找关联Tag
        /// </summary>
        /// <param name="Time"></param>
        /// <param name="TagNu"></param>
        public void TagClick(long Time, int TagNu,DataGrid grid,DataTable dt,bool type)
        {
            bool exists = false;
            if (GetTags!=null)
            {
                foreach (string item in GetTags)
                {
                    if (!item.Equals("N/A"))//排除第一个空值(N/A)
                    {
                        if (Convert.ToInt32(item).Equals(TagNu))
                        {
                            exists = true;
                        }
                    }
                }
            }
            if (exists|| GetTags==null)
            {
                map.TagFormer();//所有Tag还原为原色
                OperateDBBLL operate = new OperateDBBLL();
                string[] taglis = operate.SelectTagArr(Time, TagNu.ToString());//查询关联Tag
                GetTags = taglis;
                AddTagInfo(grid, dt, TagNu, taglis, type);
            }
        }
       
        /// <summary>
        /// 添加Tag信息
        /// </summary>
        public void AddTagInfo(DataGrid gid,DataTable table,int TagNu, string[] taglis,bool type)
        {
            if (type)
            {
                table.Rows.Add(new object[] { TagNu, TagCompile.agvSpeed[10], TagCompile.agvPbs[16], TagCompile.agvTurn[0], TagCompile.agvDire[2], TagCompile.agvHook[2], "0", "999" });
            }
            gid.ItemsSource = table.DefaultView;
            gid.AutoGenerateColumns = false;
            for (int i = 0; i < gid.Items.Count; i++)
            {
                foreach (WirePointArray item in MapInstrument.wirePointArrays)
                {
                    if (i < gid.Items.Count - 1)
                    {
                        if ((item.GetPoint.TagID.Equals(Convert.ToInt32(((DataRowView)gid.Items[i])[0])) && item.GetWirePoint.TagID.Equals(Convert.ToInt32(((DataRowView)gid.Items[i+1])[0]))) || (item.GetPoint.TagID.Equals(Convert.ToInt32(((DataRowView)gid.Items[i+1])[0]))) && item.GetWirePoint.TagID.Equals(Convert.ToInt32(((DataRowView)gid.Items[i])[0])))
                        {
                            Path path = item.GetPath;
                            if (path != null)
                            {
                                path.Stroke = Brushes.Red;
                                path.StrokeThickness = 10;
                                item.GetPath = path;
                            }
                            List<Path> paths = item.Paths;
                            if (paths != null)
                            {
                                for (int s = 0; s < paths.Count; s++)
                                {
                                    Path ph = item.Paths[s];
                                    ph.Stroke = Brushes.Red;
                                    ph.StrokeThickness = 10;
                                    item.Paths[s] = ph;
                                }
                                Point startPt =new Point() { X = MapInstrument.valuePairs[Convert.ToInt32(Convert.ToInt32(((DataRowView)gid.Items[i])[0]))].Margin.Left-19, Y= MapInstrument.valuePairs[Convert.ToInt32(Convert.ToInt32(((DataRowView)gid.Items[i])[0]))].Margin.Left - 11.5 };
                                Point endPt = new Point() { X = MapInstrument.valuePairs[Convert.ToInt32(Convert.ToInt32(((DataRowView)gid.Items[i+1])[0]))].Margin.Left - 19, Y = MapInstrument.valuePairs[Convert.ToInt32(Convert.ToInt32(((DataRowView)gid.Items[i+1])[0]))].Margin.Left - 11.5 };
                                double drn = startPt.X - endPt.X;
                                double hrn = startPt.Y - endPt.Y;


                                //if (drn < 0 && hrn>0)
                                //{
                                //    ((DataRowView)gid.Items[i])["Turn"] = "左转";
                                //}
                                //else if (drn < 0 && hrn>0)
                                //{
                                //    ((DataRowView)gid.Items[i])["Turn"] = "右转";
                                //}

                            }
                        }
                    }
                }
                MapInstrument.valuePairs[Convert.ToInt32(Convert.ToInt32(((DataRowView)gid.Items[i])[0]))].Background = new SolidColorBrush(Colors.Purple);//线路Tag标记为紫色
            }

            MapInstrument.valuePairs[TagNu].Background = new SolidColorBrush(Colors.Red);//选中Tag标记为红色
            for (int i = 0; i < taglis.Length; i++)//关联Tag标记绿色
            {
                if (!i.Equals(0))//排除第一个空值(N/A)
                {
                    MapInstrument.valuePairs[Convert.ToInt32(taglis[i].ToString())].Background = new SolidColorBrush(Colors.Green);//关联Tag标记为绿色

                }
            }
        }






    }
}
