using System;
using System.Collections.Generic;

namespace sppenyakitlambung.Models
{
    public class BasisPengetahuan
    { 
        public string _id { get; set; }
        public string penyakitId { get; set; }
        public List<DaftarGejala> daftar_gejala { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
    }
}
