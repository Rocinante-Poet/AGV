using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVManagement.MapPaint
{
    public class Error
    {

        public static string errorStr(int errorCode)
        {
            string returnErrorStr = "";
            switch (errorCode)
            {
                case 1:
                    returnErrorStr = "软件版本不一致";
                    break;
                case 2:
                    returnErrorStr = "RAM数据异常";
                    break;
                case 3:
                    returnErrorStr = "电源OFF异常";
                    break;
                case 4:
                    returnErrorStr = "CP短路异常";
                    break;
                case 5:
                    returnErrorStr = "电压异常停止";
                    break;
                case 6:
                    returnErrorStr = "电压下降警报！请更换AGV电池或者对AGV进行充电";
                    break;
                case 7:
                    returnErrorStr = "充电异常";
                    break;
                case 8:
                    returnErrorStr = "前升降单元动作异常";
                    break;
                case 9:
                    returnErrorStr = "后升降单元动作异常";
                    break;
                case 10:
                    returnErrorStr = "前右驱动马达基板异常";
                    break;
                case 11:
                    returnErrorStr = "前左驱动马达基板异常";
                    break;
                case 12:
                    returnErrorStr = "后右驱动马达基板异常";
                    break;
                case 13:
                    returnErrorStr = "后左驱动马达基板异常";
                    break;
                case 14:
                    returnErrorStr = "升降单元动作异常";
                    break;
                case 15:
                    returnErrorStr = "前升降单元线束断线";
                    break;
                case 16:
                    returnErrorStr = "后升降单元线束断线";
                    break;
                case 17:
                    returnErrorStr = "驱动马达中央不良";
                    break;
                case 18:
                    returnErrorStr = "前驱动马达中央不良";
                    break;
                case 19:
                    returnErrorStr = "后驱动马达中央不良";
                    break;
                case 20:
                    returnErrorStr = "障碍物侦测，请移开阻挡物";
                    break;
                case 21:
                    returnErrorStr = "防护区域停止";
                    break;
                case 22:
                    returnErrorStr = "前障碍物传感器故障";
                    break;
                case 23:
                    returnErrorStr = "后障碍物传感器故障";
                    break;
                case 24:
                    returnErrorStr = "右障碍物传感器故障";
                    break;
                case 25:
                    returnErrorStr = "左障碍物传感器故障";
                    break;
                case 26:
                    returnErrorStr = "主障碍物传感器故障";
                    break;
                case 27:
                    returnErrorStr = "副障碍物传感器故障";
                    break;
                case 28:
                    returnErrorStr = "紧急停止";
                    break;
                case 29:
                    returnErrorStr = "缓冲开关碰撞 (行走时 )";
                    break;
                case 30:
                    returnErrorStr = "缓冲开关碰撞 (停止时)";
                    break;
                case 31:
                    returnErrorStr = "胡须传感器碰撞(行走时)1.请移走障碍物 2.按下AGV上的Reset按钮 3.按下AGV上的Start按钮（如果MasterOn的按钮灯未亮，请先按下MasterOn按钮）";
                    break;
                case 32:
                    returnErrorStr = "胡须传感器碰撞(停止时)";
                    break;
                case 33:
                    returnErrorStr = "脱轨 1.按下AGV上的Reset按钮 2.按下DriveUp按钮（如果MasterOn的按钮灯未亮，请先按下MasterOn按钮） 3.把AGV移回轨道 4.按下DriveDown按钮 5.按下AGV上的Start按钮";
                    break;
                case 34:
                    returnErrorStr = "路径检知失败";
                    break;
                case 35:
                    returnErrorStr = "单侧驱动单元的停止异常";
                    break;
                case 36:
                    returnErrorStr = "重复读取异常停止";
                    break;
                case 37:
                    returnErrorStr = "重复读取警告";
                    break;
                case 38:
                    returnErrorStr = "安全继电器异常";
                    break;
                case 39:
                    returnErrorStr = "数据异常，电源重新输入";
                    break;
                case 40:
                    returnErrorStr = "外部紧急停止启动";
                    break;
                case 41:
                    returnErrorStr = "追尾传感器异常";
                    break;
                case 42:
                    returnErrorStr = "位置确定装置1动作异常";
                    break;
                case 43:
                    returnErrorStr = "位置确定装置2动作异常";
                    break;
                case 44:
                    returnErrorStr = "位置确定装置3动作异常	";
                    break;
                case 45:
                    returnErrorStr = "位置确定装置4动作异常	";
                    break;
                case 46:
                    returnErrorStr = "位置确定装置1连线断线异常	";
                    break;
                case 47:
                    returnErrorStr = "位置确定装置2连线断线异常	";
                    break;
                case 48:
                    returnErrorStr = "位置确定装置3连线断线异常	";
                    break;
                case 49:
                    returnErrorStr = "位置确定装置4连线断线异常	";
                    break;
                case 50:
                    returnErrorStr = "地址不一致";
                    break;
                case 51:
                    returnErrorStr = "输入指令设定范围外异常";
                    break;
                case 52:
                    returnErrorStr = "请确认交叉点优先度";
                    break;
                case 53:
                    returnErrorStr = "输入指令异常";
                    break;
                case 54:
                    returnErrorStr = "横向回旋时前后进切换异常";
                    break;
                case 55:
                    returnErrorStr = "距离计数校正异常";
                    break;
                case 56:
                    returnErrorStr = "设定速度警报";
                    break;
                case 57:
                    returnErrorStr = "障碍物区域速度设定警告";
                    break;
                case 60:
                    returnErrorStr = "挂钩动作异常";
                    break;
                case 61:
                    returnErrorStr = "前挂钩连线短路";
                    break;
                case 62:
                    returnErrorStr = "后挂钩连线短路";
                    break;
                case 63:
                    returnErrorStr = "在位检知异常";
                    break;
                //case 63: returnErrorStr = "	挂钩有无报警	";
                //    break;
                case 64:
                    returnErrorStr = "挂钩到位异常";
                    break;
                case 65:
                    returnErrorStr = "牵引失败";
                    break;
                case 66:
                    returnErrorStr = "台车结合不良";
                    break;
                case 67:
                    returnErrorStr = "右挂钩连线异常";
                    break;
                case 68:
                    returnErrorStr = "左挂钩连线异常";
                    break;
                case 70:
                    returnErrorStr = "光通信异常";
                    break;
                case 71:
                    returnErrorStr = "电路板间连接不良";
                    break;
                //case 72: returnErrorStr = "	无线模块异常	";
                //    break;
                case 72:
                    returnErrorStr = "无线局域网通讯异常";
                    break;
                case 73:
                    returnErrorStr = "无线模块通信异常";
                    break;
                case 76:
                    returnErrorStr = "前挂钩动作异常";
                    break;
                case 77:
                    returnErrorStr = "后挂钩动作异常";
                    break;
                case 78:
                    returnErrorStr = "右挂钩动作异常";
                    break;
                case 79:
                    returnErrorStr = "左挂钩动作异常";
                    break;
                case 80:
                    returnErrorStr = "无线clock异常";
                    break;
                case 81:
                    returnErrorStr = "无线启动警报";
                    break;
                case 82:
                    returnErrorStr = "线外未起动警告(无线起动信号未接受)";
                    break;
                case 83:
                    returnErrorStr = "编程切换异常";
                    break;
                case 84:
                    returnErrorStr = "step切换异常";
                    break;
                case 85:
                    returnErrorStr = "速度切换异常";
                    break;
                case 86:
                    returnErrorStr = "完整性控制交叉点异常";
                    break;
                case 90:
                    returnErrorStr = "其它计数启动中";
                    break;
                case 91:
                    returnErrorStr = "目的地指示警告";
                    break;
                case 92:
                    returnErrorStr = "Step修正";
                    break;
                case 94:
                    returnErrorStr = "挡板7LS连线断线异常";
                    break;
                case 95:
                    returnErrorStr = "挡板8LS连线断线异常";
                    break;
                case 96:
                    returnErrorStr = "挡板5故障";
                    break;
                case 97:
                    returnErrorStr = "挡板6故障";
                    break;
                case 98:
                    returnErrorStr = "挡板7故障";
                    break;
                case 99:
                    returnErrorStr = "挡板8故障";
                    break;
                case 100:
                    returnErrorStr = "自动运行切换异常";
                    break;
                case 101:
                    returnErrorStr = "挡板1LS连线断线异常";
                    break;
                //case 95: returnErrorStr = "	挡板异常";
                //    break;
                case 102:
                    returnErrorStr = "挡板2LS连线断线异常";
                    break;
                case 103:
                    returnErrorStr = "挡板3LS连线断线异常";
                    break;
                case 104:
                    returnErrorStr = "挡板4LS连线断线异常";
                    break;
                case 106:
                    returnErrorStr = "挡板1故障";
                    break;
                case 107:
                    returnErrorStr = "挡板2故障";
                    break;
                case 108:
                    returnErrorStr = "挡板3故障";
                    break;
                case 109:
                    returnErrorStr = "挡板4故障";
                    break;
                case 110:
                    returnErrorStr = "传送带马达驱动基板异常";
                    break;
                case 111:
                    returnErrorStr = "传送带异常";
                    break;
                case 112:
                    returnErrorStr = "导向装置安装异常";
                    break;
                case 113:
                    returnErrorStr = "压力传感器异常";
                    break;
                case 114:
                    returnErrorStr = "前传送带异常";
                    break;
                case 115:
                    returnErrorStr = "后传送带异常	";
                    break;
                case 116:
                    returnErrorStr = "传送带1异常";
                    break;
                //case 117: returnErrorStr = "	升降机位置不确定	";
                //    break;
                case 117:
                    returnErrorStr = "升降机LS连线断线异常";
                    break;
                //case 112: returnErrorStr = "	传送带2异常	";
                //    break;
                case 118:
                    returnErrorStr = "升降机动作异常";
                    break;
                //case 114: returnErrorStr = "	传送带3异常	";
                //    break;
                case 119:
                    returnErrorStr = "传送带4异常";
                    break;
                case 123:
                    returnErrorStr = "传送带动作异常";
                    break;
                case 124:
                    returnErrorStr = "传送带在位异常";
                    break;
                case 125:
                    returnErrorStr = "停止位置不良";
                    break;
                case 126:
                    returnErrorStr = "挡板5LS连线断线异常";
                    break;
                case 127:
                    returnErrorStr = "挡板6LS连线断线异常";
                    break;
                case 128:
                    returnErrorStr = "预期外的错误讯息";
                    break;
                case 205:
                    returnErrorStr = "离线";
                    break;
                case 300:
                    returnErrorStr = "偏离预定线路";
                    break;
                default:
                    returnErrorStr = "";
                    break;
            }
            return returnErrorStr;
        }


    }
}
