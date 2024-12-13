using MySql.Data.MySqlClient;
using AbsensiMahasiswa.Utils;
using System;

namespace AbsensiMahasiswa.Controllers
{
    public class LoginController
    {
        private string connectionString;

        public LoginController(string server, string database, string userId, string password)
        {
            connectionString = $"Server={server};Database={database};User ID={userId};Password={password};";
        }

       //Check Username dan password dari admin
        public bool CheckAdminCredentials(string username, string password)
        {
        //Query untuk memeriksa apakah username dan password yang dimasukkan benar.
            string query = $"SELECT COUNT(*) FROM Admin WHERE Username = @Username AND Password = @Password";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);  

                    var result = Convert.ToInt32(cmd.ExecuteScalar());

                    return result > 0;  // Jika ada hasil, berarti login berhasil
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
        }

        public bool Login(string username, string password)
        {
            //Chcek credentials
            return CheckAdminCredentials(username, password);
        }
    }
}
