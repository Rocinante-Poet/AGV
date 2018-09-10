using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGV.DAL;
using AGVManagement.instrument;

namespace AGV.BLL
{
    public class OperateDBBLL
    {
        OperateDB operateDB = new OperateDB();

        public List<string> AgvNumListMap(long MapTime)
        {
            return operateDB.AgvNumList(MapTime);
        }

        public void CreateDBMap()
        {
            operateDB.CreateDB();
        }


        public bool UpdateTagDtat(long MapTime, DataTable Dt)
        {
           return operateDB.UpdateTagInfo(MapTime, Dt);
        }

        public string[] SelectTagArr(long CreateTime, string TagNo)
        {
            return operateDB.SelectTag(CreateTime, TagNo);
        }

        public string ExportSettings(long MapTime, string Db)
        {
            return operateDB.ExportSetting(MapTime, Db);
        }

        public string ExportMySqlTables(string TableName, string Db)
        {
            return operateDB.ExportMySqlTable(TableName, Db);
        }

        public string ExportTableContents(string TableName, string Db, string MapTime)
        {
            return operateDB.ExportTableContent(TableName, Db, MapTime);
        }
    }
}
