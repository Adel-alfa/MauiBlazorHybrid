using MauiBlazorHybrid.Api.Data;
using MauiBlazorHybrid.Api.Data.Entitties;
using MauiBlazorHybrid.SharedLibrary.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MauiBlazorHybrid.Api.Services
{
    public class StudentQuizService
    {
        private readonly AppDbContext _dbContext;
        public StudentQuizService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        public async Task<QuizListDto[]> GetActiveQuizzesAsync(int categoryId)
        {
            var Query =_dbContext.Quizzes.Where(_=>_.IsActive);
            if(categoryId > 0)
            {
                Query = Query.Where(_ => _.CategoryId == categoryId);
            }
            var quizzes = await Query.Select(q => new QuizListDto
                {
                    Id = q.Id,
                    Name = q.Name,
                    CategoryId = q.CategoryId,
                    CategoryName = q.Category.Name,
                    TimeInMinutes = q.TimeInMinutes,
                    TotalQuestions = q.TotalQuestions,

                }).ToArrayAsync();
            return quizzes;
        }

        public async Task<QuizApiResponse<int>> StartQuizAsync(int StudentId, Guid QuizId)
        {
            try
            {
                StudentQuiz studentQuiz = new()
                {

                    StudentId = StudentId,
                    QuizId = QuizId,
                    StartedOn = DateTime.UtcNow,
                };
                _dbContext.studentQuizzes.Add(studentQuiz);
                await _dbContext.SaveChangesAsync();
                return QuizApiResponse<int>.Success(studentQuiz.Id);
            }
            catch (Exception ex)
            {

                return QuizApiResponse<int>.Fail(ex.Message);
            }
        }
        public async Task<QuizApiResponse<QuestionDto>> GetNextQuestionAsync(int studentQuizId, int StudentId)
        {
            var studentQuiz = await _dbContext.studentQuizzes
                             .Include(_=>_.StudentQuizQuestions)// include does not featch the data always 0 items
                             .FirstOrDefaultAsync(_=>_.Id == studentQuizId);
           
            if (studentQuiz == null)
            {
                return QuizApiResponse<QuestionDto>.Fail("quiz does not exit!!");
            }
            if (studentQuiz.StudentId != StudentId )
            {
                return QuizApiResponse<QuestionDto>.Fail("Invalid Request!!");
            }

            //var questionsServed = studentQuiz.StudentQuizQuestions.Select(_=>_.QuestionId).ToArray(); include does not featch the data always 0 items
            var questionsServed = await _dbContext.StudentQuizQuestions.Where(_ => _.StudentQuizId == studentQuizId).Select(_ => _.QuestionId).ToArrayAsync();

            var nextQuestion = await _dbContext.Questions.Where(_=>_.QuizId == studentQuiz.QuizId)
                                .Where(_=>!questionsServed.Contains(_.Id))
                                .OrderBy(_=>Guid.NewGuid())
                                .Take(1)
                                .Select(_ => new QuestionDto
                                {
                                    Id = _.Id,
                                    Text = _.Text,
                                    Options = _.Options.Select(o=> new OptionDto
                                    {
                                        Text = o.Text,
                                        Id = o.Id,
                                    }).ToList(),
                                })
                                .FirstOrDefaultAsync();
            if(nextQuestion == null)
            return QuizApiResponse<QuestionDto>.Fail("no more questions for this quiz exit!!");

            try
            {
                var studentQuesQui = new StudentQuizQuestion
                {
                    StudentQuizId = studentQuizId,
                    QuestionId = nextQuestion.Id,
                };
                _dbContext.StudentQuizQuestions.Add(studentQuesQui);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return QuizApiResponse<QuestionDto>.Fail(ex.Message);
            }
            return QuizApiResponse<QuestionDto>.Success(nextQuestion);
        }

        public async Task<QuizApiResponse> SaveQuestionAnswerAsync(QuziQuestionAnswerDto dto, int StudentId)
        {
            var studentQuiz = await _dbContext.studentQuizzes.AsTracking()                           
                            .FirstOrDefaultAsync(_ => _.Id == dto.StudentQuizId);
            if(studentQuiz == null)
            {
                QuizApiResponse.Fail("uiz does not exit!!");
            }
            if (studentQuiz.StudentId != StudentId)
            {
                return QuizApiResponse.Fail("Invalid Request!!");
            }
            var isCorrectOption = await _dbContext.Options.Where(_=>_.QuestionId ==dto.QuestionId && _.Id ==dto.OptionId)
                                .Select(_=> _.IsCorrect).FirstOrDefaultAsync();
            if (isCorrectOption)
            {
                studentQuiz.score++;
                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    QuizApiResponse.Fail(ex.Message);
                }
            }
            return QuizApiResponse.Success();
        }
        public async Task<QuizApiResponse> SubmitQuizAsync(int studentQuizId, int StudentId)   =>    
            await HandleQuizAsync(studentQuizId, DateTime.UtcNow, nameof(QuizStatus.Completed), StudentId);
        


        public async Task<QuizApiResponse> ExitQuizAsync(int studentQuizId , int StudentId)=>
             await HandleQuizAsync(studentQuizId, null, nameof(QuizStatus.Exited), StudentId);
        

        public async Task<QuizApiResponse> AutoSubmitQuizAsync(int studentQuizId, int StudentId) =>
            await HandleQuizAsync(studentQuizId, DateTime.UtcNow, nameof(QuizStatus.AutoSubmitted), StudentId);
       
        public async Task<QuizApiResponse> HandleQuizAsync(int studentQuizId, DateTime? completedOn , string status, int StudentId)
        {
            var studentQuiz = await _dbContext.studentQuizzes.AsTracking()
                            .FirstOrDefaultAsync(_ => _.Id == studentQuizId);
            if (studentQuiz == null)
            {
                return QuizApiResponse.Fail("Quiz does not exit!!");
            }
            if (studentQuiz.StudentId != StudentId)
            {
                return QuizApiResponse.Fail("Invalid Request!!");
            }
            if (studentQuiz!.CompletedOn.HasValue || studentQuiz.Status == nameof(QuizStatus.Exited))
            {
                return QuizApiResponse.Fail("quiz already completed!!");
            }
            try
            {
                studentQuiz.CompletedOn = completedOn;
                studentQuiz.Status = status;
                var questions = await _dbContext.StudentQuizQuestions
                                   .Where(_ => _.StudentQuizId == studentQuizId)
                                   .ToListAsync();
                _dbContext.StudentQuizQuestions.RemoveRange(questions);
                await _dbContext.SaveChangesAsync();

                return QuizApiResponse.Success();
            }
            catch (Exception ex)
            {

                return QuizApiResponse.Fail(ex.Message);
            }

        }
   
        public async Task<PageResult<StudentQuizDto>> GetStudentQuizzesAsync(int studentId, int startIndex, int pageSize)
        {
            var query =  _dbContext.studentQuizzes.Where(_=>_.StudentId == studentId);
            var count = await query.CountAsync();
            var quizzes  = await query.OrderByDescending(_=>_.StartedOn)
                            .Skip(startIndex)
                            .Take(pageSize)
                            .Select(_=> new StudentQuizDto
                            {
                                Id = _.Id,
                                QuizId = _.QuizId,
                                StartedOn = _.StartedOn,    
                                QuizName =_.Quiz.Name,
                                CategoryName = _.Quiz.Category.Name,
                                Status = _.Status,
                                score = _.score,
                                CompletedOn = _.CompletedOn,
                                QusetionNo= _.Quiz.Questions.Count,
                                overallScoreStr= $"{_.score}/{_.Quiz.Questions.Count}"
                            } )
                            .ToArrayAsync();
            return new PageResult<StudentQuizDto>(quizzes, count);
        }
    }
}
