using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVManagement.Models
{
    /// <summary>
    /// Tag实体类
    /// </summary>
    public class MapTag
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Tag号
        /// </summary>
        public string TagNo { get; set; }
        
        /// <summary>
        /// Tag名称
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// X（米）
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y（米）
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 后置tag
        /// </summary>
        public List<string> NextTag { get; set; }

        /// <summary>
        /// 左后置tag
        /// </summary>
        public List<string> NextLeftTag { get; set; }

        /// <summary>
        /// 右后置tag
        /// </summary>
        public List<string> NextRightTag { get; set; }

        /// <summary>
        /// 前置tag
        /// </summary>
        public List<string> PreTag { get; set; }

        /// <summary>
        /// 左前置tag
        /// </summary>
        public List<string> PreLeftTag { get; set; }

        /// <summary>
        /// 右前置tag
        /// </summary>
        public List<string> PreRightTag { get; set; }

        /// <summary>
        /// 正向速度
        /// </summary>
        public List<string> Speed { get; set; }

        /// <summary>
        /// 反向速度
        /// </summary>
        public List<string> SpeedRev { get; set; }

        /// <summary>
        /// 单位为s
        /// </summary>
        public string StopTime { get; set; }

        /// <summary>
        /// 障碍物扫描（正向）
        /// </summary>
        public List<string> Pbs { get; set; }

        /// <summary>
        /// 障碍物扫描（反向）
        /// </summary>
        public List<string> PbsRev { get; set; }

        /// <summary>
        /// 0为非终结点，1为终结点，2为辅助点
        /// </summary>
        public string TagTerminal { get; set; }


    }
}
