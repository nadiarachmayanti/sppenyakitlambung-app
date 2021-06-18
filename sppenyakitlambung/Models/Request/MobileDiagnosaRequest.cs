using System;
using System.Collections.Generic;
using sppenyakitlambung.Models.SubModel;

namespace sppenyakitlambung.Models.Request
{
    public class MobileDiagnosaRequest
    {
        public string userId { get; set; }
        public List<CfUser> temp_cfuser { get; set; }
    }
}
