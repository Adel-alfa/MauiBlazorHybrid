using FluentValidation;
using MauiBlazorHybrid.Api.Data.Entitties;
using MauiBlazorHybrid.Api.Services;
using MauiBlazorHybrid.SharedLibrary.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace MauiBlazorHybrid.Api.Endpoints
{
    public static class QuizEndpoints
    {
        public static IEndpointRouteBuilder MapQuizEndpoints(this IEndpointRouteBuilder app)
        {
            var QuizGroup = app.MapGroup("/api/quizzes")
               .RequireAuthorization();
            QuizGroup.MapPost("", async (QuizSaveDto dto, QuizService quizService, IValidator<QuizSaveDto> validator) =>          
            {
               var validationResult = await validator.ValidateAsync(dto);
                if(!validationResult.IsValid)
                    return Results.BadRequest(validationResult.Errors);

                return Results.Ok(await quizService.SaveQuizAsync(dto));
               
            }).RequireAuthorization(policy => policy.RequireRole("Admin"));

            QuizGroup.MapGet("", async (QuizService service) =>
            Results.Ok(await service.GetQuizzesAsync()));

            QuizGroup.MapGet("{quizId:guid}/questions", async (Guid quizId,QuizService service) =>
            Results.Ok( await service.GetQuizQuisQuestionsAsync(quizId)));

            QuizGroup.MapGet("{quizId:guid}", async (Guid quizId, QuizService service) =>
            Results.Ok(await service.GetQuizToEditAsync(quizId)));

            
            return app;
        }

    }
}
// CategoryGroup.MapPost("", async (QuizSaveDto dto, QuizService quizService) =>
//if (dto.Questions.Count == 0)
//    return Results.BadRequest("Please Provide Quesions");
//if (dto.Questions.Count != dto.TotalQuestions)
//    return Results.BadRequest("Total Questions count does not match with total questions!");