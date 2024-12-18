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
            bool exit_0602 = false;
            bool loggedIn_0602 = false;

            while (!exit_0602)
            {
                Console.Clear();

                //logged in menu kalo login
                if (loggedIn_0602)
                {
                    ShowMainMenu();
                    string pilihan_0602 = Console.ReadLine();

                    switch (pilihan_0602)
                    {
                        case "1":
                            // Navigate to MahasiswaView
                            MahasiswaView mahasiswaView_0602 = new MahasiswaView(_mahasiswaController, this);
                            mahasiswaView_0602.Show();
                            break;

                        case "2":
                            // Navigate to AbsensiView
                            AbsensiView absensiView_0602 = new AbsensiView(_absensiController, _mahasiswaController, _idAdmin);
                            absensiView_0602.Show();
                            break;

                        case "3":
                            Console.WriteLine("Exit...");
                            exit_0602 = true;
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
                    string pilihan_0602 = Console.ReadLine();

                    switch (pilihan_0602)
                    {
                        case "1":
                           
                            int idAdmin = Login();
                            if (idAdmin != -1)
                            {
                                loggedIn_0602 = true;
                                _idAdmin = idAdmin; //simpen id amdin yang login
                            }
                            break;

                        case "2":
                            exit_0602 = true;
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
            int maxAttempts_0602 = 3;
            int attemptCount_0602 = 0;

            while (attemptCount_0602 < maxAttempts_0602)
            {
                Console.Clear();
                Console.WriteLine("=== Login Page ===");
                Console.Write("Masukkan Username: ");
                string username_0602 = Console.ReadLine();
                Console.Write("Masukkan Password: ");
                string password_0602 = Console.ReadLine();

                // Auth admin
                int adminId = _loginController.Authenticate(username_0602, password_0602);

                if (adminId != -1)  // login sukses
                {
                    Console.WriteLine("Login Berhasil!");
                    return adminId;  // Return admin ID 
                }
                else
                {
                    attemptCount_0602++;
                    Console.WriteLine($"Login Gagal! Username atau Password salah. Percobaan ke-{attemptCount_0602}/{maxAttempts_0602}");

                    if (attemptCount_0602 == maxAttempts_0602)
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
