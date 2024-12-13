using AbsensiMahasiswa.Models;
namespace AbsensiMahasiswa.Models
{
    public class Kelas
    {
        public int IdKelas { get; set; }
        public string NamaKelas { get; set; }

        // Navigation Property
        public ICollection<Mahasiswa> Mahasiswas { get; set; }
    }
}
