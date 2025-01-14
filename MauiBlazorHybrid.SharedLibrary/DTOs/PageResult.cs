using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.SharedLibrary.DTOs
{
    public record PageResult<TRecord>(TRecord[] Record, int TotalCount);
   
}
