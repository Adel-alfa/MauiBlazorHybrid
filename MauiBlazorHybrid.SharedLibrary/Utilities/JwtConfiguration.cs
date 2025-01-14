using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.SharedLibrary.Utilities
{
    public class JwtConfiguration
    {
        public string Key = "6AD2EFDE-AB2C-4841-A05E-7045C855BA22";
        public string Issuer = "https://localhost:7004";
        public string Audience = "https://localhost:7267";
        public string ExpiryInDay = "1";
    }
}
