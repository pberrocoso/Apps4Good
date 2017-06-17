using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Sidercar.Data.SQLite
{
    public class DatabaseSider
    {
        readonly SQLiteAsyncConnection database;

        public DatabaseSider(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<ConfigTable>().Wait();
        }

        public Task<List<ConfigTable>> GetItemsAsync()
        {
            return database.Table<ConfigTable>().ToListAsync();
        }

        public Task<ConfigTable> GetItemAsync(int id)
        {
            return database.Table<ConfigTable>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(ConfigTable item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(ConfigTable item)
        {
            return database.DeleteAsync(item);
        }
    }
}
