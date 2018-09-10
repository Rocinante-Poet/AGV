using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AGV.DAL;
using MySql.Data.MySqlClient;

namespace AGV.BLL
{
   public class LineInfoBLL
    {
        LineInfoDAL LineInfo = new LineInfoDAL();
        public DataTable LinelistArrer(string Times)
        {
            return LineInfo.LineData(Times);
        }
    }
}
