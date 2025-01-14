using MauiBlazorHybrid.Api.Data.Entitties;
using MauiBlazorHybrid.SharedLibrary.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MauiBlazorHybrid.Api.Data
{
    public class AppDbContext:DbContext
    {
        private readonly IPasswordHasher<User> _passwordHasher;

       
        public AppDbContext(DbContextOptions<AppDbContext> options, IPasswordHasher<User> passwordHasher) : base(options)
        {
            _passwordHasher = passwordHasher;
        }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<StudentQuiz> studentQuizzes { get; set; }
        public DbSet<StudentQuizQuestion> StudentQuizQuestions { get; set; }

        
        public DbSet<ApplicationTokenInfo> ApplicationTokenInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
           // base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentQuizQuestion>().HasKey(x => new { x.StudentQuizId,x.QuestionId });
            modelBuilder.Entity<StudentQuizQuestion>().HasOne(_=>_.StudentQuiz).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<StudentQuizQuestion>().HasOne(_ => _.Question).WithMany().OnDelete(DeleteBehavior.NoAction);
            base.OnModelCreating(modelBuilder);

            //IdentitySeed
            User admin = new User()
            {
                Id = 1,
                Name = "Alfa One",
                Email = "admin@system.com",
                Phone = "0123456789",                
                Role = nameof(UserRole.Admin),
            };
            admin.PasswordHash = _passwordHasher.HashPassword(admin, "Password@123");
            modelBuilder.Entity<User>().HasData(admin);
            base.OnModelCreating(modelBuilder);
        }
    }
}
