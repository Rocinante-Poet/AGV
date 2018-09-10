using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace AGV.DAL
{
    public class LineInfoDAL
    {
        /// <summary>
        /// 查询所有线路
        /// </summary>
        /// <param name="Times"></param>
        /// <returns></returns>
        public DataTable LineData(string Times)
        {
            return MySqlHelper.ExecuteDataTable("select * from line" + Times + "");
        }







    }
}
