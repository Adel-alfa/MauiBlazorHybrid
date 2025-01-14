using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.SharedLibrary.DTOs
{
    public class AdminQuizStudentListDto
    {
        public string QuizName { get; set; }
        public string CategoryName { get; set; }
        public PageResult<AdminQuizStudentDto> students { get; set; } 
    }

    public class AdminQuizStudentDto
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public int Score { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
    }
}
