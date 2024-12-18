using AbsensiMahasiswa.Controllers;
using AbsensiMahasiswa.Models;
using System;

namespace AbsensiMahasiswa.Views
{
    public class MahasiswaView
    {
        private MahasiswaController _mahasiswaController;
        private LoginView _loginView; // object

        public MahasiswaView(MahasiswaController mahasiswaController, LoginView loginView)
        {
            _mahasiswaController = mahasiswaController;
            _loginView = loginView; // Init login view
        }

        public void Show()
        {
            bool continueMenu = true;

            while (continueMenu)
            {
                Console.WriteLine("\n=== Menu Mahasiswa ===");
                Console.WriteLine("1. Tambah Mahasiswa");
                Console.WriteLine("2. Nonaktifkan Mahasiswa");
                Console.WriteLine("3. Update Data Mahasiswa");
                Console.WriteLine("4. Kembali ke Menu Utama");
                Console.Write("Pilih opsi (1/2/3/4): ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        InsertMahasiswa();
                        break;
                    case "2":
                        DeleteMahasiswa();
                        break;
                    case "3":
                        UpdateMahasiswa();
                        break;
                    case "4":
                        continueMenu = false;
                        break;
                    default:
                        Console.WriteLine("Ups pilihan tidak valid. Coba lagi.");
                        break;
                }
            }

            Console.WriteLine("\nKembali ke menu utama...");
            _loginView.ShowMainMenu(); // Balik ke menu utama
        }

        private int SelectKelas()
        {
            bool exit = false;
            while (!exit)
            {
                var kelasList = _mahasiswaController.GetAllKelas();
                if (kelasList.Count == 0)
                {
                    Console.WriteLine("\nTidak ada kelas yang tersedia.");
                    Console.Write("Kembali ke menu mahasiswa? (y/n): ");
                    exit = Console.ReadLine().ToLower() != "y";
                    if (exit)
                    {
                        return -1; // Balik ke menu utama
                    }
                    continue; // loop kalau masih mau di menu
                }

                Console.WriteLine("\n=== Daftar Kelas ===");
                for (int i = 0; i < kelasList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {kelasList[i].NamaKelas}");
                }

                Console.Write("Pilih kelas (1-{0}): ", kelasList.Count);
                int pilihan;
                while (!int.TryParse(Console.ReadLine(), out pilihan) || pilihan < 1 || pilihan > kelasList.Count)
                {
                    Console.Write("Pilihan tidak valid. Masukkan angka antara 1 dan {0}: ", kelasList.Count);
                }

                return kelasList[pilihan - 1].IdKelas; // Pilihan valid, keluar dari loop
            }

            return -1; // exit adalah true
        }


        private Mahasiswa SelectMahasiswaByKelas(int idKelas)
        {
            
            var mahasiswaList = _mahasiswaController.GetMahasiswaByKelas(idKelas);
            if (mahasiswaList.Count == 0)
            {
                Console.WriteLine("Tidak ada mahasiswa di kelas ini.");
                return null;
            }

            Console.WriteLine("\n=== Daftar Mahasiswa ===");
            for (int i = 0; i < mahasiswaList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {mahasiswaList[i].Nama} - {mahasiswaList[i].NIM}");
            }

            Console.Write("Pilih mahasiswa (1-{0}): ", mahasiswaList.Count);
            int pilihan;
            while (!int.TryParse(Console.ReadLine(), out pilihan) || pilihan < 1 || pilihan > mahasiswaList.Count)
            {
                Console.Write("Ups pilihan tidak valid. Masukkan angka antara 1 dan {0}: ", mahasiswaList.Count);
            }

            return mahasiswaList[pilihan - 1];
        }

        private void InsertMahasiswa()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== Masukkan Mahasiswa ===");

                Console.Write("Berapa banyak mahasiswa yang ingin dimasukkan? ");
                int jumlahMahasiswa;
                while (!int.TryParse(Console.ReadLine(), out jumlahMahasiswa) || jumlahMahasiswa <= 0)
                {
                    Console.Write("Masukkan jumlah yang valid (lebih besar dari 0): ");
                }

                for (int i = 0; i < jumlahMahasiswa; i++)
                {
                    Console.WriteLine($"\nMasukkan Mahasiswa {i + 1}:");

                    Console.Write("Masukkan Nama Mahasiswa: ");
                    string nama = Console.ReadLine();

                    Console.Write("Masukkan NIM Mahasiswa: ");
                    string nim = Console.ReadLine();

                    Console.Write("Masukkan Nama Kelas Mahasiswa: ");
                    string kelas = Console.ReadLine();

                    Mahasiswa mahasiswa = new Mahasiswa
                    {
                        Nama = nama,
                        NIM = nim,
                        Status = "Aktif", // Default status
                        Kelas = new Kelas { NamaKelas = kelas } // Object dari model
                    };

                    _mahasiswaController.InsertMahasiswa(mahasiswa);
                }

                Console.WriteLine("\nSemua mahasiswa berhasil ditambahkan!");

                Console.Write("Kembali ke menu mahasiswa? (y/n): ");
                string pilihan = Console.ReadLine().ToLower();

                if (pilihan == "y")
                {
                    exit = true; // Keluar dari loop dan kembali ke menu utama
                }
                else if (pilihan != "n")
                {
                    Console.WriteLine("Ups pilihan tidak valid. Harap pilih 'y' untuk kembali atau 'n' untuk tetap di menu ini.");
                }
            }
        }

        public void DeleteMahasiswa()
        {
             bool exit = false;
            while (!exit)
            {
            int idKelas = SelectKelas();
            if (idKelas == -1) return;

            var mahasiswa = SelectMahasiswaByKelas(idKelas);
            if (mahasiswa == null) return;

            Console.Write($"Apakah Anda yakin ingin menghapus mahasiswa {mahasiswa.Nama} dengan NIM {mahasiswa.NIM}? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                bool success = _mahasiswaController.DeleteMahasiswa(mahasiswa.NIM);
                Console.WriteLine(success ? "Status mahasiswa berhasil diubah menjadi Tidak Aktif!" : "Gagal mengubah status mahasiswa.");
            }
            else
            {
                Console.WriteLine("Penghapusan dibatalkan.");
            }
             Console.Write("Kembali ke menu mahasiswa? (y/n): ");
                exit = Console.ReadLine().ToLower() != "y";
            }
        }
    

        public void UpdateMahasiswa()
        {
            bool exit = false;
            while (!exit)
            {
                int idKelas = SelectKelas();
                if (idKelas == -1) return;

                var mahasiswa = SelectMahasiswaByKelas(idKelas);
                if (mahasiswa == null) return;

                Console.WriteLine("\nMasukkan data baru (kosongkan jika tidak ingin mengubah):");

                Console.Write($"Nama ({mahasiswa.Nama}): ");
                string namaBaru = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(namaBaru)) mahasiswa.Nama = namaBaru;

                Console.Write($"Kelas ({mahasiswa.Kelas?.NamaKelas}): ");
                string kelasBaru = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(kelasBaru)) mahasiswa.Kelas = new Kelas { NamaKelas = kelasBaru };

                bool success = _mahasiswaController.UpdateMahasiswa(mahasiswa);
                Console.WriteLine(success ? "Data ahasiswa berhasil diperbarui!" : "Gagal memperbarui Data mahasiswa.");

                Console.Write("Kembali ke menu mahasiswa? (y/n): ");
                exit = Console.ReadLine().ToLower() != "y";
            }
        }
    }
}
