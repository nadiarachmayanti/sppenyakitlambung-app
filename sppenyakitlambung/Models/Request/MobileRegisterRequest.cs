﻿using System;
namespace sppenyakitlambung.Utilities.Models
{
    public class MobileRegisterRequest
    {
        public string fullname { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string gender { get; set; }
        public int umur { get; set; }
        public string phone_number { get; set; }
        public string address { get; set; }
        public string status { get; set; }
    }
}
