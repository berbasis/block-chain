using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace KTPBlockChain.Models
{
    public class KTP
    {
        public int ID { get; set; }
        [RegularExpression(@"(^[0-9]{16}$)", ErrorMessage = "NIK harus 16 digit.")]
        [Required]
        [StringLength(16)]
        public string NIK { get; set; }
        [RegularExpression(@"^(?!\s*$)[A-Z\s]*$", ErrorMessage = "Gunakan huruf kapital [A-Z].")]
        [Required]
        [StringLength(50)]
        public string Nama { get; set; }
        [RegularExpression(@"^(?!\s*$)[A-Z\s]*$", ErrorMessage = "Gunakan huruf kapital [A-Z].")]
        [Required]
        [StringLength(50)]
        public string TempatLahir { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TanggalLahir { get; set; }
        [Required]
        public string JenisKelamin { get; set; }
        [RegularExpression(@"^(?!\s*$)[A-Z\s]*$", ErrorMessage = "Gunakan huruf kapital [A-Z].")]
        [Required]
        [StringLength(50)]
        public string Alamat { get; set; }
        [Required]
        public string Agama { get; set; }
        [Required]
        public string StatusKawin { get; set; }
        [RegularExpression(@"^(?!\s*$)[A-Z\s]*$", ErrorMessage = "Gunakan huruf kapital [A-Z].")]
        [Required]
        [StringLength(50)]
        public string Pekerjaan { get; set; }
        [RegularExpression(@"^(?!\s*$)[A-Z\s]*$", ErrorMessage = "Gunakan huruf kapital [A-Z].")]
        [Required]
        [StringLength(50)]
        public string Kewarganegaraan { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BerlakuHingga { get; set; }
        [RegularExpression(@"^(?!\s*$)[A-Z\s]*$", ErrorMessage = "Gunakan huruf kapital [A-Z].")]
        [Required]
        [StringLength(50)]
        public string Foto { get; set; }
        public DateTime TimeStamp { get; set; }
        public const string KTPSalt1 = "really blockchains my ktps";
        public const string KTPSalt2 = "really ktps my blockchains";

        public KTP() { }

        public KTP(int id, string nIK, string nama, string tempatLahir, DateTime tanggalLahir, string jenisKelamin, string alamat, string agama, string statusKawin, string pekerjaan, string kewarganegaraan, DateTime berlakuHingga, string foto, DateTime timeStamp)
        {
            ID = id;
            NIK = nIK;
            Nama = nama;
            TempatLahir = tempatLahir;
            TanggalLahir = tanggalLahir;
            JenisKelamin = jenisKelamin;
            Alamat = alamat;
            Agama = agama;
            StatusKawin = statusKawin;
            Pekerjaan = pekerjaan;
            Kewarganegaraan = kewarganegaraan;
            BerlakuHingga = berlakuHingga;
            Foto = foto;
            TimeStamp = timeStamp;
        }

        public string CreateHash()
        {
            var sha256 = new SHA256Managed();
            string nik = NIK.Substring(NIK.Length - 2);

            if (Convert.ToInt32(nik) % 2 == 1)
            {
                byte[] bytes = Encoding.ASCII.GetBytes($"{ID}-{NIK}-{Nama}-{TempatLahir}-{TanggalLahir}-{JenisKelamin}-{TempatLahir}-{Alamat}-{Agama}-{StatusKawin}-{Pekerjaan}-{Kewarganegaraan}-{BerlakuHingga}-{Foto}-{KTPSalt1}");
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
            else
            {
                byte[] bytes = Encoding.ASCII.GetBytes($"{ID}-{NIK}-{Nama}-{TempatLahir}-{TanggalLahir}-{JenisKelamin}-{TempatLahir}-{Alamat}-{Agama}-{StatusKawin}-{Pekerjaan}-{Kewarganegaraan}-{BerlakuHingga}-{Foto}-{KTPSalt2}");
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }

        }

    }

}
