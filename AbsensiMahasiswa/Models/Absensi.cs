using AbsensiMahasiswa.Models;
namespace AbsensiMahasiswa.Models
{
    public class Absensi
    {
        public int IdAbsensi { get; set; }
        public int IdMahasiswa { get; set; }
        public int IdAdmin { get; set; }
        public DateTime Tanggal { get; set; }
        public string Status { get; set; } // "Hadir", "Sakit", "Alfa"

        // Navigation Properties
        public Mahasiswa Mahasiswa { get; set; }
        public Admin Admin { get; set; }
    }
}
