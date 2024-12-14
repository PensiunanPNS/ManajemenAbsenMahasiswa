using AbsensiMahasiswa.Models;
using AbsensiMahasiswa.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;


namespace AbsensiMahasiswa.Controllers
{

    public class AbsensiController
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly int _idAdmin;

        public AbsensiController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        //method buat insert absen in database
public void InsertAbsensi(Absensi absensi)
{
    using (var connection = _databaseHelper.GetConnection())
    {
        try
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO absensi (id_mahasiswa, tanggal, status, id_admin) 
                                        VALUES (@id_mahasiswa, @tanggal, @status, @id_admin)";
                command.Parameters.AddWithValue("@id_mahasiswa", absensi.IdMahasiswa);
                command.Parameters.AddWithValue("@tanggal", absensi.Tanggal);
                command.Parameters.AddWithValue("@status", absensi.Status);
                command.Parameters.AddWithValue("@id_admin", absensi.IdAdmin);

                command.ExecuteNonQuery();
                Console.WriteLine("Absensi berhasil disimpan!");
            }
        }
        catch (MySqlException ex) when (ex.Number == 1452) // Foreign key constraint error
        {
            Console.WriteLine("Error: Mahasiswa atau Admin tidak ditemukan.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}



//method buat dapetin semua kelas dari db

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

        // Method dapetin semua mahasiswa dari kelas
          public List<Mahasiswa> GetMahasiswaByKelas(int idKelas)
{
    var mahasiswaList = new List<Mahasiswa>();

    using (var connection = _databaseHelper.GetConnection())
    {
        try
        {
            connection.Open();

            // Perbaikan: Ambil id_mahasiswa dari tabel mahasiswa
            string sqlGetMahasiswa = @"
                SELECT m.id_mahasiswa, m.nama, m.nim, k.kelas, m.status
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
                            IdMahasiswa = Convert.ToInt32(reader["id_mahasiswa"]), // Ambil id_mahasiswa
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


         // Method dapaetin mahasiswa berdasarkan nim nya
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

// method buat dapetin rekap absensi nya
public List<Absensi> GetRekapAbsensiByTanggal(int idKelas, DateTime tanggal)
{
    List<Absensi> absensiList = new List<Absensi>();

    using (var connection = _databaseHelper.GetConnection())
    {
        try
        {
            connection.Open();

            string sqlRekapAbsensi = @"
                SELECT a.id_mahasiswa, m.nama, m.nim, a.status
                FROM absensi a
                JOIN mahasiswa m ON a.id_mahasiswa = m.id_mahasiswa
                JOIN kelas k ON m.id_kelas = k.id_kelas
                WHERE k.id_kelas = @id_kelas AND a.tanggal = @tanggal";

            using (var cmdRekapAbsensi = new MySqlCommand(sqlRekapAbsensi, connection))
            {
                cmdRekapAbsensi.Parameters.AddWithValue("@id_kelas", idKelas);
                cmdRekapAbsensi.Parameters.AddWithValue("@tanggal", tanggal.ToString("yyyy-MM-dd"));

                using (var reader = cmdRekapAbsensi.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        absensiList.Add(new Absensi
                        {
                            IdMahasiswa = Convert.ToInt32(reader["id_mahasiswa"]),
                            Tanggal = tanggal,
                            Status = reader["status"].ToString(),
                            Mahasiswa = new Mahasiswa
                            {
                                Nama = reader["nama"].ToString(),
                                NIM = reader["nim"].ToString()
                            }
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

    return absensiList;
}

// method buat update status absen

public void UpdateAbsensi(Absensi absensi)
{
    using (var connection = _databaseHelper.GetConnection())
    {
        try
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"UPDATE absensi 
                                        SET status = @status
                                        WHERE id_mahasiswa = @id_mahasiswa AND tanggal = @tanggal";
                command.Parameters.AddWithValue("@status", absensi.Status);
                command.Parameters.AddWithValue("@id_mahasiswa", absensi.IdMahasiswa);
                command.Parameters.AddWithValue("@tanggal", absensi.Tanggal);

                command.ExecuteNonQuery();
            
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}


    }

    
}
