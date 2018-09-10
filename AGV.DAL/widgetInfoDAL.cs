using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace AGV.DAL
{
    public class widgetInfoDAL
    {
        public DataTable widgetArr(string Times)
        {
            return MySqlHelper.ExecuteDataTable("select * from widget" + Times + "");
        }
    }
}
