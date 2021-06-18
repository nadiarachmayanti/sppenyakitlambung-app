using System;
namespace sppenyakitlambung.Models
{
    public class UserHistoryDiagnosa
    {
        public string _id { get; set; }
        public string penyakitId { get; set; }
        public string hasilnilai { get; set; }
        public UserDiagnosa konsultasiId { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }

    }
}
