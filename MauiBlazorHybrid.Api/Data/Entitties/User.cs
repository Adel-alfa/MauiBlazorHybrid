using MauiBlazorHybrid.SharedLibrary.Utilities;
using System.ComponentModel.DataAnnotations;

namespace MauiBlazorHybrid.Api.Data.Entitties
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(25)]
        public string Name { get; set; }
        [MaxLength(100), EmailAddress]
        public string Email { get; set; }
        [Length(10, 14)]
        public string Phone { get; set; }
        [MaxLength(250)]
        public string PasswordHash { get; set; }
        [MaxLength(125)]
        public string Role { get; set; } = nameof(UserRole.Client);
        public bool IsApproved { get; set; }


    }
}
