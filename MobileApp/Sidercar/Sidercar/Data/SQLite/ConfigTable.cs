using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Sidercar.Data.SQLite
{
    public class ConfigTable
    {

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Metros { get; set; }
        public int Tiempo { get; set; }
    }

}
