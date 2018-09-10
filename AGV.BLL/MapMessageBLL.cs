using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGV.DAL;
using System.Data;
using MySql.Data.MySqlClient;

namespace AGV.BLL
{
   public class MapMessageBLL
    {
        MapMessageDAL MapMessage = new MapMessageDAL();


        /// <summary>
        /// 启动地图修改
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public bool UpdateSettingMap(long Map, int Mode)
        {
            return MapMessage.UpdateSetting(Map, Mode);
        }

        public bool InsertDeviceMap(long mapTime, DataTable data)
        {
            return MapMessage.InsertDevice(mapTime,data);
        }
        public DataTable LoadDeviceMap(long MapTime)
        {
            return MapMessage.LoadDevice(MapTime);
        }


        /// <summary>
        /// 查询启动地图
        /// </summary>
        /// <returns></returns>
        public string SettingInfoMap()
        {
            return MapMessage.SettingInfo();
        }


        /// <summary>
        /// 删除线路
        /// </summary>
        /// <param name="MapTime"></param>
        /// <param name="Program"></param>
        /// <returns></returns>
        public bool DelRouteMap(long MapTime, int Program)
        {
            return MapMessage.DelRoute(MapTime, Program);
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
        public bool InsertRouteMap(string Program, string RouteName, long CreateTime, long MapTime, string Tag, string Speed, string Stop, string Turn, string Dire, string Pbs, string Hook, string Agv, string ChangeProgram)
        {
            return MapMessage.InsertRoute(Program, RouteName, CreateTime, MapTime, Tag, Speed, Stop, Turn, Dire, Pbs, Hook, Agv, ChangeProgram);
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
        public bool UpdateRouteMap(long MapTime, int Program, string MapName, string TagStr, string SpeedStr, string StopStr, string TurnStr, string DireStr, string PbsStr, string HookStr, string AgvStr, string ProgramStr)
        {
            return MapMessage.UpdateRoute(MapTime, Program, MapName, TagStr, SpeedStr, StopStr, TurnStr, DireStr, PbsStr, HookStr, AgvStr, ProgramStr);
        }

        /// <summary>
        /// 判断线路号是否存在
        /// </summary>
        /// <param name="Program"></param>
        /// <param name="MapTime"></param>
        /// <returns></returns>
        public bool Program(string Program, long MapTime)
        {
            return MapMessage.ExistsProgram(Program,MapTime);
        }

        public DataTable BLLMapRoute(string MapName)
        {
            return MapMessage.MapRoute(MapName);
        }
        public DataTable GetMapData(string MpName)
        {
            return MapMessage.MapList(MpName);
        }

        public DataTable MapParray(string MpName)
        {
            return MapMessage.MapSelect(MpName);
        }

        public bool MapToleadNumber(string sql)
        {
            return MapMessage.MapTolead(sql);
        }

        public bool SaveAGVMapBLL(List<string> sql)
        {
            return MapMessage.SaveAGVMap(sql);
        }
    }
}
