using MySql.Data.MySqlClient;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Tls;
using System;
internal class Program
{
    
    
    private static void Main(string[] args)
    {
        string koneksi = "Server=localhost;Database=absensi_mahasiswa;User ID=root;Password=;";
        MySqlConnection conn = new MySqlConnection(koneksi);
        conn.Open();

        bool pilihmenu = false;
        while (!pilihmenu)
        {
            Console.WriteLine("Pilihan menu");
            Console.WriteLine("1. Absensi Mahasiswa");
            Console.WriteLine("2. Lihat Rekap Absensi");
            Console.WriteLine("3. Masukkan Mahasiswa");
            Console.WriteLine("4. Hapus Mahasiswa");
            Console.WriteLine("5. Hapus Rekap Absensi");
            Console.WriteLine("6. Keluar");
            Console.Write("Pilihan: ");
            int pilih = Convert.ToInt16(Console.ReadLine());

            switch (pilih)
            {
                case 1:
                {
                    InputAbsen();
                }
                break;
                case 2:
                {
                    
                }
                break;
                case 3:
                {
                    InputMahasiswa();
                }
                break;
                case 4:
                {

                }
                break;
                case 5:
                {

                }
                break;
                case 6:
                {
                    pilihmenu = true;
                }
                break;
            }

        }
    }


    public static void InputAbsen ()
    {
        Console.WriteLine("Input Absensi Mahasiswa");
    }

// Input Mahasiswa
    public static void InputMahasiswa()
    {
        string namamahasiswa = string.Empty;
        string nimmahasiswa = string.Empty;
        string kelasmahasiswa = string.Empty;

        while (true)
        {
            Console.Write("Input nama mahasiswa: ");
            namamahasiswa = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(namamahasiswa))
            {
                break; // Jika valid, keluar dari loop
            }
            Console.WriteLine("Nama mahasiswa tidak boleh kosong. Silakan input kembali.");
        }

        while (true)
        {
            Console.Write("Input NIM mahasiswa: ");
            nimmahasiswa = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nimmahasiswa) && nimmahasiswa.All(char.IsDigit))
            {
                break; // Jika valid, keluar dari loop
            }
            Console.WriteLine("NIM mahasiswa harus berupa angka dan tidak boleh kosong. Silakan input kembali.");
        }
        while (true)
        {
            Console.Write("Input kelas mahasiswa: ");
            kelasmahasiswa = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(kelasmahasiswa))
            {
                break; // Jika valid, keluar dari loop
            }
            Console.WriteLine("Kelas mahasiswa tidak boleh kosong. Silakan input kembali.");
        }

        SaveInputMahasiswa(namamahasiswa, nimmahasiswa, kelasmahasiswa);
        Console.WriteLine("Data mahasiswa berhasil disimpan!");
    }

    public static void SaveInputMahasiswa(string nama, string nim, string kelas)
    {
        string koneksi = "Server=localhost;Database=absensi_mahasiswa;User ID=root;Password=;";
        using (MySqlConnection conn = new MySqlConnection(koneksi))
        {
            conn.Open();

            // Query untuk memastikan kelas ada atau menambahkannya jika tidak ada
            string sqlInsertKelas = @"
                INSERT INTO kelas (kelas)
                SELECT @kelas
                FROM DUAL
                WHERE NOT EXISTS (SELECT 1 FROM kelas WHERE kelas = @kelas)";
            MySqlCommand cmdInsertKelas = new MySqlCommand(sqlInsertKelas, conn);
            cmdInsertKelas.Parameters.AddWithValue("@kelas", kelas);
            cmdInsertKelas.ExecuteNonQuery();

            // Query untuk mendapatkan id_kelas berdasarkan nama kelas
            string sqlGetIdKelas = "SELECT id_kelas FROM kelas WHERE kelas = @kelas";
            MySqlCommand cmdGetIdKelas = new MySqlCommand(sqlGetIdKelas, conn);
            cmdGetIdKelas.Parameters.AddWithValue("@kelas", kelas);
            int id_kelas = Convert.ToInt32(cmdGetIdKelas.ExecuteScalar());

            // Query untuk menyisipkan mahasiswa dengan id_kelas
            string sqlInsertMahasiswa = @"
                INSERT INTO mahasiswa (nama, nim, id_kelas)
                VALUES (@nama, @nim, @id_kelas)";
            MySqlCommand cmdInsertMahasiswa = new MySqlCommand(sqlInsertMahasiswa, conn);
            cmdInsertMahasiswa.Parameters.AddWithValue("@nama", nama);
            cmdInsertMahasiswa.Parameters.AddWithValue("@nim", nim);
            cmdInsertMahasiswa.Parameters.AddWithValue("@id_kelas", id_kelas);
            cmdInsertMahasiswa.ExecuteNonQuery();

            Console.WriteLine("Data mahasiswa berhasil disimpan!");
        }
    }
// End Input Mahasiswa
}