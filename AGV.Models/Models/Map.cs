using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGV.Models.Models
{
    public class Map
    {
        /// <summary>
        /// ID
        /// </summary>
        public static int id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public static string createname { get; set; }

        /// <summary>
        /// 地图名称
        /// </summary>
        public static string name { get; set; }

        /// <summary>
        /// 地图宽度（米）
        /// </summary>
        public static double width { get; set; }

        /// <summary>
        /// 地图长度（米）
        /// </summary>
        public static double height { get; set; }

        /// <summary>
        /// 地图上注册的AGV
        /// </summary>
        public static string agv { get; set; }

        /// <summary>
        /// 地图类型，磁标，RFID，激光 0磁标，1RFID，2激光
        /// </summary>
        public static int type { get; set; }

    }
}
