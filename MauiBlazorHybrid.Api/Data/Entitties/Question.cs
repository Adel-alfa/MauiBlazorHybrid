using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.Api.Data.Entitties
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(250)]
        public string Text { get; set; }
        public Guid QuizId { get; set; }
        [ForeignKey(nameof(QuizId))]
        public virtual Quiz? Quiz { get; set; }
        public ICollection<Option> Options { get; set; } = [];
    }
}
