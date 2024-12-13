using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace AbsensiMahasiswa.Utils
{
    public class DatabaseHelper
    {
        private string connectionString;

        // Constructor 
        public DatabaseHelper(string server, string database, string userId, string password)
        {
            connectionString = $"Server={server};Database={database};User ID={userId};Password={password};";
        }

        // Method 
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        // Methodn query yan gak menghasilkan data (INSERT, UPDATE, DELETE)
        public void ExecuteNonQuery(string query)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        // Method query yang menghasilkan data (SELECT)
        public DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return dt;
        }
    }
}
