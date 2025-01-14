using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.SharedLibrary.DTOs
{
    public class RegisterDto
    {
        [Required, MaxLength(25)]
        public string Name { get; set; }
        [Required, EmailAddress , DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Length(10, 14)]
        public string Phone { get; set; }

        [Required,MaxLength(250), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required,  DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
