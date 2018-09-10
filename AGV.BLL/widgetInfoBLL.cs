using AGV.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace AGV.BLL
{
   public class widgetInfoBLL
    {
        widgetInfoDAL infoDAL = new widgetInfoDAL();

        public DataTable WidgetLIst(string Times)
        {
            return infoDAL.widgetArr(Times);
        }
    }
}
