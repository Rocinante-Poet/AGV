using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGV.DAL
{
    public class MapMessageDAL
    {

        /// <summary>
        /// 启动地图修改
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public bool UpdateSetting(long Map, int Mode)
        {
            string desql = string.Format("DELETE FROM agv.`setting`;");
            string sql = string.Format("INSERT INTO agv.`setting` (`Map`,`Mode`) VALUES({0},{1})", Map, Mode);
            return MySqlHelper.ExecuteNonQuery(desql + sql) > 0 ? true : false;
        }

        /// <summary>
        /// 串口添加修改
        /// </summary>
        /// <param name="mapTime"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertDevice(long mapTime, DataTable data)
        {
            string desql = string.Format("DELETE FROM agv.`device{0}`;", mapTime.ToString());
           
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(desql);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string sql = string.Format("INSERT INTO agv.`device{0}` (`Com`, `Baud`, `Agv`) VALUES ", mapTime.ToString());
                sbSql.Append(sql);
                sbSql.Append('(');
                sbSql.Append(data.Rows[i][0]);
                sbSql.Append(",'");
                sbSql.Append(data.Rows[i][1]);
                sbSql.Append("','");
                sbSql.Append(data.Rows[i][2]);
                sbSql.Append("')");
                sbSql.Append(";");
            }
            return MySqlHelper.ExecuteNonQuery(sbSql.ToString()) > 0 ? true : false;
        }


        public DataTable LoadDevice(long MapTime)
        {
            string sql = string.Format("SELECT `Com`, `Baud`, `Agv` FROM agv.`device{0}`", MapTime);
            return MySqlHelper.ExecuteDataTable(sql);
        }

        /// <summary>
        /// 查询启动地图
        /// </summary>
        /// <returns></returns>
        public string SettingInfo()
        {
            List<string> setList = new List<string>();
            string sql = string.Format("SELECT Map FROM agv.`setting`");
            DataTable dt = MySqlHelper.ExecuteDataTable(sql);
            return dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : null;
        }


        /// <summary>
        /// 判断线路号是否存在
        /// </summary>
        /// <param name="Program"></param>
        /// <param name="MapTime"></param>
        /// <returns></returns>
        public bool ExistsProgram(string Program, long MapTime)
        {
            DataTable dt = MySqlHelper.ExecuteDataTable(string.Format("SELECT `CreateTime` FROM agv.`route{0}` WHERE `Program` = {1}", MapTime, Program));
            return dt.Rows.Count > 0 ? true : false;
        }

        /// <summary>
        /// 删除线路
        /// </summary>
        /// <param name="MapTime"></param>
        /// <param name="Program"></param>
        /// <returns></returns>
        public bool DelRoute(long MapTime, int Program)
        {
            string sql = string.Format("DELETE FROM agv.`route{0}` WHERE `Program` = {1}", MapTime, Program);
            return MySqlHelper.ExecuteNonQuery(sql) > 0 ? true : false;
        }

        /// <summary>
        /// 插入线路
        /// </summary>
        /// <param name="Program"></param>
        /// <param name="RouteName"></param>
        /// <param name="CreateTime"></param>
        /// <param name="MapTime"></param>
        /// <param name="Tag"></param>
        /// <param name="Speed"></param>
        /// <param name="Stop"></param>
        /// <param name="Turn"></param>
        /// <param name="Dire"></param>
        /// <param name="Pbs"></param>
        /// <param name="Hook"></param>
        /// <param name="Agv"></param>
        /// <param name="ChangeProgram"></param>
        /// <returns></returns>
        public bool InsertRoute(string Program, string RouteName, long CreateTime, long MapTime, string Tag, string Speed, string Stop, string Turn, string Dire, string Pbs, string Hook, string Agv, string ChangeProgram)
        {
            string sql = string.Format("INSERT INTO agv.`route{0}` (`Program`, `Name`, `CreateTime`, `Tag`, `Speed`, `Stop`, `Turn`, `Direction`, `Pbs`, `Hook`, `ChangeProgram`, `AGV`)  VALUES ('{1}', '{2}', {3}, '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}') ", MapTime, Program, RouteName, CreateTime, Tag, Speed, Stop, Turn, Dire, Pbs, Hook, ChangeProgram, Agv);
            return MySqlHelper.ExecuteNonQuery(sql) > 0 ? true : false;
        }

        /// <summary>
        /// 更新线路
        /// </summary>
        /// <param name="MapTime"></param>
        /// <param name="Program"></param>
        /// <param name="MapName"></param>
        /// <param name="TagStr"></param>
        /// <param name="SpeedStr"></param>
        /// <param name="StopStr"></param>
        /// <param name="TurnStr"></param>
        /// <param name="DireStr"></param>
        /// <param name="PbsStr"></param>
        /// <param name="HookStr"></param>
        /// <param name="AgvStr"></param>
        /// <param name="ProgramStr"></param>
        /// <param name="RouteTime"></param>
        public bool UpdateRoute(long MapTime, int Program, string MapName, string TagStr, string SpeedStr, string StopStr, string TurnStr, string DireStr, string PbsStr, string HookStr, string AgvStr, string ProgramStr)
        {
            string sql = string.Format("UPDATE agv.`route{0}` SET  `Name` = '{1}', `Tag` = '{2}', `Speed` = '{3}', `Stop` = '{4}', `Turn` = '{5}', `Direction` = '{6}', Pbs = '{7}', Hook = '{8}', `AGV` = '{9}', `ChangeProgram` = '{10}'  WHERE Program = {11}", MapTime, MapName, TagStr, SpeedStr, StopStr, TurnStr, DireStr, PbsStr, HookStr, AgvStr, ProgramStr, Program);
            return MySqlHelper.ExecuteNonQuery(sql) > 0 ? true : false;
        }

        public DataTable MapList(string MpName)
        {
            string Sql = "";
            if (MpName != null)
            {
                Sql = "select * FROM map where Name LIKE  '%" + MpName + "%'";
            }
            else
            {
                Sql = "select * from map";
            }
            DataTable data = MySqlHelper.ExecuteDataTable(Sql);
            return data.Rows.Count > 0 ? data : null;
        }

        public DataTable MapSelect(string MpNAme)
        {
            return MySqlHelper.ExecuteDataTable("select * FROM map where Name='"+ MpNAme + "'");
        }

        public bool MapTolead(string Sql)
        {
            try
            {
                return MySqlHelper.ExecuteSqlTran(new List<string>() { Sql });
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 地图保存
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool SaveAGVMap(List<string> sql)
        {
            return MySqlHelper.ExecuteSqlTran(sql);
        }

        /// <summary>
        /// 线路
        /// </summary>
        /// <returns></returns>
        public DataTable MapRoute(string MapName)
        {
            DataTable dt = MySqlHelper.ExecuteDataTable(string.Format("select * FROM route{0}", MapName));
            return dt;
        }
    }
}
