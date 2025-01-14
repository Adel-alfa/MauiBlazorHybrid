using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.Api.Data.Entitties
{
    public class StudentQuiz
    {
        [Key]
        public int Id { get; set; }
        public Guid QuizId { get; set; }
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual User? Student { get; set; }
        [ForeignKey("QuizId")]
        public virtual Quiz? Quiz { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public int score { get; set; }
        [AllowedValues(nameof(QuizStatus.Started),
            nameof(QuizStatus.Completed),
            nameof(QuizStatus.AutoSubmitted),
            nameof(QuizStatus.Exited))]
        public string Status { get; set; } = nameof(QuizStatus.Started);
        public virtual ICollection<StudentQuizQuestion> StudentQuizQuestions { get; set; } = [];
    }
}
