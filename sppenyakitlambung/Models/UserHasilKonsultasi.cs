using System;
using System.Collections.Generic;
using sppenyakitlambung.Models.SubModel;

namespace sppenyakitlambung.Models
{
    public class UserHasilKonsultasi
    {
        public Pengguna user { get; set; }
        //public List<DataCfUser> gejalaUser { get; set; }
        public HasilPerhitungan hasil_konsultasi { get; set; }
    }

    public class HasilPerhitungan
    {
        public Penyakit penyakit { get; set; }
        public string hasil { get; set; }
    }

    public class DataCfUser
    {
        public DaftarPertanyaan pertanyaanId { get; set; }
        public KondisiUser kondisiuserId { get; set; }
    }
}