using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using appData.Models;
using MySql.Data.MySqlClient;

namespace webApp.Models
{
    public class cardsContext
    {
        public string ConnectionString { get; set; }

        public cardsContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<cards> GetAllCards()
        {
            List<cards> list = new List<cards>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from cards where cardId < 10", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while( reader.Read())
                    {
                        list.Add(new cards()
                        {
                            cardId = Convert.ToInt32(reader["id"]),
                            cvv = reader["cvv"].ToString(),
                            expirationDate = reader["exiprationDate"].ToString(),
                            cardNumber = reader["cardNumber"].ToString()
                        });
                    }
                }
            }
            return list;
        }
    }
}
