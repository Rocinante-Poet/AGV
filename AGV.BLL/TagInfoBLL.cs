using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGV.DAL;
using MySql.Data.MySqlClient;

namespace AGV.BLL
{
    public class TagInfoBLL
    {
       private TagInfoDAL tagInfoDAL = new TagInfoDAL();
        public DataTable RataTable  (string exls)
        {
            return tagInfoDAL.GetMapTags(exls);
        }

        public MySqlDataReader RataTable(long exls)
        {
            return tagInfoDAL.GetMapTags(exls);
        }
    }
}
