using MauiBlazorHybrid.SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.Mobile.Service
{
    internal class MobilePlatform : IPlatform
    {
        public bool IsMobile => true;

        public bool IsWeb => false;
    }
}
