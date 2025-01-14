using MauiBlazorHybrid.SharedLibrary.DTOs;
using Refit;

namespace MauiBlazorHybrid.SharedComponents.Apis
{
    public interface IAuthApi
    {
        [Post("/api/auth/login")]
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        [Post("/api/auth/register")]
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    }
}
