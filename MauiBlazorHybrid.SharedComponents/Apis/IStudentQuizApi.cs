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
    public interface IStudentQuizApi
    {
        [Get("/api/students/available-quizzes")]
        Task<QuizListDto[]> GetActiveQuizzesAsync(int categoryId);

        [Post("/api/students/quiz/{quizId}/start")]
        Task<QuizApiResponse<int>> StartQuizAsync(Guid quizId);

        [Get("/api/students/quiz/{studentQuizId}/next-question")]
        Task<QuizApiResponse<QuestionDto>> GetNextQuestionAsync(int studentQuizId);
        [Post("/api/students/quiz/{studentQuizId}/save-answer")]
        Task<QuizApiResponse> SaveQuestionAnswerAsync(int studentQuizId, QuziQuestionAnswerDto dto);
        [Post("/api/students/quiz/{studentQuizId}/exit")]
        Task<QuizApiResponse> ExitQuizAsync(int studentQuizId);
        [Post("/api/students/quiz/{studentQuizId}/auto-submit")]
        Task<QuizApiResponse> AutoSubmitQuizAsync(int studentQuizId);
        [Post("/api/students/quiz/{studentQuizId}/submit")]
        Task<QuizApiResponse> SubmitQuizAsync(int studentQuizId);
        
        [Get("/api/students/my-quizzes")]
        Task<PageResult<StudentQuizDto>> GetStudentQuizzesAsync( int startIndex, int pageSize);
    }
}
