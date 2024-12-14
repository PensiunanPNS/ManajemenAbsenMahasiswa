using AbsensiMahasiswa.Controllers;
using System;

namespace AbsensiMahasiswa.Views
{
    public class LoginView
    {
        private LoginController _loginController;
        private MahasiswaController _mahasiswaController;
        private AbsensiController _absensiController;
        private int _idAdmin;  // Buat nyimpen id admin yang login

        public LoginView(LoginController loginController, MahasiswaController mahasiswaController, AbsensiController absensiController)
        {
            //inisailisasi controller
            _loginController = loginController;
            _mahasiswaController = mahasiswaController;
            _absensiController = absensiController; 
        }

        public void Show()
        {
            bool exit = false;
            bool loggedIn = false;

            while (!exit)
            {
                Console.Clear();

                //logged in menu kalo login
                if (loggedIn)
                {
                    ShowMainMenu();
                    string pilihan = Console.ReadLine();

                    switch (pilihan)
                    {
                        case "1":
                            // Navigate to MahasiswaView
                            MahasiswaView mahasiswaView = new MahasiswaView(_mahasiswaController, this);
                            mahasiswaView.Show();
                            break;

                        case "2":
                            // Navigate to AbsensiView
                            AbsensiView absensiView = new AbsensiView(_absensiController, _mahasiswaController, _idAdmin);
                            absensiView.Show();
                            break;

                        case "3":
                            Console.WriteLine("Exit...");
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Opsi tidak valid. Silakan coba lagi.");
                            break;
                    }
                }
                else
                {
                    //menu kalo belom login
                    Console.WriteLine("=== Selamat datang di Aplikasi Absensi ===");
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Exit");
                    Console.Write("Pilih opsi (1/2): ");
                    string pilihan = Console.ReadLine();

                    switch (pilihan)
                    {
                        case "1":
                           
                            int idAdmin = Login();
                            if (idAdmin != -1)
                            {
                                loggedIn = true;
                                _idAdmin = idAdmin; //simpen id amdin yang login
                            }
                            break;

                        case "2":
                            exit = true;
                            Console.WriteLine("Terima kasih telah menggunakan aplikasi. Keluar...");
                            break;

                        default:
                            Console.WriteLine("Opsi tidak valid, coba lagi.");
                            break;
                    }
                }
            }
        }

        public void ShowMainMenu()
        {
            Console.WriteLine("=== Menu Utama ===");
            Console.WriteLine("1. Mahasiswa");
            Console.WriteLine("2. Absensi");
            Console.WriteLine("3. Exit");
            Console.Write("Pilih opsi (1/2/3): ");
        }

      private int Login()
{
    int maxAttempts = 3;
    int attemptCount = 0;

    while (attemptCount < maxAttempts)
    {
        Console.Clear();
        Console.WriteLine("=== Login Page ===");
        Console.Write("Masukkan Username: ");
        string username = Console.ReadLine();
        Console.Write("Masukkan Password: ");
        string password = Console.ReadLine();

        // Auth admin
        int adminId = _loginController.Authenticate(username, password);

        if (adminId != -1)  // login sukses
        {
            Console.WriteLine("Login Berhasil!");
            return adminId;  // Return admin ID 
        }
        else
        {
            attemptCount++;
            Console.WriteLine($"Login Gagal! Username atau Password salah. Percobaan ke-{attemptCount}/{maxAttempts}");

            if (attemptCount == maxAttempts)
            {
                Console.WriteLine("Terlalu banyak percobaan login gagal. Keluar aplikasi.");
                Environment.Exit(0);  //3x gagal exit
            }
        }

        Console.WriteLine("Tekan Enter untuk mencoba lagi...");
        Console.ReadLine();
    }

    return -1; 
}
    }
}
