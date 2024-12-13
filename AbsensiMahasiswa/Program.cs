using AbsensiMahasiswa.Controllers;
using AbsensiMahasiswa.Utils;
using AbsensiMahasiswa.Views;
using System;

namespace AbsensiMahasiswa
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Instance db helper
            DatabaseHelper databaseHelper = new DatabaseHelper("localhost", "absensi_mahasiswa", "root", "");

            // instance mahasiswa contorller
            MahasiswaController mahasiswaController = new MahasiswaController(databaseHelper);

            // instancec login
            LoginController loginController = new LoginController("localhost", "absensi_mahasiswa", "root", "");

            // instance login view x controller
            LoginView loginView = new LoginView(loginController, mahasiswaController);

            // Menampilkan Login View
            loginView.Show();
        }
    }
}
