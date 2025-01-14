using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MauiBlazorHybrid.Api.Data.Entitties
{
    public class StudentQuizQuestion
    {
        [Key]
        public int StudentQuizId { get; set; }
        
        public int QuestionId { get; set; }
        [ForeignKey("StudentQuizId")]
        public virtual StudentQuiz? StudentQuiz { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question? Question { get; set; }



    }
}
