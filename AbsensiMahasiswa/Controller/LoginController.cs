using MySql.Data.MySqlClient;
using AbsensiMahasiswa.Utils;
using System;

namespace AbsensiMahasiswa.Controllers
{
    public class LoginController
    {
        private readonly DatabaseHelper _databaseHelper;

        public LoginController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }


     
        /// Jangan DIHAPUS!
        /// Auth admin berdasarkan username and password dan return the admin ID.
     
        /// <param name="username">Admin username</param>
        /// <param name="password">Admin password</param>
      
        public int Authenticate(string username, string password)
        {
            string query = "SELECT admin_id FROM Admin WHERE Username = @Username AND Password = @Password";

            using (var connection = _databaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        object result = cmd.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int adminId))
                        {
                            return adminId;  // Return the admin ID if login is successful
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            // Return -1 if authentication fails or an error occurs
            return -1;
        }
    }
}
