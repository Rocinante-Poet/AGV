using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGV.DAL;
using System.Data;
using MySql.Data.MySqlClient;

namespace AGV.BLL
{
   public class widgetManagementBLL
    {
        widgetManagementDAL ManagementDAL = new widgetManagementDAL();

        public DataTable GetWidget(string Times)
        {
            return ManagementDAL.widgetArrlist(Times);
        }


    }
}
