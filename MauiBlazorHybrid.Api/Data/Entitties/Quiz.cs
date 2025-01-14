using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.Api.Data.Entitties
{
    public class Quiz
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } = Guid.CreateVersion7();
        [MaxLength(25)]
        public string Name { get; set; }
        public int TotalQuestions { get; set; }
        public int TimeInMinutes { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Question> Questions { get; set; } = [];
   
    }
}
