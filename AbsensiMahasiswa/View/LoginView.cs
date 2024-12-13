using AbsensiMahasiswa.Controllers;
using System;

namespace AbsensiMahasiswa.Views
{
    public class LoginView
    {
        private LoginController _loginController;

        public LoginView(LoginController loginController)
        {
            _loginController = loginController;
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
                    ShowMainMenu(); //Main Menu Setelah Login
                    string pilihan = Console.ReadLine();

                    switch (pilihan)
                    {
                        case "1":
                            Console.WriteLine("Absensi Mahasiswa...");
                            // Implementasi Absensi Mahasiswa
                            break;
                        case "2":
                            Console.WriteLine("Lihat Rekap Absensi...");
                            // Implementasi Lihat Rekap Absensi
                            break;
                        case "3":
                            Console.WriteLine("Masukkan Mahasiswa...");
                            // Implementasi Masukkan Mahasiswa
                            break;
                        case "4":
                            Console.WriteLine("Hapus Mahasiswa...");
                            // Implementasi Hapus Mahasiswa
                            break;
                        case "5":
                            Console.WriteLine("Hapus Rekap Absensi...");
                            // Implementasi Hapus Rekap Absensi
                            break;
                        case "6":
                            Console.WriteLine("Keluar...");
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Opsi tidak valid. Silakan coba lagi.");
                            break;
                    }
                }
                else
                {
                    // Jika belum login, tampilkan opsi login atau keluar
                    Console.WriteLine("=== Selamat datang di Aplikasi Absensi ===");
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Exit");
                    Console.Write("Pilih opsi (1/2): ");
                    string pilihan = Console.ReadLine();

                    switch (pilihan)
                    {
                        case "1":
                            loggedIn = Login();  // Jika login berhasil, set loggedIn ke true
                            break;
                        case "2":
                            exit = true;  // Keluar dari aplikasi
                            Console.WriteLine("Terima kasih telah menggunakan aplikasi. Keluar...");
                            break;
                        default:
                            Console.WriteLine("Opsi tidak valid, coba lagi.");
                            break;
                    }
                }
            }
        }

        private void ShowMainMenu()
        {
            // Menampilkan menu utama setelah login berhasil
            Console.WriteLine("Pilihan menu:");
            Console.WriteLine("1. Absensi Mahasiswa");
            Console.WriteLine("2. Lihat Rekap Absensi");
            Console.WriteLine("3. Masukkan Mahasiswa");
            Console.WriteLine("4. Hapus Mahasiswa");
            Console.WriteLine("5. Hapus Rekap Absensi");
            Console.WriteLine("6. Keluar");
            Console.Write("Pilih opsi (1/2/3/4/5/6): ");
        }

        // Method untuk menangani login
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

                // Memeriksa kredensial login
                bool loginSuccessful = _loginController.Login(username, password);

                if (loginSuccessful)
                {
                    Console.WriteLine("Login Berhasil!");
                    return true;  // Login berhasil, return true untuk menandakan bahwa login berhasil
                }
                else
                {
                    attemptCount++;
                    Console.WriteLine($"Login Gagal! Username atau Password salah. Percobaan ke-{attemptCount}/{maxAttempts}");

                    if (attemptCount == maxAttempts)
                    {
                        Console.WriteLine("Terlalu banyak percobaan login gagal. Keluar aplikasi.");
                        Environment.Exit(0);  // Keluar dari aplikasi setelah 3 kali gagal
                    }
                }

                Console.WriteLine("Tekan Enter untuk mencoba lagi...");
                Console.ReadLine();
            }

            return false; // Jika login gagal setelah 3 percobaan
        }
    }
}
