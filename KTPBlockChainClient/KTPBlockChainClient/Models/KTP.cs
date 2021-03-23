using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace KTPBlockChainClient.Models
{
    public class KTP
    {
        public int ID { get; set; }
        [RegularExpression(@"(^[0-9]{16}$)", ErrorMessage = "NIK harus 16 digit.")]
        [Required(ErrorMessage = "NIK harus diisi.")]
        [StringLength(16)]
        public string NIK { get; set; }
        [RegularExpression(@"^(?!\s*$)[A-Z\s]*$", ErrorMessage = "Gunakan huruf kapital [A-Z].")]
        [Required(ErrorMessage = "Nama harus diisi.")]
        [StringLength(50)]
        public string Nama { get; set; }
        [RegularExpression(@"^(?!\s*$)[A-Z\s]*$", ErrorMessage = "Gunakan huruf kapital [A-Z].")]
        [Required(ErrorMessage = "Tempat lahir harus diisi.")]
        [StringLength(50)]
        public string TempatLahir { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Tanggal lahir harus diisi.")]
        public DateTime TanggalLahir { get; set; }
        [Required]
        public string JenisKelamin { get; set; }
        [RegularExpression(@"^(?!\s*$)[A-Z\s]*$", ErrorMessage = "Gunakan huruf kapital [A-Z].")]
        [Required(ErrorMessage = "Alamat harus diisi.")]
        [StringLength(50)]
        public string Alamat { get; set; }
        [Required]
        public string Agama { get; set; }
        [Required]
        public string StatusKawin { get; set; }
        [RegularExpression(@"^(?!\s*$)[A-Z\s]*$", ErrorMessage = "Gunakan huruf kapital [A-Z].")]
        [Required(ErrorMessage = "Pekerjaan harus diisi.")]
        [StringLength(50)]
        public string Pekerjaan { get; set; }
        [RegularExpression(@"^(?!\s*$)[A-Z\s]*$", ErrorMessage = "Gunakan huruf kapital [A-Z].")]
        [Required(ErrorMessage = "Kewarganegaraan harus diisi.")]
        [StringLength(50)]
        public string Kewarganegaraan { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Berlaku hingga harus diisi.")]
        public DateTime BerlakuHingga { get; set; }
        [Required(ErrorMessage = "Foto harus diisi.")]
        public string Foto { get; set; }
        public DateTime TimeStamp { get; set; }

        public KTP(){ }
    }
}
