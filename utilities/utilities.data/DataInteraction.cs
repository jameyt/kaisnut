using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace utilities.data
{
    public class DataInteraction
    {
        public static IDbConnection CreateLocalSqlConnection(string databaseLocation)
        {
            var cs = string.Format(
                        @"Data Source=(LocalDB)\v11.0;
                          AttachDbFilename={0};
                          Integrated Security=True;
                          Connect Timeout=30;"
                        , databaseLocation);
            var cn = new SqlConnection(cs);
            cn.Open();
            return cn;
        }

    }
}
