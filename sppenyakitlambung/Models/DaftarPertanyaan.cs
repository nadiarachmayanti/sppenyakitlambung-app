﻿using System;
using System.Collections.Generic;
using sppenyakitlambung.Utilities.Models;

namespace sppenyakitlambung.Models
{
    public class DaftarPertanyaan : BaseModel
    {
        public string _id { get; set; }
        public List<String> gejalaId { get; set; }
        public string pertanyaan { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
    }

}
