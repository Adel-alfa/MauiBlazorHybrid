using MauiBlazorHybrid.Api.Services;
using MauiBlazorHybrid.SharedLibrary.DTOs;

namespace MauiBlazorHybrid.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/auth/login",async(LoginDto logDto, AuthService authService)=>
            Results.Ok(await authService.LoginAsync(logDto)));


            app.MapPost("/api/auth/register", async (RegisterDto registerDto, AuthService authService) =>
            Results.Ok(await authService.RegisterAsync(registerDto)));


            return app;
        }
            
    }
}
