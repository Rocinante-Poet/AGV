using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace AGVManagement.instrument
{
    public class OperateDB
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["MySQLconn"].ConnectionString;
        public string ConficString = Confic();

        public static string Confic()
        {
            string[] ConficString = ConfigurationManager.ConnectionStrings["MySQLconn"].ConnectionString.ToString().Split(';');
            return ConficString[0] + ";" + ConficString[1] + ";" + ConficString[2] + ";";
        }


        public List<string> AgvNumList(long MapTime)
        {
            List<string> agvNumList = new List<string>();
            string sql = string.Format("SELECT `AGV` FROM agv.device{0}", MapTime);
            MySqlDataReader mr = AGV.DAL.MySqlHelper.ExecuteReader(sql);
            while (mr.Read())
            {
                string agvStr = (mr.IsDBNull(0) ? "" : mr.GetString(0));
                if (!string.IsNullOrEmpty(agvStr) && agvStr != "Charge" && agvStr != "Button")
                {
                    string[] agvArray = agvStr.Split(',');
                    for (int i = 0; i < agvArray.Length; i++)
                    {
                        agvNumList.Add(agvArray[i]);
                    }
                }
            }
            mr.Close();
            return agvNumList;
        }



        /// <summary>
        /// 初始化数据库连接
        /// </summary>
        public void CreateDB()
        {
            try
            {
                string DB = string.Format("CREATE DATABASE IF NOT EXISTS `agv`; ");
                string userSql = string.Format("CREATE TABLE IF NOT EXISTS agv.`user` ( `ID` int(10) NOT NULL AUTO_INCREMENT, `User` varchar(50) NOT NULL COMMENT '用户名', `Password` varchar(50) DEFAULT NULL COMMENT '密码', `CardNo` varchar(50) DEFAULT NULL COMMENT '工牌号', `Authorization` varchar(50) NOT NULL COMMENT '权限，0管理员，1调试用户，2普通用户', PRIMARY KEY(`ID`)) ENGINE = InnoDB AUTO_INCREMENT = 2 DEFAULT CHARSET = utf8; ");
                string mapSql = string.Format("CREATE TABLE IF NOT EXISTS agv.`map` ( `ID` int(10) NOT NULL AUTO_INCREMENT, `CreateTime` bigint(20) DEFAULT NULL COMMENT '创建时间', `Name` varchar(50) DEFAULT NULL COMMENT '地图名称', `Width` double DEFAULT NULL COMMENT '地图宽度（米）', `Height` double unsigned DEFAULT NULL COMMENT '地图长度（米）',  `AGV` varchar(50) DEFAULT NULL COMMENT '地图上注册的AGV',  `Type` int(11) DEFAULT NULL COMMENT '地图类型，磁标，RFID，激光 0磁标，1RFID，2激光', PRIMARY KEY(`ID`)) ENGINE = InnoDB AUTO_INCREMENT = 42 DEFAULT CHARSET = utf8 ROW_FORMAT = COMPACT COMMENT = '取名为map+UTC时间，如map1234567890'; ");
                string settingSql = string.Format("CREATE TABLE IF NOT EXISTS agv.`setting` (`ID` int(11) NOT NULL AUTO_INCREMENT, `Map` bigint(20) DEFAULT '0' COMMENT '默认地图', `Mode` int(10) DEFAULT '0' COMMENT '启动方式：0.编辑模式，1.自启动模式', PRIMARY KEY(`ID`)) ENGINE = InnoDB AUTO_INCREMENT = 2 DEFAULT CHARSET = utf8; ");
                string sql = DB + userSql + mapSql + settingSql;
                MySqlConnection mySqlConnection = new MySqlConnection(ConficString);
                mySqlConnection.Open();
                MySqlCommand mc = new MySqlCommand(sql, mySqlConnection);
                mc.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误:" + ex.ToString());
            }
        }

        public bool UpdateTagInfo(long MapTime, DataTable Dt)
        {
            int rowsCount = Dt.Rows.Count;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < rowsCount; i++)
            {
                string sql = string.Format("UPDATE agv.`tag{0}` SET `NextTag` = '{1}',`NextLeftTag` = '{2}', `NextRightTag` = '{3}',`PreTag` = '{4}',`PreLeftTag` = '{5}',`PreRightTag` = '{6}',`Speed` = {7},`SpeedRev` = {8},`StopTime` = {9},`Pbs` = {10},`PbsRev` = {11}, `TagTerminal` = '{12}' WHERE TagName = {13};", MapTime, Dt.Rows[i][4].ToString(), Dt.Rows[i][5].ToString(), Dt.Rows[i][6].ToString(), Dt.Rows[i][1].ToString(), Dt.Rows[i][2].ToString(), Dt.Rows[i][3].ToString(), Dt.Rows[i][7].ToString(), Dt.Rows[i][8].ToString(), Dt.Rows[i][9].ToString(), Dt.Rows[i][10].ToString(), Dt.Rows[i][11].ToString(), Dt.Rows[i][12].ToString(), Dt.Rows[i][0].ToString());
                stringBuilder.Append(sql);
            }

            return AGV.DAL.MySqlHelper.ExecuteNonQuery(stringBuilder.ToString()) > 0 ? true : false;
        }

        public string[] SelectTag(long CreateTime, string TagNo)
        {
            List<string> listSTag = new List<string>();

            listSTag.Add("N/A");
            string sql1 = string.Format("SELECT tag1 FROM agv.line{0} WHERE tag2 = 'TA{1}'", CreateTime, TagNo);
            MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
            mySqlConnection.Open();
            MySqlCommand mc1 = new MySqlCommand(sql1, mySqlConnection);
            MySqlDataReader mr1 = mc1.ExecuteReader();
            while (mr1.Read())
            {
                StringBuilder sb = new StringBuilder(mr1.GetString(0));
                listSTag.Add(sb.Remove(0, 2).ToString());
            }
            mr1.Close();

            string sql2 = string.Format("SELECT tag2 FROM agv. line{0} WHERE tag1 = 'TA{1}'", CreateTime, TagNo);
            MySqlCommand mc2 = new MySqlCommand(sql2, mySqlConnection);
            MySqlDataReader mr2 = mc2.ExecuteReader();
            while (mr2.Read())
            {
                StringBuilder sb = new StringBuilder(mr2.GetString(0));
                listSTag.Add(sb.Remove(0, 2).ToString());
            }
            mr2.Close();

            string[] selectTag = listSTag.ToArray();
            mySqlConnection.Close();
            return selectTag;
        }

        public string ExportSetting(long MapTime, string Db)
        {
            string Delstr = string.Format("DELETE FROM `agv`.`setting`;");
            string insertSqlText = string.Format("INSERT INTO `agv`.`setting` (`ID`, `Map`, `Mode`) VALUES (1, {0} , {1});", MapTime, 1);
            return Delstr + insertSqlText;
        }

        public string ExportMySqlTable(string TableName, string Db)
        {
            string dropSql = string.Format("DROP TABLE IF EXISTS {0}.`{1}`;", Db, TableName);
            StringBuilder sqlText = new StringBuilder(dropSql);
            string createSql = string.Format("CREATE TABLE IF NOT EXISTS {0}.`{1}`", Db, TableName);
            sqlText.Append(createSql);
            sqlText.Append(" (");
            string sql = string.Format("SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}' AND TABLE_SCHEMA = '{1}'", TableName, Db);
            MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
            mySqlConnection.Open();
            MySqlCommand mc = new MySqlCommand(sql, mySqlConnection);

            MySqlDataReader mr = mc.ExecuteReader();
            string keyStr = "";
            int colCount = 0;
            while (mr.Read())
            {
                sqlText.Append(" `");
                sqlText.Append(mr["COLUMN_NAME"].ToString());
                sqlText.Append("` ");
                sqlText.Append(mr["COLUMN_TYPE"].ToString().ToUpper());
                if (mr["IS_NULLABLE"].ToString() == "NO")
                {
                    sqlText.Append(" NOT NULL ");
                }
                else
                {
                    sqlText.Append(" DEFAULT NULL ");
                }
                sqlText.Append(mr["EXTRA"].ToString().ToUpper());
                if (!string.IsNullOrEmpty(mr["COLUMN_COMMENT"].ToString()))
                {
                    sqlText.Append(" COMMENT ");
                    sqlText.Append("'");
                    sqlText.Append(mr["COLUMN_COMMENT"].ToString());
                    sqlText.Append("'");
                }
                sqlText.Append(",");
                if (mr["COLUMN_KEY"].ToString() == "PRI")
                {
                    keyStr = "PRIMARY KEY(`ID`)";
                }
                colCount++;
            }
            sqlText.Append(keyStr);
            sqlText.Append(") ENGINE = InnoDB AUTO_INCREMENT = 1 DEFAULT CHARSET = utf8 ROW_FORMAT = COMPACT;");
            mr.Close();

            sql = string.Format("SELECT * FROM {0}.`{1}`", Db, TableName);
            mc.CommandText = sql;
            mr = mc.ExecuteReader();

            string Delstr = string.Format("DELETE FROM {0}.`{1}`;", Db, TableName);
            StringBuilder insertSqlText = new StringBuilder(Delstr);
            int getName = 0;
            while (mr.Read())
            {
                if (getName == 0)
                {
                    string insertSql = string.Format("INSERT INTO {0}.`{1}` (", Db, TableName);
                    insertSqlText.Append(insertSql);
                    for (int i = 0; i < colCount; i++)
                    {
                        insertSqlText.Append(" `");
                        insertSqlText.Append(mr.GetName(i));
                        if (i < colCount - 1)
                        {
                            insertSqlText.Append("`, ");
                        }
                        else
                        {
                            insertSqlText.Append("`) VALUES");
                        }
                    }
                    getName = 1;
                }

                for (int i = 0; i < colCount; i++)
                {
                    if (i == 0)
                    {
                        insertSqlText.Append("('");
                    }
                    else
                    {
                        insertSqlText.Append(" '");
                    }

                    insertSqlText.Append(mr[i].ToString());

                    if (i < colCount - 1)
                    {
                        insertSqlText.Append("', ");
                    }
                    else
                    {
                        insertSqlText.Append("'),");
                    }
                }
            }
            mr.Close();
            insertSqlText.Remove(insertSqlText.Length - 1, 1);
            insertSqlText.Append(";");
            return sqlText.ToString() + insertSqlText.ToString();
        }


        public string ExportTableContent(string TableName, string Db, string MapTime)
        {
            StringBuilder sqlText = new StringBuilder();
            string sql = string.Format("SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}' AND TABLE_SCHEMA = '{1}'", TableName, Db);
            MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
            mySqlConnection.Open();
            MySqlCommand mc = new MySqlCommand(sql, mySqlConnection);
            MySqlDataReader mr = mc.ExecuteReader();
            int colCount = 0;
            while (mr.Read())
            {
                colCount++;
            }
            //sqlText.Append(keyStr);
            //sqlText.Append(") ENGINE = InnoDB AUTO_INCREMENT = 1 DEFAULT CHARSET = utf8 ROW_FORMAT = COMPACT;");
            mr.Close();

            sql = string.Format("SELECT * FROM {0}.`{1}` WHERE `CreateTime` = '{2}'", Db, TableName, MapTime);
            mc.CommandText = sql;
            mr = mc.ExecuteReader();

            string Delstr = string.Format("DELETE FROM {0}.`{1}` WHERE `CreateTime` = '{2}' ;", Db, TableName, MapTime);
            StringBuilder insertSqlText = new StringBuilder();
            int getName = 0;
            while (mr.Read())
            {
                if (getName == 0)
                {
                    string insertSql = string.Format("INSERT INTO {0}.`{1}` (", Db, TableName);
                    insertSqlText.Append(insertSql);
                    for (int i = 0; i < colCount; i++)
                    {
                        insertSqlText.Append(" `");
                        insertSqlText.Append(mr.GetName(i));
                        if (i < colCount - 1)
                        {
                            insertSqlText.Append("`, ");
                        }
                        else
                        {
                            insertSqlText.Append("`) VALUES");
                        }
                    }
                    getName = 1;
                }

                for (int i = 0; i < colCount; i++)
                {
                    if (i == 0)
                    {
                        insertSqlText.Append("('");
                    }
                    else
                    {
                        insertSqlText.Append(" '");
                    }

                    insertSqlText.Append(mr[i].ToString());

                    if (i < colCount - 1)
                    {
                        insertSqlText.Append("', ");
                    }
                    else
                    {
                        insertSqlText.Append("'),");
                    }
                }
            }
            mr.Close();
            insertSqlText.Remove(insertSqlText.Length - 1, 1);
            insertSqlText.Append(";");
            return Delstr + insertSqlText.ToString();
        }
    }
}
