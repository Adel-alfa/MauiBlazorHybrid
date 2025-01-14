using MauiBlazorHybrid.SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.SharedLibrary
{
    public class QuizState
    {
        public int StudentQuizId { get; set; }
         public QuizListDto? Quiz {  get; private set; }
        public void StartQuiz(QuizListDto? quiz, int studentQuizId) => (Quiz , StudentQuizId )=(quiz, studentQuizId);
        public void StopQuiz() => (Quiz, StudentQuizId) = (null, 0);
    }

}
