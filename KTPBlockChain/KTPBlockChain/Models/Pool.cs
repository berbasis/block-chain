using System;
using System.ComponentModel.DataAnnotations;

namespace KTPBlockChain.Models
{
    public class Pool
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
        [Required]
        public string Foto { get; set; }
        public DateTime TimeStamp { get; set; }

    }

}
