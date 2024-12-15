using AbsensiMahasiswa.Models;
using AbsensiMahasiswa.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace AbsensiMahasiswa.Controllers
{
    public class MahasiswaController
    {
        private readonly DatabaseHelper _databaseHelper;

        // Constructor menerima DatabaseHelper sebagai dependency
        public MahasiswaController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        // Method untuk menambahkan mahasiswa
        public void InsertMahasiswa(Mahasiswa mahasiswa)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    // Insert kelas jika belum ada
                    string sqlInsertKelas = @"
                        INSERT INTO kelas (kelas)
                        SELECT @kelas
                        FROM DUAL
                        WHERE NOT EXISTS (SELECT 1 FROM kelas WHERE kelas = @kelas)";
                    using (var cmdInsertKelas = new MySqlCommand(sqlInsertKelas, connection))
                    {
                        cmdInsertKelas.Parameters.AddWithValue("@kelas", mahasiswa.Kelas.NamaKelas);
                        cmdInsertKelas.ExecuteNonQuery();
                    }

                    // Ambil id_kelas dari tabel kelas
                    string sqlGetIdKelas = "SELECT id_kelas FROM kelas WHERE kelas = @kelas";
                    int idKelas;
                    using (var cmdGetIdKelas = new MySqlCommand(sqlGetIdKelas, connection))
                    {
                        cmdGetIdKelas.Parameters.AddWithValue("@kelas", mahasiswa.Kelas.NamaKelas);
                        idKelas = Convert.ToInt32(cmdGetIdKelas.ExecuteScalar());
                    }

                    // Insert mahasiswa dengan id_kelas yang sudah didapat
                    string sqlInsertMahasiswa = @"
                        INSERT INTO mahasiswa (nama, nim, id_kelas, status)
                        VALUES (@nama, @nim, @id_kelas, 'Aktif')";
                    using (var cmdInsertMahasiswa = new MySqlCommand(sqlInsertMahasiswa, connection))
                    {
                        cmdInsertMahasiswa.Parameters.AddWithValue("@nama", mahasiswa.Nama);
                        cmdInsertMahasiswa.Parameters.AddWithValue("@nim", mahasiswa.NIM);
                        cmdInsertMahasiswa.Parameters.AddWithValue("@id_kelas", idKelas);

                        cmdInsertMahasiswa.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Terjadi kesalahan: " + ex.Message);
                }
            }
        }

        // Method untuk menghapus mahasiswa berdasarkan NIM
         public bool DeleteMahasiswa(string nim)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    string sqlUpdateStatus = "UPDATE mahasiswa SET status = 'Tidak Aktif' WHERE nim = @nim";
                    using (var cmdUpdateStatus = new MySqlCommand(sqlUpdateStatus, connection))
                    {
                        cmdUpdateStatus.Parameters.AddWithValue("@nim", nim);

                        int rowsAffected = cmdUpdateStatus.ExecuteNonQuery();
                        return rowsAffected > 0; // Return true kalo ada baris yang diperbarui
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Terjadi kesalahan: " + ex.Message);
                    return false;
                }
            }
        }

        // Method untuk mendapatkan mahasiswa berdasarkan NIM
        public Mahasiswa GetMahasiswaByNIM(string nim)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    string sqlGetMahasiswa = @"
                        SELECT m.nama, m.nim, k.kelas, m.status
                        FROM mahasiswa m
                        JOIN kelas k ON m.id_kelas = k.id_kelas
                        WHERE m.nim = @nim";
                    using (var cmdGetMahasiswa = new MySqlCommand(sqlGetMahasiswa, connection))
                    {
                        cmdGetMahasiswa.Parameters.AddWithValue("@nim", nim);

                        using (var reader = cmdGetMahasiswa.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Mahasiswa
                                {
                                    Nama = reader["nama"].ToString(),
                                    NIM = reader["nim"].ToString(),
                                    Status = reader["status"].ToString(),
                                    Kelas = new Kelas { NamaKelas = reader["kelas"].ToString() }
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Terjadi kesalahan: " + ex.Message);
                }
            }

            return null; // Return null kalo gk ditemukan
        }

        // Method untuk memperbarui mahasiswa
        public bool UpdateMahasiswa(Mahasiswa mahasiswa)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    // Update kelas 
                    string sqlInsertKelas = @"
                        INSERT INTO kelas (kelas)
                        SELECT @kelas
                        FROM DUAL
                        WHERE NOT EXISTS (SELECT 1 FROM kelas WHERE kelas = @kelas)";
                    using (var cmdInsertKelas = new MySqlCommand(sqlInsertKelas, connection))
                    {
                        cmdInsertKelas.Parameters.AddWithValue("@kelas", mahasiswa.Kelas.NamaKelas);
                        cmdInsertKelas.ExecuteNonQuery();
                    }

                    // Ambil id_kelas baru
                    string sqlGetIdKelas = "SELECT id_kelas FROM kelas WHERE kelas = @kelas";
                    int idKelas;
                    using (var cmdGetIdKelas = new MySqlCommand(sqlGetIdKelas, connection))
                    {
                        cmdGetIdKelas.Parameters.AddWithValue("@kelas", mahasiswa.Kelas.NamaKelas);
                        idKelas = Convert.ToInt32(cmdGetIdKelas.ExecuteScalar());
                    }

                    // Update mahasiswa
                    string sqlUpdateMahasiswa = @"
                        UPDATE mahasiswa
                        SET nama = @nama, id_kelas = @id_kelas
                        WHERE nim = @nim";
                    using (var cmdUpdateMahasiswa = new MySqlCommand(sqlUpdateMahasiswa, connection))
                    {
                        cmdUpdateMahasiswa.Parameters.AddWithValue("@nama", mahasiswa.Nama);
                        cmdUpdateMahasiswa.Parameters.AddWithValue("@id_kelas", idKelas);
                        cmdUpdateMahasiswa.Parameters.AddWithValue("@nim", mahasiswa.NIM);

                        int rowsAffected = cmdUpdateMahasiswa.ExecuteNonQuery();
                        return rowsAffected > 0; // Return true kalo ada baris yang diperbarui
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Terjadi kesalahan: " + ex.Message);
                    return false;
                }
            }
        }
    
     // Method  mendapatkan semua kelas
        public List<Kelas> GetAllKelas()
        {
            List<Kelas> kelasList = new List<Kelas>();
            using (var connection = _databaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    string sqlGetAllKelas = "SELECT id_kelas, kelas FROM kelas";
                    using (var cmd = new MySqlCommand(sqlGetAllKelas, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kelasList.Add(new Kelas
                            {
                                IdKelas = Convert.ToInt32(reader["id_kelas"]),
                                NamaKelas = reader["kelas"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Terjadi kesalahan: " + ex.Message);
                }
            }
            return kelasList;
        }

        // Method  mendapatkan semua mahasiswa dalam sebuah kelas
          public List<Mahasiswa> GetMahasiswaByKelas(int idKelas)
        {
            var mahasiswaList = new List<Mahasiswa>();

            using (var connection = _databaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    string sqlGetMahasiswa = @"
                        SELECT m.nama, m.nim, k.kelas, m.status
                        FROM mahasiswa m
                        JOIN kelas k ON m.id_kelas = k.id_kelas
                        WHERE m.id_kelas = @id_kelas AND m.status = 'Aktif';";

                    using (var cmdGetMahasiswa = new MySqlCommand(sqlGetMahasiswa, connection))
                    {
                        cmdGetMahasiswa.Parameters.AddWithValue("@id_kelas", idKelas);

                        using (var reader = cmdGetMahasiswa.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                mahasiswaList.Add(new Mahasiswa
                                {
                                    Nama = reader["nama"].ToString(),
                                    NIM = reader["nim"].ToString(),
                                    Status = reader["status"].ToString(),
                                    Kelas = new Kelas { NamaKelas = reader["kelas"].ToString() }
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Terjadi kesalahan: " + ex.Message);
                }
            }

            return mahasiswaList;
        }
    }
}


