using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Cartomatic.Utils.Data
{
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// Closes db connection 
        /// </summary>
        /// <param name="conn">Database connection object</param>
        public static void CloseConnection(this IDbConnection conn)
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }
}
