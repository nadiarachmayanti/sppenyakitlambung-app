using System;
using System.Collections.Generic;
using sppenyakitlambung.Models.SubModel;

namespace sppenyakitlambung.Models
{
    public class UserDiagnosa
    {
        public string _id { get; set; }
        public string userId { get; set; }
        public List<CfUser> temp_cfuser { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
    }
}
