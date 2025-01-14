using MauiBlazorHybrid.Api.Data.Entitties;
using MauiBlazorHybrid.Api.Data;
using MauiBlazorHybrid.SharedLibrary.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using MauiBlazorHybrid.SharedLibrary.Utilities;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace MauiBlazorHybrid.Api.Services
{
    public class AuthService
    {
        private readonly AppDbContext db;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly JwtConfiguration jwtConfig;
        public AuthService(AppDbContext _db,
            IPasswordHasher<User> _passwordHasher,
             IOptions<JwtConfiguration> config)
        {
            db = _db;
            passwordHasher = _passwordHasher;
            jwtConfig = config.Value;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            if (loginDto == null) return new AuthResponseDto(default, "Invalid login model"); ;
            var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(_ => _.Email == loginDto.Email);
            if (user is null)
            {

                return new AuthResponseDto(default, "Invalid user name");
            }
            if (!user.IsApproved)
            {
                return new AuthResponseDto(default, "Accout is not approved yet");
            }
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return new AuthResponseDto(default, "Incorrect Password"); ;
            }
            // Generate JWT
            var jwt = GenerateTokenString(user);
            LoggedInUser loggedInUser = new(user.Id,user.Name, user.Role, jwt);
            return new AuthResponseDto(loggedInUser);
        }
        public async Task<QuizApiResponse> RegisterAsync(RegisterDto registerModel)
        {
            if(await db.Users.AnyAsync(_=>_.Email == registerModel.Email))
             {
                return QuizApiResponse.Fail("Email already exist!");
            }
            User user = new User
            {
                Email = registerModel.Email,
                Name = registerModel.Name,
                Phone = registerModel.Phone,
                Role = nameof(UserRole.Client),
                IsApproved = false,
            };
            user.PasswordHash = passwordHasher.HashPassword(user, registerModel.Password);
            db.Users.Add(user);
            try
            {
                await db.SaveChangesAsync();
                return QuizApiResponse.Success();
            }
            catch (Exception ex)
            {

                return QuizApiResponse.Fail(ex.Message);
            }
        }
        public string GenerateTokenString(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key));
            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var expiryInDay = DateTime.Now.AddDays(Convert.ToInt32(jwtConfig.ExpiryInDay));
            var securityToken = new JwtSecurityToken(
                claims: claims,
                //expires: DateTime.Now.AddMinutes(60),
                expires: expiryInDay,
                issuer: jwtConfig.Issuer,
                audience: jwtConfig.Audience,
                signingCredentials: signingCred);
            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }
    }
}
