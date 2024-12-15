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
    bool exit = false;
    while (!exit)
    {
        Console.Clear();
        Console.WriteLine("=== Menu Absensi ===");
        Console.WriteLine("1. Lakukan Absensi");
        Console.WriteLine("2. Rekap Absensi Berdasarkan Tanggal");
        Console.WriteLine("3. Edit Absensi");
        Console.WriteLine("4. Kembali");
        Console.Write("Pilih menu: ");

        string pilihan = Console.ReadLine();

        switch (pilihan)
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
                exit = true;
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
    var kelasList = _absensiController.GetAllKelas();
    if (kelasList == null || !kelasList.Any())
    {
        Console.WriteLine("Tidak ada data kelas.");
        Console.ReadLine();
        return;
    }

    Console.WriteLine("=== Daftar Kelas ===");
    for (int i = 0; i < kelasList.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {kelasList[i].NamaKelas}");
    }

    Console.Write("Pilih kelas (masukkan angka): ");
    if (!int.TryParse(Console.ReadLine(), out int kelasIndex) || kelasIndex < 1 || kelasIndex > kelasList.Count)
    {
        Console.WriteLine("Pilihan tidak valid.");
        Console.ReadLine();
        return;
    }

    // Dapetin id kelas yang dipilih
    var selectedKelas = kelasList[kelasIndex - 1];
    Console.WriteLine($"Anda memilih kelas: {selectedKelas.NamaKelas}");

    // Input tanggal yang dimau
    Console.Write("Masukkan tanggal (yyyy-MM-dd): ");
    if (!DateTime.TryParse(Console.ReadLine(), out DateTime tanggal))
    {
        Console.WriteLine("Format tanggal tidak valid.");
        Console.ReadLine();
        return;
    }

    // tampilin rekap absen yang ada
    var rekapAbsensi = _absensiController.GetRekapAbsensiByTanggal(selectedKelas.IdKelas, tanggal);

    if (rekapAbsensi == null || !rekapAbsensi.Any())
    {
        Console.WriteLine("Tidak ada absensi pada tanggal ini.");
        Console.ReadLine();
        return;
    }

    Console.WriteLine($"=== Rekap Absensi untuk Kelas: {selectedKelas.NamaKelas} pada {tanggal:yyyy-MM-dd} ===");

    // Displat list absensi dari mahasiswa
    for (int i = 0; i < rekapAbsensi.Count; i++)
    {
        var absensi = rekapAbsensi[i];
        Console.WriteLine($"{i + 1}. {absensi.Mahasiswa.Nama} ({absensi.Mahasiswa.NIM}) - Status: {absensi.Status}");
    }

    // pilih ahasiswa ynag mau diedit
    Console.Write("Pilih mahasiswa yang ingin diedit (masukkan angka): ");
    if (!int.TryParse(Console.ReadLine(), out int studentIndex) || studentIndex < 1 || studentIndex > rekapAbsensi.Count)
    {
        Console.WriteLine("Pilihan tidak valid.");
        Console.ReadLine();
        return;
    }

    var mahasiswaToEdit = rekapAbsensi[studentIndex - 1];
    Console.WriteLine($"Absensi yang ada untuk {mahasiswaToEdit.Mahasiswa.Nama}: {mahasiswaToEdit.Status}");

    // Edit absensi
    Console.Write("Masukkan status baru (Hadir/Sakit/Alfa): ");
    string newStatus = Console.ReadLine();

    if (newStatus != "Hadir" && newStatus != "Sakit" && newStatus != "Alfa")
    {
        Console.WriteLine("Status tidak valid.");
        Console.ReadLine();
        return;
    }

    // Update 
    mahasiswaToEdit.Status = newStatus;

    // Save 
    _absensiController.UpdateAbsensi(mahasiswaToEdit);

    Console.WriteLine("Absensi berhasil diperbarui.");
    Console.ReadLine();
}



private void RekapAbsensi()
{
    // Tampilkan semua kelas
    var kelasList = _absensiController.GetAllKelas();
    if (kelasList == null || !kelasList.Any())
    {
        Console.WriteLine("Tidak ada data kelas.");
        Console.ReadLine();
        return;
    }

    Console.WriteLine("=== Daftar Kelas ===");
    for (int i = 0; i < kelasList.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {kelasList[i].NamaKelas}");
    }

    Console.Write("Pilih kelas (masukkan angka): ");
    if (!int.TryParse(Console.ReadLine(), out int kelasIndex) || kelasIndex < 1 || kelasIndex > kelasList.Count)
    {
        Console.WriteLine("Pilihan tidak valid.");
        Console.ReadLine();
        return;
    }

    // pilih id yang dipilih
    var selectedKelas = kelasList[kelasIndex - 1];
    Console.WriteLine($"Anda memilih kelas: {selectedKelas.NamaKelas}");

    // minta input tanggal untuk rekap absensi
    Console.Write("Masukkan tanggal (yyyy-MM-dd): ");
    if (!DateTime.TryParse(Console.ReadLine(), out DateTime tanggal))
    {
        Console.WriteLine("Format tanggal tidak valid.");
        Console.ReadLine();
        return;
    }

    // dapetin rekap berdsarkan absensi dan tanggal
    var rekapAbsensi = _absensiController.GetRekapAbsensiByTanggal(selectedKelas.IdKelas, tanggal);

    if (rekapAbsensi == null || !rekapAbsensi.Any())
    {
        Console.WriteLine("Tidak ada absensi pada tanggal ini.");
        Console.ReadLine();
        return;
    }

    Console.WriteLine($"=== Rekap Absensi untuk Kelas: {selectedKelas.NamaKelas} pada {tanggal:yyyy-MM-dd} ===");

    foreach (var absensi in rekapAbsensi)
    {
        Console.WriteLine($"{absensi.Mahasiswa.Nama} ({absensi.Mahasiswa.NIM}) - Status: {absensi.Status}");
    }

    Console.WriteLine("Tekan Enter untuk kembali ke menu.");
    Console.ReadLine();
}
        private void Absensi()
        {
            // Tampilkan semua kelas
            var kelasList = _absensiController.GetAllKelas();
            if (kelasList == null || !kelasList.Any())
            {
                Console.WriteLine("Tidak ada data kelas.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("=== Daftar Kelas ===");
            for (int i = 0; i < kelasList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {kelasList[i].NamaKelas}");
            }

            Console.Write("Pilih kelas (masukkan angka): ");
            if (!int.TryParse(Console.ReadLine(), out int kelasIndex) || kelasIndex < 1 || kelasIndex > kelasList.Count)
            {
                Console.WriteLine("Pilihan tidak valid.");
                Console.ReadLine();
                return;
            }

            // Daparkan id kelas yang dipilih
            var selectedKelas = kelasList[kelasIndex - 1];
            Console.WriteLine($"Anda memilih kelas: {selectedKelas.NamaKelas}");

            // Tampilkan mahasiswa yang udah ada
            var mahasiswaList = _absensiController.GetMahasiswaByKelas(selectedKelas.IdKelas);
            if (mahasiswaList == null || !mahasiswaList.Any())
            {
                Console.WriteLine("Tidak ada mahasiswa dalam kelas ini.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("=== Daftar Mahasiswa ===");

            // loop semua mahasiswa sampai exit
            foreach (var mahasiswa in mahasiswaList)
            {
                Console.WriteLine($"{mahasiswa.Nama} ({mahasiswa.NIM})");

                // Masukkan status absensi
                Console.Write("Masukkan status kehadiran (Hadir/Sakit/Alfa): ");
                string status = Console.ReadLine();

                // Validasi input
                if (status != "Hadir" && status != "Sakit" && status != "Alfa")
                {
                    Console.WriteLine("Status tidak valid. Mengabaikan absensi untuk mahasiswa ini.");
                    continue;  // Lanjutkan ke mahasiswa berikutnya
                }

                // Buat objek absensi dan simpan
                var absensi = new Absensi
                {
                    IdMahasiswa = mahasiswa.IdMahasiswa,  // Pastikan IdMahasiswa dipilih
                    Tanggal = DateTime.Now,
                    Status = status,
                    IdAdmin = _idAdmin
                };

                _absensiController.InsertAbsensi(absensi);

                Console.WriteLine($"Absensi untuk {mahasiswa.Nama} berhasil disimpan!");
                Console.WriteLine("Tekan Enter untuk melanjutkan ke mahasiswa berikutnya.");
                Console.ReadLine();
            }

            // Kembali ke menu utama kalo udah selesai
            Console.WriteLine("Semua absensi selesai. Tekan Enter untuk kembali ke menu.");
            Console.ReadLine();
        }
    }
}
