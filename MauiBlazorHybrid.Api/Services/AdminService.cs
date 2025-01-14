using MauiBlazorHybrid.Api.Data;
using MauiBlazorHybrid.Api.Data.Entitties;
using MauiBlazorHybrid.SharedLibrary.DTOs;
using MauiBlazorHybrid.SharedLibrary.Utilities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MauiBlazorHybrid.Api.Services
{
    public class AdminService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        public AdminService(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }
        
        public async Task<PageResult<UserDto>> GetUserAsync(UserApprovedFilter approvedType, int startIndex, int pageSize)
        {
            using var db = _dbFactory.CreateDbContext(); 
            var query = db.Users.Where(_=>_.Role != nameof(UserRole.Admin)).AsQueryable();
            if (approvedType != UserApprovedFilter.Tous)
            {
                if (approvedType == UserApprovedFilter.Approuvé_Seulement)
                query = query.Where(_=>_.IsApproved);
                else
                    query = query.Where(_=>!_.IsApproved);
            }
            var total = await query.CountAsync();
            var users = await query.OrderByDescending(u=>u.Id).Skip(startIndex).Take(pageSize)
                .Select(u=> new UserDto(u.Id, u.Name, u.Email,u.Phone,u.IsApproved)).ToArrayAsync();
            
            return new PageResult<UserDto> ( users, total );
        }

        public async Task ToggleUserApprovedStatus(int studentId)
        {
            using var dbContext = _dbFactory.CreateDbContext(); 

            var dbUser = await dbContext.Users.FirstOrDefaultAsync(_=>_.Id == studentId); 
            if (dbUser != null)            
              {
                  dbUser.IsApproved = !dbUser.IsApproved;
                  await dbContext.SaveChangesAsync();
              }
        }
        
        public async Task<AdminDashboardDto> GetHomeStatisticDataAsync()
        {
           
            var TotalCategoriesTask =   _dbFactory.CreateDbContext().Categories.CountAsync();
            var TotalQuizzesTask =  _dbFactory.CreateDbContext().Quizzes.CountAsync();
            var ActiveQuizzesTask =  _dbFactory.CreateDbContext().Quizzes.Where(_=> _.IsActive).CountAsync();
            var TotalStudentsTask =  _dbFactory.CreateDbContext().Users.Where(_=> _.Role.Contains("Client")).CountAsync();
            var ApprovedStudentsTask =  _dbFactory.CreateDbContext().Users.Where(_ => _.Role.Contains("Client")&& _.IsApproved).CountAsync();

            var TotalCategories = await TotalCategoriesTask;
            var TotalQuizzes = await TotalQuizzesTask;
            var ActiveQuizzes = await ActiveQuizzesTask;
            var TotalStudents = await TotalStudentsTask;
            var ApprovedStudents = await ApprovedStudentsTask;
            return new AdminDashboardDto(TotalCategories, TotalStudents,ApprovedStudents, TotalQuizzes, ActiveQuizzes);
        }
    
        public async Task<AdminQuizStudentListDto> GetQuizStudentsAsync(Guid quizId , int startIndex, int pageSize, bool fetchQuizInfo)
        {
            var Result = new AdminQuizStudentListDto();
            using var Context = _dbFactory.CreateDbContext();
            if (fetchQuizInfo)
            {
                var quizInfo = await Context.Quizzes
                                .Where(_ => _.Id == quizId)
                                .Select(_ => new
                                {
                                    QuizName = _.Name,
                                    CategoryName = _.Category.Name,
                                })
                                .FirstOrDefaultAsync();

                if (quizInfo == null)
                {
                    Result.students = new PageResult<AdminQuizStudentDto>([], 0);
                    return Result;
                }
                Result.QuizName = quizInfo.QuizName;
                Result.CategoryName = quizInfo.CategoryName;
                var query = Context.studentQuizzes.Where(_ => _.QuizId == quizId);

                var count = await query.CountAsync();

                var students = await query.OrderByDescending(_ => _.StartedOn)
                                .Skip(startIndex)
                                .Take(pageSize)
                                .Select(_ => new AdminQuizStudentDto
                                {
                                    CompletedOn = DateTime.Now,
                                    Name = _.Student.Name,
                                    StartedOn = DateTime.Now,
                                    Score = _.score,
                                    Status = _.Status,
                                }).ToArrayAsync();

                var pageStudents = new PageResult<AdminQuizStudentDto>(students, count);
                Result.students = pageStudents;
            }           
            
            return Result;
        }
    }
}
