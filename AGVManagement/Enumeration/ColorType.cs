using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVManagement.Enumeration
{
    public enum Colortype
    {
        FontColor,//字体颜色

        BgColor, //背景颜色

        BrColor, //边框颜色
    }


    public enum CircuitType
    {
        /// <summary>
        /// 直线
        /// </summary>
        Line,

        /// <summary>
        /// 折线
        /// </summary>
        Broken,

        /// <summary>
        /// 半圆
        /// </summary>
        Semicircle,

        /// <summary>
        /// 清除线路
        /// </summary>
        Clear
    }

}
