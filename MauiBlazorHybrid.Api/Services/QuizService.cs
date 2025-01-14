using MauiBlazorHybrid.Api.Data;
using MauiBlazorHybrid.Api.Data.Entitties;
using MauiBlazorHybrid.SharedLibrary.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MauiBlazorHybrid.Api.Services
{
    public class QuizService
    {
        private readonly AppDbContext _dbContext;

        public QuizService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<QuizApiResponse> SaveQuizAsync(QuizSaveDto quizDto)
        {

            var questions = quizDto.Questions.Select(q => new Question
            {
                Id = q.Id,
                Text = q.Text,
                Options = q.Options.Select(o => new Option
                {
                    Id = o.Id,
                    Text = o.Text,
                    IsCorrect = o.IsCorrect,
                }).ToList(),
            }).ToList();
            if (quizDto.Id == Guid.Empty)
            {

                Quiz Quize = new Quiz
                {
                    Name = quizDto.Name,
                    CategoryId = quizDto.CategoryId,
                    IsActive = quizDto.IsActive,
                    TimeInMinutes = quizDto.TimeInMinutes,
                    TotalQuestions = quizDto.TotalQuestions,
                    Questions = questions
                };
                _dbContext.Quizzes.Add(Quize);
            }
            else
            {
                var dbQuize = _dbContext.Quizzes.Include(q => q.Questions).ThenInclude(q => q.Options).FirstOrDefault(q => q.Id == quizDto.Id);
               
            
                //var dbQuize = _dbContext.Quizzes.FirstOrDefault(_ => _.Id == quizDto.Id);
                if (dbQuize == null)
                {
                    return QuizApiResponse.Fail("quize does not exists");

                }
                else
                {
                    foreach (var dbQuestion in dbQuize.Questions)
                    {
                        var dbOptions = dbQuestion.Options.Select(q => q ).ToList();
                        var updatedQuestion = questions.FirstOrDefault(q=> q.Id == dbQuestion.Id);
                        if (updatedQuestion != null)
                        {
                            foreach (var item in dbOptions)
                            {
                                var currentOption = updatedQuestion.Options.FirstOrDefault(o => o.Id == item.Id);
                                if (currentOption == null)
                                    _dbContext.Options.Remove(item);
                            }
                        }
                        
                    }
                    dbQuize.CategoryId = quizDto.CategoryId;
                    dbQuize.IsActive = quizDto.IsActive;
                    dbQuize.TimeInMinutes = quizDto.TimeInMinutes;
                    dbQuize.TotalQuestions = quizDto.TotalQuestions;
                    dbQuize.Name = quizDto.Name;
                    dbQuize.Questions = questions;


                }
            }
            try
            {
                await _dbContext.SaveChangesAsync();
                return QuizApiResponse.Success();
            }
            catch (Exception ex)
            {

                return QuizApiResponse.Fail(ex.Message);
            }
        }


        public async Task<QuizListDto[]> GetQuizzesAsync()
        {
            return await _dbContext.Quizzes.Select(q=> new QuizListDto
            {
                Id = q.Id,
                Name = q.Name,
                TimeInMinutes = q.TimeInMinutes,
                TotalQuestions = q.TotalQuestions,
                IsActive = q.IsActive,
                CategoryId = q.CategoryId,
                CategoryName= q.Category.Name
            }).ToArrayAsync();
        }
        public async Task<QuestionDto[]> GetQuizQuisQuestionsAsync(Guid quizId)=>
            await _dbContext.Questions.Where(q=> q.QuizId == quizId).Select(q=> new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
            }).ToArrayAsync();
        public async Task<QuizSaveDto?> GetQuizToEditAsync(Guid quizId)
        {
            var quiz = await _dbContext.Quizzes.Where(_=>_.Id == quizId).Select(q=> new QuizSaveDto
            {
                Id = q.Id,
                CategoryId=q.CategoryId,
                IsActive=q.IsActive,
                Name = q.Name,
                TimeInMinutes=q.TimeInMinutes,
                TotalQuestions=q.TotalQuestions,
                Questions = q.Questions.Select(qu=>new QuestionDto
                {
                    Id = qu.Id,
                    Text = qu.Text,
                    Options= qu.Options.Select(o=> new OptionDto
                    {
                        Text = o.Text,
                        Id=o.Id,
                        IsCorrect=o.IsCorrect,
                    }).ToList()
                }).ToList()
            }).FirstOrDefaultAsync();
            return quiz;
        }
    }
}
