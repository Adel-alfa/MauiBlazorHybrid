using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.SharedLibrary
{
    public interface IPlatform
    {
        bool IsMobile { get; }
        bool IsWeb { get; }

    }
}
