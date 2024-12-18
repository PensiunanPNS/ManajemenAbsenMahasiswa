using System;
using System.Linq;
using AbsensiMahasiswa.Models;
using AbsensiMahasiswa.Controllers;
using System.Collections.Generic;

namespace AbsensiMahasiswa.Views
{
    public class AbsensiView
    {
        private AbsensiController _absensiController;
        private MahasiswaController _mahasiswaController;
        private readonly int _idAdmin;

        public AbsensiView(AbsensiController absensiController, MahasiswaController mahasiswaController, int idAdmin)
        {
            _absensiController = absensiController;
            _mahasiswaController = mahasiswaController;
            _idAdmin = idAdmin;
        }

        // View: AbsensiView
        public void Show()
        {
            bool exit_0602 = false;
            while (!exit_0602)
            {
                Console.Clear();
                Console.WriteLine("=== Menu Absensi ===");
                Console.WriteLine("1. Lakukan Absensi");
                Console.WriteLine("2. Rekap Absensi Berdasarkan Tanggal");
                Console.WriteLine("3. Edit Absensi");
                Console.WriteLine("4. Kembali");
                Console.Write("Pilih menu: ");

                string pilihan_0602 = Console.ReadLine();

                switch (pilihan_0602)
                {
                    case "1":
                        Absensi();
                        break;
                    case "2":
                        RekapAbsensi();
                        break;
                    case "3":
                        EditAbsensi();
                        break;
                    case "4":
                        exit_0602 = true;
                        break;
                    default:
                        Console.WriteLine("Pilihan tidak valid. Tekan Enter untuk mencoba lagi.");
                        Console.ReadLine();
                        break;
                }
            }
        }


        private void EditAbsensi()
        {
            // Tampilkan semua kelas
            var kelasList_0602 = _absensiController.GetAllKelas();
            if (kelasList_0602 == null || !kelasList_0602.Any())
            {
                Console.WriteLine("Tidak ada data kelas.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("=== Daftar Kelas ===");
            for (int i = 0; i < kelasList_0602.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {kelasList_0602[i].NamaKelas}");
            }

            Console.Write("Pilih kelas (masukkan angka): ");
            if (!int.TryParse(Console.ReadLine(), out int kelasIndex) || kelasIndex < 1 || kelasIndex > kelasList_0602.Count)
            {
                Console.WriteLine("Pilihan tidak valid.");
                Console.ReadLine();
                return;
            }

            // Dapetin id kelas yang dipilih
            var selectedKelas_0602 = kelasList_0602[kelasIndex - 1];
            Console.WriteLine($"Anda memilih kelas: {selectedKelas_0602.NamaKelas}");

            // Input tanggal yang dimau
            Console.Write("Masukkan tanggal (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime tanggal_0602))
            {
                Console.WriteLine("Format tanggal tidak valid.");
                Console.ReadLine();
                return;
            }

            // tampilin rekap absen yang ada
            var rekapAbsensi_0602 = _absensiController.GetRekapAbsensiByTanggal(selectedKelas_0602.IdKelas, tanggal_0602);

            if (rekapAbsensi_0602 == null || !rekapAbsensi_0602.Any())
            {
                Console.WriteLine("Tidak ada absensi pada tanggal ini.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"=== Rekap Absensi untuk Kelas: {selectedKelas_0602.NamaKelas} pada {tanggal_0602:yyyy-MM-dd} ===");

            // Displat list absensi dari mahasiswa
            for (int i = 0; i < rekapAbsensi_0602.Count; i++)
            {
                var absensi_0602 = rekapAbsensi_0602[i];
                Console.WriteLine($"{i + 1}. {absensi_0602.Mahasiswa.Nama} ({absensi_0602.Mahasiswa.NIM}) - Status: {absensi_0602.Status}");
            }

            // pilih ahasiswa ynag mau diedit
            Console.Write("Pilih mahasiswa yang ingin diedit (masukkan angka): ");
            if (!int.TryParse(Console.ReadLine(), out int studentIndex) || studentIndex < 1 || studentIndex > rekapAbsensi_0602.Count)
            {
                Console.WriteLine("Pilihan tidak valid.");
                Console.ReadLine();
                return;
            }

            var mahasiswaToEdit_0602 = rekapAbsensi_0602[studentIndex - 1];
            Console.WriteLine($"Absensi yang ada untuk {mahasiswaToEdit_0602.Mahasiswa.Nama}: {mahasiswaToEdit_0602.Status}");

            // Edit absensi
            Console.WriteLine("Masukkan status kehadiran baru! ");
            Console.WriteLine("1. Hadir");
            Console.WriteLine("2. Sakit");
            Console.WriteLine("3. Alfa");
            Console.Write("Status kehadiran baru (1-3): ");
            string[] statusabsen = {"Hadir", "Sakit", "Alfa"};
            int inputabsen = Convert.ToInt32(Console.ReadLine());
            string newStatus = statusabsen[inputabsen];

            if (newStatus != "Hadir" && newStatus != "Sakit" && newStatus != "Alfa")
            {
                Console.WriteLine("Status tidak valid.");
                Console.ReadLine();
                return;
            }

            // Update 
            mahasiswaToEdit_0602.Status = newStatus;

            // Save 
            _absensiController.UpdateAbsensi(mahasiswaToEdit_0602);

            Console.WriteLine("Absensi berhasil diperbarui.");
            Console.ReadLine();
        }



        private void RekapAbsensi()
        {
            // Tampilkan semua kelas
            var kelasList_0602 = _absensiController.GetAllKelas();
            if (kelasList_0602 == null || !kelasList_0602.Any())
            {
                Console.WriteLine("Tidak ada data kelas.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("=== Daftar Kelas ===");
            for (int i = 0; i < kelasList_0602.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {kelasList_0602[i].NamaKelas}");
            }

            Console.Write("Pilih kelas (masukkan angka): ");
            if (!int.TryParse(Console.ReadLine(), out int kelasIndex) || kelasIndex < 1 || kelasIndex > kelasList_0602.Count)
            {
                Console.WriteLine("Pilihan tidak valid.");
                Console.ReadLine();
                return;
            }

            // pilih id yang dipilih
            var selectedKelas_0602 = kelasList_0602[kelasIndex - 1];
            Console.WriteLine($"Anda memilih kelas: {selectedKelas_0602.NamaKelas}");

            // minta input tanggal untuk rekap absensi
            Console.Write("Masukkan tanggal (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime tanggal_0602))
            {
                Console.WriteLine("Format tanggal tidak valid.");
                Console.ReadLine();
                return;
            }

            // dapetin rekap berdsarkan absensi dan tanggal
            var rekapAbsensi_0602 = _absensiController.GetRekapAbsensiByTanggal(selectedKelas_0602.IdKelas, tanggal_0602);

            if (rekapAbsensi_0602 == null || !rekapAbsensi_0602.Any())
            {
                Console.WriteLine("Tidak ada absensi pada tanggal ini.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"=== Rekap Absensi untuk Kelas: {selectedKelas_0602.NamaKelas} pada {tanggal_0602:yyyy-MM-dd} ===");

            foreach (var absensi_0602 in rekapAbsensi_0602)
            {
                Console.WriteLine($"{absensi_0602.Mahasiswa.Nama} ({absensi_0602.Mahasiswa.NIM}) - Status: {absensi_0602.Status}");
            }

            Console.WriteLine("Tekan Enter untuk kembali ke menu.");
            Console.ReadLine();
        }


        private void Absensi()
        {
            // Tampilkan semua kelas
            var kelasList_0602 = _absensiController.GetAllKelas();
            if (kelasList_0602 == null || !kelasList_0602.Any())
            {
                Console.WriteLine("Tidak ada data kelas.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("=== Daftar Kelas ===");
            for (int i = 0; i < kelasList_0602.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {kelasList_0602[i].NamaKelas}");
            }

            Console.Write("Pilih kelas (masukkan angka): ");
            if (!int.TryParse(Console.ReadLine(), out int kelasIndex) || kelasIndex < 1 || kelasIndex > kelasList_0602.Count)
            {
                Console.WriteLine("Pilihan tidak valid.");
                Console.ReadLine();
                return;
            }

            // Daparkan id kelas yang dipilih
            var selectedKelas_0602 = kelasList_0602[kelasIndex - 1];
            Console.WriteLine($"Anda memilih kelas: {selectedKelas_0602.NamaKelas}");

            // Tampilkan mahasiswa yang udah ada
            var mahasiswaList_0602 = _absensiController.GetMahasiswaByKelas(selectedKelas_0602.IdKelas);
            if (mahasiswaList_0602 == null || !mahasiswaList_0602.Any())
            {
                Console.WriteLine("Tidak ada mahasiswa dalam kelas ini.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("=== Daftar Mahasiswa ===");

            // loop semua mahasiswa sampai exit
            foreach (var mahasiswa_0602 in mahasiswaList_0602)
            {
                Console.WriteLine($"{mahasiswa_0602.Nama} ({mahasiswa_0602.NIM})");

                // Masukkan status absensi
                Console.WriteLine("Masukkan status kehadiran! (Hadir/Sakit/Alfa): ");
                Console.WriteLine("1. Hadir");
                Console.WriteLine("2. Sakit");
                Console.WriteLine("3. Alfa");
                Console.Write("Status kehadiran(1-3): ");
                string[] statusabsen_0602 = {"Hadir", "Sakit", "Alfa"};
                int inputabsen_0602 = Convert.ToInt32(Console.ReadLine());
                string status_0602 = statusabsen_0602[inputabsen_0602];

                // Validasi input
                if (status_0602 != "Hadir" && status_0602 != "Sakit" && status_0602 != "Alfa")
                {
                    Console.WriteLine("Status tidak valid. Mengabaikan absensi untuk mahasiswa ini.");
                    continue;  // Lanjutkan ke mahasiswa berikutnya
                }

                // Buat objek absensi dan simpan
                var absensi = new Absensi
                {
                    IdMahasiswa = mahasiswa_0602.IdMahasiswa,  // Pastikan IdMahasiswa dipilih
                    Tanggal = DateTime.Now,
                    Status = status_0602,
                    IdAdmin = _idAdmin
                };

                _absensiController.InsertAbsensi(absensi);

                Console.WriteLine($"Absensi untuk {mahasiswa_0602.Nama} berhasil disimpan!");
                Console.WriteLine("Tekan Enter untuk melanjutkan ke mahasiswa berikutnya.");
                Console.ReadLine();
            }

            // Kembali ke menu utama kalo udah selesai
            Console.WriteLine("Semua absensi selesai. Tekan Enter untuk kembali ke menu.");
            Console.ReadLine();
        }
    }
}
