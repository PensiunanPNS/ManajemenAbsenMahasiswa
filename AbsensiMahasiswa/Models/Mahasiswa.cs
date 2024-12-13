using AbsensiMahasiswa.Models;
namespace AbsensiMahasiswa.Models
{
    public class Mahasiswa
    {
        public int IdMahasiswa { get; set; }
        public string Nama { get; set; }
        public string NIM { get; set; }
        public int IdKelas { get; set; }
        public string Status { get; set; }

        // Navigation Property
        public Kelas Kelas { get; set; }
        public ICollection<Absensi> Absensi { get; set; }
    }
}
