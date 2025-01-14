using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.SharedLibrary.DTOs
{
    public class OptionDto
    {
        public int Id { get; set; }
        [Required, MaxLength(250)]
        public string Text { get; set; }
        public bool IsCorrect { get; set; }      


    }
}
