using AbsensiMahasiswa.Controllers;
using System;

namespace AbsensiMahasiswa.Views
{
    public class LoginView
    {
        private LoginController _loginController;
        private MahasiswaController _mahasiswaController;  //Object

        public LoginView(LoginController loginController, MahasiswaController mahasiswaController)
        {
            _loginController = loginController;
            _mahasiswaController = mahasiswaController;  // init controller
        }

        public void Show()
        {
            bool exit = false;
            bool loggedIn = false;  // Login Check

            while (!exit)
            {
                Console.Clear();
                
                // Menu Login
                if (loggedIn)
                {
                    ShowMainMenu(); // Main Menu 
                    string pilihan = Console.ReadLine();

                    switch (pilihan)
                    {
                        case "1":
                            Console.WriteLine("Absensi Mahasiswa...");
                            // Implement Absensi Mahasiswa
                            break;
                        case "2":
                            Console.WriteLine("Lihat Rekap Absensi...");
                            // Implement Lihat Rekap Absensi
                            break;
                        case "3":
                             MahasiswaView mahasiswaView = new MahasiswaView(_mahasiswaController, this); //oper Informasi LoGIN
                             mahasiswaView.Show();  // Pass the correct controller
                            break;
                        case "4":
                            Console.WriteLine("Hapus Mahasiswa...");
                            // Implement Hapus Mahasiswa
                            break;
                        case "5":
                            Console.WriteLine("Hapus Rekap Absensi...");
                            // Implement Hapus Rekap Absensi
                            break;
                        case "6":
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
                  // kalo login false tampilin
                    Console.WriteLine("=== Selamat datang di Aplikasi Absensi ===");
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Exit");
                    Console.Write("Pilih opsi (1/2): ");
                    string pilihan = Console.ReadLine();

                    switch (pilihan)
                    {
                        case "1":
                            loggedIn = Login();  // kalo login sukses tampiin
                            break;
                        case "2":
                            exit = true;  // Exit 
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
           //Main Menu
            Console.WriteLine("Pilihan menu:");
            Console.WriteLine("1. Absensi Mahasiswa");
            Console.WriteLine("2. Lihat Rekap Absensi");
            Console.WriteLine("3. Masukkan Mahasiswa");
            Console.WriteLine("4. Hapus Mahasiswa");
            Console.WriteLine("5. Exit");
            Console.Write("Pilih opsi (1/2/3/4/5/6): ");
        }

        // Method  login
        private bool Login()
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

                // Check  credentials
                bool loginSuccessful = _loginController.Login(username, password);

                if (loginSuccessful)
                {
                    Console.WriteLine("Login Berhasil!");
                    return true;  // Login successful, return true
                }
                else
                {
                    attemptCount++;
                    Console.WriteLine($"Login Gagal! Username atau Password salah. Percobaan ke-{attemptCount}/{maxAttempts}");

                    if (attemptCount == maxAttempts)
                    {
                        Console.WriteLine("Terlalu banyak percobaan login gagal. Keluar aplikasi.");
                        Environment.Exit(0);  // Exit 3 kali gagal 
                    }
                }

                Console.WriteLine("Tekan Enter untuk mencoba lagi...");
                Console.ReadLine();
            }

            return false; // Return false kalo login fails 3 
        }
    }
}
