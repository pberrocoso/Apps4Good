using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sidercar.Data.SQLite
{
    public class DatabaseHelper
    {
        static DatabaseSider database;

        public static DatabaseSider Database
        {
            get
            {
                if (database == null)
                {
                    database = new DatabaseSider(DependencyService.Get<IFileHelper>().GetLocalFilePath("siderSQLite.db3"));
                }
                return database;
            }
        }
    }
}
