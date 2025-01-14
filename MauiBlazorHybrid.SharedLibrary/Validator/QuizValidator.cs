using System;
using FluentValidation;
using MauiBlazorHybrid.SharedLibrary.DTOs;

namespace MauiBlazorHybrid.SharedLibrary.Validator
{
   

    
    public class QuizValidator : AbstractValidator<QuizSaveDto>
    {
        public QuizValidator()
        {
            RuleFor(quiz => quiz.Questions)
                .NotEmpty().WithMessage("Please provide questions")
                .Must((quiz, questions) => questions.Count == quiz.TotalQuestions)
                .WithMessage("Total Questions count does not match with total questions!");
        }
    }

}

