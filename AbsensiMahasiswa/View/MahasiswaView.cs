using AbsensiMahasiswa.Controllers;
using AbsensiMahasiswa.Models;
using System;

namespace AbsensiMahasiswa.Views
{
    public class MahasiswaView
    {
        private MahasiswaController _mahasiswaController;
        private LoginView _loginView;  //object

        public MahasiswaView(MahasiswaController mahasiswaController, LoginView loginView)
        {
            _mahasiswaController = mahasiswaController;
            _loginView = loginView;  // Init login view
        }

        public void Show()
        {
            bool continueInput = true;

            while (continueInput)
            {
                Console.WriteLine("=== Masukkan Mahasiswa ===");

                // seberapa banyak input
                Console.Write("Berapa banyak mahasiswa yang ingin dimasukkan? ");
                int jumlahMahasiswa;
                while (!int.TryParse(Console.ReadLine(), out jumlahMahasiswa) || jumlahMahasiswa <= 0)
                {
                    Console.Write("Masukkan jumlah yang valid (lebih besar dari 0): ");
                }

                // Loop input
                for (int i = 0; i < jumlahMahasiswa; i++)
                {
                    Console.WriteLine($"\nMasukkan Mahasiswa {i + 1}:");

                    Console.Write("Masukkan Nama Mahasiswa: ");
                    string nama = Console.ReadLine();

                    Console.Write("Masukkan NIM Mahasiswa: ");
                    string nim = Console.ReadLine();

                    Console.Write("Masukkan Nama Kelas Mahasiswa: ");
                    string kelas = Console.ReadLine();

                    // ambil object dari model
                    Mahasiswa mahasiswa = new Mahasiswa
                    {
                        Nama = nama,
                        NIM = nim,
                        Status = "Aktif", // default status
                        Kelas = new Kelas { NamaKelas = kelas }  // object dari model
                    };

                    //panggil controller
                    _mahasiswaController.InsertMahasiswa(mahasiswa);
                }

                //Barhasil ditambahin
                Console.WriteLine("\nSemua mahasiswa berhasil ditambahkan!");

                // lanjut atau engga
                Console.Write("\nApakah Anda ingin melanjutkan? (y/n): ");
                string choice = Console.ReadLine();
                if (choice.ToLower() != "y")
                {
                    continueInput = false; //exit loop
            }

            Console.WriteLine("\nKembali ke menu utama...");
            _loginView.ShowMainMenu();  // balik main menu
    }
}
    }
}