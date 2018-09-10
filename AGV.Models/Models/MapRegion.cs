using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVManagement.Models
{
    /// <summary>
    /// 区域实体类
    /// </summary>
    public class MapRegion
    {
        /// <summary>
        /// 区域名称
        /// </summary>
        public string MapRegionName { get; set; }

        /// <summary>
        /// 字体大小
        /// </summary>
        public double MapFontSise { get; set; }

        /// <summary>
        /// 字体位置
        /// </summary>
        public string MapPosition { get; set; }

        /// <summary>
        /// 字体颜色
        /// </summary>
        public string MapFontColor { get; set; }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public string MapBgColor { get; set; }

        /// <summary>
        /// X轴距离
        /// </summary>
        public double DistanceX { get; set; }

        /// <summary>
        /// Y轴距离
        /// </summary>
        public double DistanceY { get; set; }

        /// <summary>
        /// 区域宽度
        /// </summary>
        public double RegionWidth { get; set; }

        /// <summary>
        /// 区域高度
        /// </summary>
        public double RegionHeight { get; set; }
    }
}
