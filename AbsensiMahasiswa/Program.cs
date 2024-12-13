using AbsensiMahasiswa.Controllers;
using AbsensiMahasiswa.Views;
using System;

namespace AbsensiMahasiswa
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // buat instance logincontroller 
            LoginController loginController = new LoginController("localhost", "absensi_mahasiswa", "root", "");

            // buat instance dari LoginView
            LoginView loginView = new LoginView(loginController);

            // Nampilin Login View
            loginView.Show();
        }
    }
}
