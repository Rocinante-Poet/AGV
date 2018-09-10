using AGV.BLL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AGVManagement.MapPaint
{
   public class TagCompile
    {
        public static string[] agvSpeed = { "0: 5", "1: 10", "2: 15", "3: 20", "4: 25", "5: 30", "6: 35", "7: 40", "8: 50", "9: 30", "无变化" };
        public static string[] agvPbs = { "区域0", "区域1", "区域2", "区域3", "区域4", "区域5", "区域6", "区域7", "区域8", "区域9", "区域10", "区域11", "区域12", "区域13", "区域14", "区域15", "无变化" };
        public static string[] agvDire = { "前进", "后退", "保持" };
        public static string[] agvTurn = { "无转弯", "左转", "右转", "取消转弯" };
        public static string[] agvHook = { "下降", "升起", "保持" };
        public static string[] agvTime = { "0", "-99" };



        /// <summary>
        /// 显示所有信标数据
        /// </summary>
        /// <param name="Beacon"></param>
        /// <param name="arr"></param>
        public void TagManagement(DataGrid Beacon,string arr)
        {
            Thread thread = new Thread(() => {
                TagInfoBLL tagInfo = new TagInfoBLL();
                MySqlDataReader data = tagInfo.RataTable(long.Parse(arr));
                DataTable table = new DataTable();
                table.Columns.Add(new DataColumn("TagName"));
                table.Columns.Add(new DataColumn("PreTag"));
                table.Columns.Add(new DataColumn("PreLeftTag"));
                table.Columns.Add(new DataColumn("PreRightTag"));
                table.Columns.Add(new DataColumn("NextTag"));
                table.Columns.Add(new DataColumn("NextLeftTag"));
                table.Columns.Add(new DataColumn("NextRightTag"));
                table.Columns.Add(new DataColumn("Speed"));
                table.Columns.Add(new DataColumn("SpeedRev"));
                table.Columns.Add(new DataColumn("StopTime"));
                table.Columns.Add(new DataColumn("Pbs"));
                table.Columns.Add(new DataColumn("PbsRev"));
                table.Columns.Add(new DataColumn("TagTerminal"));
                DataRow dr;
                while (data.Read())
                {
                    dr = table.NewRow();
                    dr["TagName"] = data["TagName"].ToString();
                    dr["PreTag"] = data["PreTag"].ToString();
                    dr["PreLeftTag"] = data["PreLeftTag"].ToString();
                    dr["PreRightTag"] = data["PreRightTag"].ToString();
                    dr["NextTag"] = data["NextTag"].ToString();
                    dr["NextLeftTag"] = data["NextLeftTag"].ToString();
                    dr["NextRightTag"] = data["NextRightTag"].ToString();
                    dr["Speed"] = agvSpeed[Convert.ToInt32(data["Speed"].ToString())];
                    dr["SpeedRev"] = agvSpeed[Convert.ToInt32(data["SpeedRev"].ToString())];
                    dr["StopTime"] = data["StopTime"].ToString();
                    dr["Pbs"] = agvPbs[Convert.ToInt32(data["Pbs"].ToString())];
                    dr["PbsRev"] = agvPbs[Convert.ToInt32(data["PbsRev"].ToString())];
                    dr["TagTerminal"] = data["TagTerminal"].ToString();
                    table.Rows.Add(dr);
                }
                data.Close();
                Beacon.Dispatcher.BeginInvoke(new Action(() => {
                    Beacon.ItemsSource = table.DefaultView;
                    Beacon.AutoGenerateColumns = false;
                }));
            });
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 查找速度索引
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public int agvSpeedIndex(string Text)
        {
            for (int i = 0; i < agvSpeed.Length; i++)
            {
                if (agvSpeed[i].Equals(Text))
                {
                    return i;
                }    
            }

            return 0;
        }


        /// <summary>
        /// 查找方向索引
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public int agvDireIndex(string Text)
        {
            for (int i = 0; i < agvDire.Length; i++)
            {
                if (agvDire[i].Equals(Text))
                {
                    return i;
                }
            }
            return 0;
        }


        /// <summary>
        /// 查找转向索引
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public int agvTurnIndex(string Text)
        {
            for (int i = 0; i < agvTurn.Length; i++)
            {
                if (agvTurn[i].Equals(Text))
                {
                    return i;
                }
            }
            return 0;
        }


        /// <summary>
        /// 查找Hook索引
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public int agvHookIndex(string Text)
        {
            for (int i = 0; i < agvHook.Length; i++)
            {
                if (agvHook[i].Equals(Text))
                {
                    return i;
                }
            }
            return 0;
        }


        /// <summary>
        /// 查找PBS索引
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public int agvPbsIndex(string Text)
        {
            for (int i = 0; i < agvPbs.Length; i++)
            {
                if (agvPbs[i].Equals(Text))
                {
                    return i;
                }
            }
            return 0;
        }

    }
}
