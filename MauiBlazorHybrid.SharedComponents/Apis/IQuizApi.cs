using MauiBlazorHybrid.SharedLibrary.DTOs;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.SharedComponents.Apis
{
    [Headers("Authorization: Bearer ")]
    public interface IQuizApi
    {
        [Post("/api/quizzes")]
        Task<QuizApiResponse> SaveQuizAsync(QuizSaveDto category);
        [Get("/api/quizzes")]
        Task<QuizListDto[]> GetQuizzesAsync();
        [Get("/api/quizzes/{quizId}/questions")]
        Task<QuestionDto[]> GetQuizQuisQuestionsAsync(Guid quizId);
        [Get("/api/quizzes/{quizId}")]
        Task<QuizSaveDto?> GetQuizToEditAsync(Guid quizId);
    }
}
