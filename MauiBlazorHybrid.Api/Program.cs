
using FluentValidation;
using MauiBlazorHybrid.Api.Data;
using MauiBlazorHybrid.Api.Data.Entitties;
using MauiBlazorHybrid.Api.Endpoints;
using MauiBlazorHybrid.Api.Services;
using MauiBlazorHybrid.SharedLibrary.DTOs;
using MauiBlazorHybrid.SharedLibrary.Utilities;
using MauiBlazorHybrid.SharedLibrary.Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MauiBlazorHybrid.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOpenApi();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
            var connection = builder.Configuration.GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection),optionsLifetime: ServiceLifetime.Singleton);

            builder.Services.AddDbContextFactory<AppDbContext>(options =>
            {
                options.UseSqlServer(connection);
            });

            
            builder.Services.AddTransient<AuthService>()
                .AddTransient<CategoryService>()
                .AddTransient<QuizService>()
                .AddTransient<AdminService>()
                   .AddTransient<StudentQuizService>();
            builder.Services.AddScoped<IValidator<QuizSaveDto>, QuizValidator>();

            builder.Services.AddApplicationJwtAuth(builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>()!);

           
            var allowedOriginStr = builder.Configuration["FrontendUrl"];
            var allowedOrigins = allowedOriginStr!.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            builder.Services.AddCors(option => option.AddPolicy("Maui",
             policy => policy.WithOrigins(allowedOrigins)
             .AllowAnyMethod()
             .AllowAnyHeader()
             .AllowCredentials()
             ));
            builder.Services.AddAuthorization();
            var app = builder.Build();
          
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("Maui");
            app.UseAuthentication();
            app.UseAuthorization();

           

            app.MapAuthEndpoints()
                .MapCategoryEndpoints()
                .MapQuizEndpoints()
                .MapAdminEndpoints()
                .MapStudentQuizEndpoints();
            app.Run();
        }
    }
}
