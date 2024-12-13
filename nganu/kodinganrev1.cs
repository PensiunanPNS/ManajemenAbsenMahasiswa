using System;
using MySql.Data.MySqlClient;

namespace StudentAttendanceSystem
{
    class Program
    {
        private static string connectionString = "Server=localhost;Database=sistem_absensi;Uid=root;Pwd=;";

        static void Main(string[] args)
{
    int choice;
    do
    {
        Console.WriteLine("\nMain Menu:");
        Console.WriteLine("1. Absensi Mahasiswa");
        Console.WriteLine("2. Rekap Absen Mahasiswa");
        Console.WriteLine("3. Input Mahasiswa");
        Console.WriteLine("4. Hapus Data Mahasiswa");
        Console.WriteLine("5. Hapus Rekap Absen");
        Console.WriteLine("6. Keluar");
        Console.Write("Masukkan pilihan: ");
        choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1:
                AbsensiMahasiswa();
                break;
            case 2:
                RekapAbsen();
                break;
            case 3:
                InputMahasiswa();
                break;
            case 4:
                DeleteMahasiswa();
                break;
            case 5:
                DeleteRekapAbsen();
                break;
            case 6:
                Console.WriteLine("Program selesai.");
                break;
            default:
                Console.WriteLine("Pilihan tidak valid.");
                break;
        }
    } while (choice != 6);
}


    static void AbsensiMahasiswa()
{
    Console.Write("Masukkan tanggal absensi (YYYY-MM-DD): ");
    string dateInput = Console.ReadLine();

    if (!DateTime.TryParseExact(dateInput, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime date))
    {
        Console.WriteLine("Format tanggal tidak valid!");
        return;
    }

    bool exit = false;
    while (!exit)
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            // Fetch Students
            string query = "SELECT * FROM students";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\nDaftar Mahasiswa:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["id"]}. {reader["name"]} - {reader["nim"]}");
                    }
                    Console.WriteLine("*. Keluar");
                }
            }

            Console.Write("\nPilih ID Mahasiswa untuk absensi (atau ketik * untuk keluar): ");
            string input = Console.ReadLine();
            if (input == "*")
            {
                exit = true;
                continue;
            }

            int studentId;
            if (!int.TryParse(input, out studentId))
            {
                Console.WriteLine("Input tidak valid!");
                continue;
            }

            Console.WriteLine("1. Hadir, 2. Sakit, 3. Alfa");
            Console.Write("Masukkan status: ");
            int statusInput = int.Parse(Console.ReadLine());
            string status = statusInput switch
            {
                1 => "Hadir",
                2 => "Sakit",
                3 => "Alfa",
                _ => throw new Exception("Pilihan status tidak valid.")
            };

            string insertQuery = "INSERT INTO attendance (date, student_id, status) VALUES (@date, @student_id, @status)";
            using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@student_id", studentId);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Absensi berhasil disimpan.");
            }
        }
    }
}




        static void RekapAbsen()
{
    Console.Write("Masukkan tanggal (YYYY-MM-DD): ");
    string dateInput = Console.ReadLine();

    if (!DateTime.TryParseExact(dateInput, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime date))
    {
        Console.WriteLine("Format tanggal tidak valid!");
        return;
    }

    using (MySqlConnection conn = new MySqlConnection(connectionString))
    {
        conn.Open();
        string query = @"
            SELECT s.name, s.nim, a.status 
            FROM attendance a 
            JOIN students s ON a.student_id = s.id 
            WHERE a.date = @date";

        using (MySqlCommand cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\nRekap Absen:");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["name"]} - {reader["nim"]} - {reader["status"]}");
                }
            }
        }
    }
}


        static void InputMahasiswa()
{
    bool exit = false;

    while (!exit)
    {
        Console.WriteLine("\nTambah Mahasiswa:");
        Console.WriteLine("1. Input Data");
        Console.WriteLine("*. Keluar");
        Console.Write("Pilih opsi: ");
        string input = Console.ReadLine();

        if (input == "*")
        {
            exit = true;
            continue;
        }

        Console.Write("Masukkan nama mahasiswa: ");
        string name = Console.ReadLine();
        Console.Write("Masukkan NIM mahasiswa: ");
        string nim = Console.ReadLine();

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();
            string query = "INSERT INTO students (name, nim) VALUES (@name, @nim)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@nim", nim);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Mahasiswa berhasil ditambahkan.");
            }
        }
    }
}



       static void DeleteMahasiswa()
{
    Console.Write("Masukkan ID mahasiswa yang akan dihapus: ");
    int studentId = int.Parse(Console.ReadLine());

    using (MySqlConnection conn = new MySqlConnection(connectionString))
    {
        conn.Open();

        // Delete the student
        string deleteQuery = "DELETE FROM students WHERE id = @id";
        using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn))
        {
            cmd.Parameters.AddWithValue("@id", studentId);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Mahasiswa berhasil dihapus.");
        }

        // Reassign IDs in `students` and reset AUTO_INCREMENT
        string reassignIds = @"
            SET @row_number = 0;
            UPDATE students SET id = (@row_number := @row_number + 1);
            ALTER TABLE students AUTO_INCREMENT = 1;";
        using (MySqlCommand cmd = new MySqlCommand(reassignIds, conn))
        {
            cmd.ExecuteNonQuery();
            Console.WriteLine("ID mahasiswa telah diurutkan ulang.");
        }
    }
}




       static void DeleteRekapAbsen()
{
    Console.Write("Masukkan tanggal (YYYY-MM-DD) yang akan dihapus: ");
    string dateInput = Console.ReadLine();

    if (!DateTime.TryParseExact(dateInput, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime date))
    {
        Console.WriteLine("Format tanggal tidak valid!");
        return;
    }

    using (MySqlConnection conn = new MySqlConnection(connectionString))
    {
        conn.Open();
        string query = "DELETE FROM attendance WHERE date = @date";
        using (MySqlCommand cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Rekap absen berhasil dihapus.");
        }
    }
}

}
}