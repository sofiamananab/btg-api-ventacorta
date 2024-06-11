using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace API_GP_VentaCorta.Models
{
    public class Parameter
    {
        public string name { get; set; }
        public object value { get; set; }
        // public DbType type { get; set; }
        public OracleDbType type { get; set; }
        public bool isCursor { get; set; }
        public int size { get; set; }
    }
}