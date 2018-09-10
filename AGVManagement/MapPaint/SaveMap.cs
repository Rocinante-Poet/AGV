using AGV.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Controls;
using AGVManagement.Enumeration;
using MySql.Data.MySqlClient;

namespace AGVManagement.MapPaint
{
    public class SaveMap
    {
        TagInfoBLL tagInfo = new TagInfoBLL();
        MapMessageBLL map = new MapMessageBLL();
        List<string> Sql = new List<string>();

        /// <summary>
        /// 保存地图信息
        /// </summary>
        /// <param name="Times"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool SaveAtlas(string Times, bool type, string Name, double Width, double Height, string AgvStr, int MapType)
        {
            if (type)
            {
                InsertMapInfo(Name, Times, Width, Height, AgvStr, MapType);
            }
            else
            {
                UpdateMap(Name, AgvStr, Times, MapType);
            }
            SaveTag(Times, type);
            InsertLine(Times);
            InsertArea(Times);
            InsertText(Times);
            return map.SaveAGVMapBLL(Sql);
        }

        /// <summary>
        /// 更新Map信息
        /// </summary>
        /// <param name="MapName"></param>
        /// <param name="AgvStr"></param>
        /// <param name="MapTime"></param>
        /// <param name="MapType"></param>
        public void UpdateMap(string MapName, string AgvStr, string MapTime, int MapType)
        {
            Sql.Add(string.Format("UPDATE agv.`map` SET `Name` = '{0}', `AGV` = '{1}', `Type` = {2} WHERE CreateTime = {3}", MapName, AgvStr, MapType, MapTime));
        }

        /// <summary>
        /// 插入table.map的数据库信息
        /// </summary>
        /// <param name="Conn"></param>
        /// <param name="Name"></param>
        /// <param name="CreateTime">utc时间</param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        public void InsertMapInfo(string Name, string CreateTime, double Width, double Height, string AgvStr, int MapType)
        {
            Sql.Add(string.Format("INSERT INTO agv.`map` (`Name`, `CreateTime`, `Width`, `Height`, `AGV`, `Type`) VALUES ('{0}', {1}, {2}, {3}, '{4}', {5})", Name, CreateTime, Width, Height, AgvStr, MapType));
        }

        /// <summary>
        /// Tag保存
        /// </summary>
        /// <param name="CreateTime"></param>
        /// <param name="type"></param>
        private void SaveTag(string CreateTime, bool type)
        {
            string sqlTag = string.Format("DROP TABLE IF EXISTS agv.`tag{0}`;CREATE TABLE IF NOT EXISTS agv.`tag{0}` ( `ID` int(10) NOT NULL AUTO_INCREMENT, `TagNo` varchar(50) DEFAULT NULL COMMENT 'tag号（地图记录用）', `TagName` varchar(50) DEFAULT NULL COMMENT 'tag名称', `X` double DEFAULT NULL COMMENT 'X（米）', `Y` double DEFAULT NULL COMMENT 'Y（米）', `NextTag` varchar(10) DEFAULT NULL COMMENT '后置tag', `NextLeftTag` varchar(10) DEFAULT NULL COMMENT '左后置tag', `NextRightTag` varchar(10) DEFAULT NULL COMMENT '右后置tag', `PreTag` varchar(10) DEFAULT NULL COMMENT '前置tag', `PreLeftTag` varchar(10) DEFAULT NULL COMMENT '左前置tag', `PreRightTag` varchar(10) DEFAULT NULL COMMENT '右前置tag', `Speed` int(10) DEFAULT NULL COMMENT '正向速度', `SpeedRev` int(10) DEFAULT NULL COMMENT '反向速度', `StopTime` int(10) DEFAULT NULL COMMENT '单位为s', `Pbs` int(10) DEFAULT NULL COMMENT '障碍物扫描（正向）', `PbsRev` int(10) DEFAULT NULL COMMENT '障碍物扫描（反向）', `TagTerminal` VARCHAR(50) DEFAULT NULL COMMENT '0为非终结点，1为终结点，2为辅助点', PRIMARY KEY(`ID`)) ENGINE = InnoDB DEFAULT CHARSET = utf8 COMMENT = '取名为tag+UTC时间，如map1234567890'; ", CreateTime.ToString());
            string sqlWidget = string.Format("DROP TABLE IF EXISTS agv.`widget{0}`;CREATE TABLE IF NOT EXISTS agv.`widget{0}` ( `ID` int(10) NOT NULL AUTO_INCREMENT, `WidgetNo` varchar(50) DEFAULT NULL COMMENT '控件编号', `Name` varchar(50) DEFAULT NULL COMMENT '文字内容', `X` double DEFAULT NULL COMMENT 'X坐标位置（米）', `Y` double DEFAULT NULL COMMENT 'Y坐标位置（米）', `Width` double DEFAULT NULL COMMENT '宽度（米）', `Height` double DEFAULT NULL COMMENT '高度（米）', `FontSize` int(11) DEFAULT NULL COMMENT '字体大小', `FontPosition` varchar(50) DEFAULT NULL COMMENT '字体位置（16进制）', `ForeColor` varchar(50) DEFAULT NULL COMMENT '字体颜色（16进制）', `BackColor` varchar(50) DEFAULT NULL COMMENT '背景颜色（16进制）', `BorderColor` varchar(50) DEFAULT NULL COMMENT '边框颜色（16进制）', PRIMARY KEY(`ID`)) ENGINE = InnoDB DEFAULT CHARSET = utf8; ", CreateTime.ToString());
            string sqlLine = string.Format("DROP TABLE IF EXISTS agv.`line{0}`;CREATE TABLE IF NOT EXISTS agv.`line{0}` ( `ID` int(10) NOT NULL AUTO_INCREMENT, `StartX` double DEFAULT NULL COMMENT '起始X坐标位置（米）', `StartY` double DEFAULT NULL COMMENT '起始Y坐标位置（米）', `EndX` double DEFAULT NULL COMMENT '终点X坐标位置（米）', `EndY` double DEFAULT NULL COMMENT '终点Y坐标位置（米）', `LineStyel` int(11) DEFAULT NULL COMMENT 'Line类型：1为直线，2为折线', `Tag1` varchar(50) DEFAULT NULL, `Tag2` varchar(50) DEFAULT NULL, PRIMARY KEY(`ID`)) ENGINE = InnoDB DEFAULT CHARSET = utf8 ROW_FORMAT = COMPACT; ", CreateTime.ToString());
            string sqlDevice = string.Format("CREATE TABLE IF NOT EXISTS agv.`device{0}` ( `ID` int(10) NOT NULL AUTO_INCREMENT, `Com` int(11) DEFAULT NULL, `Baud` int(11) DEFAULT NULL, `Agv` varchar(50) DEFAULT NULL,  PRIMARY KEY(`ID`)) ENGINE = InnoDB DEFAULT CHARSET = utf8 ROW_FORMAT = COMPACT; ", CreateTime.ToString());
            string sqlRoute = string.Format("CREATE TABLE IF NOT EXISTS agv.`route{0}` ( `ID` int(10) NOT NULL AUTO_INCREMENT, `Program` int(11) DEFAULT NULL COMMENT '线路对应的Program', `Name` varchar(50) DEFAULT NULL COMMENT '线路名称', `CreateTime` bigint(20) NOT NULL DEFAULT '0' COMMENT '线路的创建时间', `Tag` varchar(500) DEFAULT NULL COMMENT 'tag序列', `Speed` varchar(500) DEFAULT NULL COMMENT 'tag速度序列（单位m/min)', `Stop` varchar(500) DEFAULT NULL COMMENT '停止列（单位为s）', `Turn` varchar(500) DEFAULT NULL COMMENT '转弯列，0无动作，1左转，2右转，3取消转弯', `Direction` varchar(500) DEFAULT NULL COMMENT '前进列，后退，0前进，1后退', `Pbs` varchar(500) DEFAULT NULL COMMENT 'Pbs列', `revPbs` varchar(500) DEFAULT NULL COMMENT '反向Pbs列', `Hook` varchar(500) DEFAULT NULL COMMENT 'hook列 0下降，1升起', `ChangeProgram` varchar(500) DEFAULT NULL COMMENT '修改Program', `AGV` varchar(500) DEFAULT NULL COMMENT '线路包括的agv', PRIMARY KEY(`ID`)) ENGINE = InnoDB AUTO_INCREMENT = 7 DEFAULT CHARSET = utf8 ROW_FORMAT = COMPACT; ", CreateTime.ToString());
            DataTable data = null;
            if (!type)
            {
                data = tagInfo.RataTable(CreateTime);
            }
            Sql.Add(sqlTag + sqlWidget + sqlLine + sqlDevice + sqlRoute);
            StringBuilder sqlTagd = new StringBuilder();
            foreach (Label lb in MapInstrument.valuePairs.Values)
            {
                string strTag = string.Format("INSERT INTO agv.`tag{0}` (`TagNo`, `TagName`, `X`, `Y`, `NextTag`, `NextLeftTag`, `NextRightTag`, `PreTag`, `PreLeftTag`, `PreRightTag`, `Speed`, `SpeedRev`, `StopTime`, `Pbs`, `PbsRev`, `TagTerminal`) VALUES ", CreateTime.ToString());
                sqlTagd.Append(strTag);
                sqlTagd.Append("('");
                sqlTagd.Append("TA" + lb.Tag);
                sqlTagd.Append("','");
                sqlTagd.Append(lb.Tag);
                sqlTagd.Append("',");
                sqlTagd.Append((((lb.Margin.Left + 19) / Painting.siseWin) /10));
                sqlTagd.Append(",");
                sqlTagd.Append((((lb.Margin.Top + 11.5) / Painting.siseWin) / 10));
                for (int i = 0; i < 6; i++)
                {
                    sqlTagd.Append(",'N/A'");
                }
                for (int i = 0; i < 6; i++)
                {
                    sqlTagd.Append(",0");
                }
                sqlTagd.Append(")");
                sqlTagd.Append(";");
            }
            Sql.Add(sqlTagd.ToString());
            if (data != null)
            {
                foreach (DataRow Dt in data.Rows)
                {
                    string sql = string.Format("UPDATE agv.`tag{0}` SET `NextTag` = '{1}',`NextLeftTag` = '{2}', `NextRightTag` = '{3}',`PreTag` = '{4}',`PreLeftTag` = '{5}',`PreRightTag` = '{6}',`Speed` = '{7}',`SpeedRev` = '{8}',`StopTime` = '{9}',`Pbs` = '{10}',`PbsRev` = '{11}', `TagTerminal` = '{12}' WHERE TagName = '{13}';\n", CreateTime, Dt[5].ToString(), Dt[6].ToString(), Dt[7].ToString(), Dt[8].ToString(), Dt[9].ToString(), Dt[10].ToString(), Dt[11].ToString(), Dt[12].ToString(), Dt[13].ToString(), Dt[14].ToString(), Dt[15].ToString(), Dt[16].ToString(), Dt[2].ToString());
                    Sql.Add(sql);
                }
            }
        }
        AreaCompile area = new AreaCompile();

        /// <summary>
        /// 区域保存
        /// </summary>
        /// <param name="CreateTime"></param>
        public void InsertArea(string CreateTime)
        {
            StringBuilder sqlArea = new StringBuilder();
            foreach (Label lb in MapInstrument.keyValuePairs.Values)
            {
                string strArea = string.Format("INSERT INTO agv.`widget{0}` (`WidgetNo`, `Name`, `FontSize`,`ForeColor`, `X`, `Y`, `Width`, `Height`,`BackColor`,`BorderColor` , `FontPosition`) VALUES ", CreateTime.ToString());
                sqlArea.Append(strArea);
                sqlArea.Append("('");
                sqlArea.Append("AR" + lb.Tag);
                sqlArea.Append("','");
                sqlArea.Append(lb.Content);
                sqlArea.Append("',");
                sqlArea.Append((lb.FontSize) / Painting.siseWin);
                sqlArea.Append(",'");
                sqlArea.Append(lb.Foreground.ToString().Substring(1));
                sqlArea.Append("',");
                sqlArea.Append((lb.Margin.Left/ Painting.siseWin) / 10);
                sqlArea.Append(",");
                sqlArea.Append((lb.Margin.Top/ Painting.siseWin) / 10);
                sqlArea.Append(",");
                sqlArea.Append((lb.Width / Painting.siseWin) /10);
                sqlArea.Append(",");
                sqlArea.Append((lb.Height/ Painting.siseWin) /10 );
                sqlArea.Append(",'");
                sqlArea.Append(lb.Background.ToString().Substring(1));
                sqlArea.Append("','");
                sqlArea.Append(lb.BorderBrush.ToString().Substring(1));
                sqlArea.Append("','");
                sqlArea.Append(area.aAlignment(lb));
                sqlArea.Append("')");
                sqlArea.Append(";");
            }
            Sql.Add(sqlArea.ToString());
        }


        /// <summary>
        /// 文字保存
        /// </summary>
        /// <param name="CreateTime"></param>
        public void InsertText(string CreateTime)
        {
            StringBuilder sqlText = new StringBuilder();
            foreach (Label lb in MapInstrument.GetKeyValues.Values)
            {
                string strText = string.Format("INSERT INTO agv.`widget{0}` (`WidgetNo`, `Name`, `FontSize`,`ForeColor`, `X`, `Y`) VALUES ", CreateTime.ToString());
                sqlText.Append(strText);
                sqlText.Append("('");
                sqlText.Append("TE" + lb.Tag);
                sqlText.Append("','");
                sqlText.Append(lb.Content);
                sqlText.Append("'");
                sqlText.Append(",");
                sqlText.Append((lb.FontSize) / Painting.siseWin);
                sqlText.Append(",'");
                sqlText.Append(lb.Foreground.ToString().Substring(1));
                sqlText.Append("',");
                sqlText.Append((lb.Margin.Left/ Painting.siseWin) / 10);
                sqlText.Append(",");
                sqlText.Append((lb.Margin.Top/ Painting.siseWin) /10);
                sqlText.Append(")");
                sqlText.Append(";");
            }
            Sql.Add(sqlText.ToString());
        }

        /// <summary>
        /// 线路保存
        /// </summary>
        /// <param name="CreateTime"></param>
        public void InsertLine(string CreateTime)
        {
            StringBuilder sqlLine = new StringBuilder();
            foreach (WirePointArray point in MapInstrument.wirePointArrays)
            {
                string strLine = string.Format("INSERT INTO agv.`line{0}` (`StartX`, `StartY`, `EndX`, `EndY`, `LineStyel` , `Tag1`, `Tag2`) VALUES ", CreateTime.ToString());
                sqlLine.Append(strLine);
                sqlLine.Append("('");
                sqlLine.Append((point.GetPoint.SetPoint.X / 10) / Painting.siseWin);
                sqlLine.Append("','");
                sqlLine.Append((point.GetPoint.SetPoint.Y / 10) / Painting.siseWin);
                sqlLine.Append("','");
                sqlLine.Append((point.GetWirePoint.SetPoint.X / 10) / Painting.siseWin);
                sqlLine.Append("','");
                sqlLine.Append((point.GetWirePoint.SetPoint.Y / 10) / Painting.siseWin);
                sqlLine.Append("','");
                if (point.circuitType.Equals(CircuitType.Line))
                {
                    sqlLine.Append(1);
                }
                else if (point.circuitType.Equals(CircuitType.Broken))
                {
                    sqlLine.Append(2);
                }
                else
                {
                    sqlLine.Append(3);
                }
                sqlLine.Append("','");
                sqlLine.Append("TA" + point.GetPoint.TagID);
                sqlLine.Append("','");
                sqlLine.Append("TA" + point.GetWirePoint.TagID);
                sqlLine.Append("')");
                sqlLine.Append(";");
            }
            Sql.Add(sqlLine.ToString());
        }








    }
}
