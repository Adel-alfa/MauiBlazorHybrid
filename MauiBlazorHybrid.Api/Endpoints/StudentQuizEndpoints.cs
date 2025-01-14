using MauiBlazorHybrid.Api.Data.Entitties;
using MauiBlazorHybrid.Api.Services;
using MauiBlazorHybrid.SharedLibrary.DTOs;
using MauiBlazorHybrid.SharedLibrary.Utilities;
using System.Security.Claims;
using System.Security.Principal;

namespace MauiBlazorHybrid.Api.Endpoints
{
    public static class StudentQuizEndpoints
    {
        public static int GetStudentId (this ClaimsPrincipal pricipal)=>
            Convert.ToInt32(pricipal.FindFirstValue(ClaimTypes.NameIdentifier));
        public static IEndpointRouteBuilder MapStudentQuizEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/students")
              .RequireAuthorization(p=> p.RequireRole(nameof(UserRole.Client)));

            group.MapGet("/available-quizzes", async (int categoryId, StudentQuizService studentQuizService) =>
                Results.Ok(await studentQuizService.GetActiveQuizzesAsync(categoryId)));

            group.MapGet("/my-quizzes", async(int startIndex, int pageSize, ClaimsPrincipal pricipal, StudentQuizService studentQuizService)=>
                 Results.Ok(await studentQuizService.GetStudentQuizzesAsync(pricipal.GetStudentId(), startIndex, pageSize)));

            //var quizGroup = app.MapGroup("/quiz");



            group.MapPost("/quiz/{quizId:guid}/start", async (Guid quizId,ClaimsPrincipal pricipal, StudentQuizService studentQuizService) =>
                Results.Ok(await studentQuizService.StartQuizAsync(pricipal.GetStudentId(), quizId)));

            group.MapGet("/quiz/{studentQuizId:int}/next-question", async (int studentQuizId, ClaimsPrincipal pricipal, StudentQuizService studentQuizService) =>
                Results.Ok(await studentQuizService.GetNextQuestionAsync(studentQuizId, pricipal.GetStudentId())));

           group.MapPost("/quiz/{studentQuizId:int}/save-answer", async (int studentQuizId, QuziQuestionAnswerDto dto, ClaimsPrincipal pricipal, StudentQuizService studentQuizService) =>
            {
                if (studentQuizId != dto.StudentQuizId)
                    return Results.Unauthorized();
                return Results.Ok(await studentQuizService.SaveQuestionAnswerAsync(dto, pricipal.GetStudentId()));
            });

           group.MapPost("/quiz/{studentQuizId:int}/submit", async (int studentQuizId, ClaimsPrincipal pricipal, StudentQuizService studentQuizService) =>
             Results.Ok(await studentQuizService.SubmitQuizAsync(studentQuizId, pricipal.GetStudentId())));

           group.MapPost("/quiz/{studentQuizId:int}/auto-submit", async (int studentQuizId, ClaimsPrincipal pricipal, StudentQuizService studentQuizService) =>
            Results.Ok(await studentQuizService.AutoSubmitQuizAsync(studentQuizId, pricipal.GetStudentId())));

           group.MapPost("/quiz/{studentQuizId:int}/exit", async (int studentQuizId, ClaimsPrincipal pricipal, StudentQuizService studentQuizService) =>
            Results.Ok(await studentQuizService.ExitQuizAsync(studentQuizId, pricipal.GetStudentId())));

            return app;
        }
    }
}
