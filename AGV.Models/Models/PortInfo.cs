using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGV.Models.Models
{
    public class PortInfo
    {
        //AGV
        public static List<int> AGVCom = new List<int>();
        public static List<int> Baud = new List<int>();
        public static List<string> agv = new List<string>();

        //PLC
        //public static List<int> plcCom = new List<int>();
        //public static List<int> plcBaud = new List<int>();
        //public static List<string> plcStr = new List<string>();

        //按钮
        public static List<SerialPort> buttonPort = new List<SerialPort>();
        public static List<int> buttonCom = new List<int>();
        public static List<int> buttonBaud = new List<int>();
        public static List<string> buttonStr = new List<string>();

        //充电机
        public static List<SerialPort> chargePort = new List<SerialPort>();
        public static List<int> chargeCom = new List<int>();
        public static List<int> chargeBaud = new List<int>();
        public static List<string> chargeStr = new List<string>();
    }
}
