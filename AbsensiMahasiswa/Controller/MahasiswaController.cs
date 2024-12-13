using AbsensiMahasiswa.Models;
using AbsensiMahasiswa.Utils;
using MySql.Data.MySqlClient;
using System;

namespace AbsensiMahasiswa.Controllers
{
    public class MahasiswaController
    {
        private DatabaseHelper _databaseHelper;

        public MahasiswaController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

       //Method insert mahasiswa
        public void InsertMahasiswa(Mahasiswa mahasiswa)
        {
            string koneksi = "Server=localhost;Database=absensi_mahasiswa;User ID=root;Password=;";
            
            using (MySqlConnection conn = new MySqlConnection(koneksi))
            {
                conn.Open();

                
                string sqlInsertKelas = @"
                    INSERT INTO kelas (kelas)
                    SELECT @kelas
                    FROM DUAL
                    WHERE NOT EXISTS (SELECT 1 FROM kelas WHERE kelas = @kelas)";
                MySqlCommand cmdInsertKelas = new MySqlCommand(sqlInsertKelas, conn);
                cmdInsertKelas.Parameters.AddWithValue("@kelas", mahasiswa.Kelas.NamaKelas);
                cmdInsertKelas.ExecuteNonQuery();

                string sqlGetIdKelas = "SELECT id_kelas FROM kelas WHERE kelas = @kelas";
                MySqlCommand cmdGetIdKelas = new MySqlCommand(sqlGetIdKelas, conn);
                cmdGetIdKelas.Parameters.AddWithValue("@kelas", mahasiswa.Kelas.NamaKelas);
                int idKelas = Convert.ToInt32(cmdGetIdKelas.ExecuteScalar());

               
                string sqlInsertMahasiswa = @"
                    INSERT INTO mahasiswa (nama, nim, id_kelas, status)
                    VALUES (@nama, @nim, @id_kelas, 'Aktif')";
                MySqlCommand cmdInsertMahasiswa = new MySqlCommand(sqlInsertMahasiswa, conn);
                cmdInsertMahasiswa.Parameters.AddWithValue("@nama", mahasiswa.Nama);
                cmdInsertMahasiswa.Parameters.AddWithValue("@nim", mahasiswa.NIM);
                cmdInsertMahasiswa.Parameters.AddWithValue("@id_kelas", idKelas);
                
                int rowsAffected = cmdInsertMahasiswa.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Mahasiswa berhasil ditambahkan.");
                }
                else
                {
                    Console.WriteLine("Gagal menambahkan mahasiswa.");
                }
            }
        }
    }
}
