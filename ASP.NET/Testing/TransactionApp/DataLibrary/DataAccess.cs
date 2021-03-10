using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class DataAccess : IDataAccess
    {
        public async Task<List<T>> LoadData<T, U>(string sql, U parameters, string connectionString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                var rows = await connection.QueryAsync<T>(sql, parameters);
                return rows.ToList();
            }
        }

        public async Task<int> SaveData<T>(string sql, T parameters, string connectionString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                return await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
