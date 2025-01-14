using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.SharedLibrary.DTOs
{
    public class QuizSaveDto
    {
        public Guid Id { get; set; } 
        [MaxLength(25)]
        public string Name { get; set; }
        public int TotalQuestions { get; set; }
        [Range(1, 90, ErrorMessage = "Please provide valid time in minutes")]
        public int TimeInMinutes { get; set; }
        [Range(1, int.MaxValue, ErrorMessage="Please provide valid number of question")]
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public List<QuestionDto> Questions { get; set; } = [];
        public string? Validator() 
        {
            if (TotalQuestions != Questions.Count)             
                return "Number of questions does not match with Total questions ";
             
            if (Questions.Any(Q => string.IsNullOrWhiteSpace(Q.Text)))          
                return " question is required! ";                
         
            if (Questions.Any(Q => Q.Options.Count < 2))           
               return "question must has at lest 2 options! ";

            if (Questions.Any(Q => !Q.Options.Any(o=>o.IsCorrect)))
                return "Pour chaque question, une option doit être sélectionnée! ";

            return null;
        }
}
}
