using Azure.Core;
using MauiBlazorHybrid.SharedLibrary;
using MauiBlazorHybrid.SharedLibrary.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.SharedComponents.Auth
{
    public class QuizAuthStateProvider : AuthenticationStateProvider
    {
        private const string AuthType = "access_token";
        private const string UserDataKey = "access_Key";
        public LoggedInUser User { get; private set ; }
        public bool _isAuthenticated => User?.Id > 0;
        private  Task<AuthenticationState> _authenticatedTask;
        private readonly IStorageService _storageService;
        private readonly NavigationManager _navigationManager;

       public QuizAuthStateProvider(IStorageService storageService, NavigationManager navigationManager)
        {
            _storageService = storageService;
           _navigationManager = navigationManager;
            ClaimsPrincipal anonymous = new(new ClaimsIdentity());
            var authstate = new AuthenticationState(anonymous);

            _authenticatedTask = Task.FromResult(authstate);
           
        }
        public override Task<AuthenticationState> GetAuthenticationStateAsync() => _authenticatedTask;

        public async Task SetLoginAsync(LoggedInUser user)
        {
            User = user;
            await _storageService.SetItem( UserDataKey, user.ToJson());

            var identity = new ClaimsIdentity(user.ToClaims(), AuthType);
            
            ClaimsPrincipal principal = new(identity);
            var authstate = new AuthenticationState(principal);

            _authenticatedTask = Task.FromResult(authstate);

            NotifyAuthenticationStateChanged(_authenticatedTask);
        }
        public async Task SetLogOutAsync()
        {
            User = null;
           
            ClaimsPrincipal anonymous = new(new ClaimsIdentity());
            var authstate = new AuthenticationState(anonymous);
            _authenticatedTask = Task.FromResult(authstate);
            NotifyAuthenticationStateChanged(_authenticatedTask);
            await _storageService.RemoveItem( UserDataKey);
        }
        public bool IsInitializing { get; private set; } = true;
        public NavigationManager NavigationManager { get; }

        public async Task InitializeAsync()
        {
            await InitializeAsync(redirectToLogin: true);
        }
        public async Task<bool> InitializeAsync(bool redirectToLogin = true)
        {
            try
            {
                var userdata = await _storageService.GetItem(UserDataKey);
                if (string.IsNullOrWhiteSpace(userdata))
                {
                    if(redirectToLogin) 
                     _navigationManager.NavigateTo("/account/login");
                    return false;
                }
                var user = LoggedInUser.FromJson(userdata);
                if (user == null || user.Id <= 0)
                {
                    if (redirectToLogin)
                        _navigationManager.NavigateTo("/account/login");
                    return false;
                }
                if (!IsTokenValid(user.Token))
                {
                    if (redirectToLogin)
                        _navigationManager.NavigateTo("/account/login");
                    return false;
                }
                   
                await SetLoginAsync(user);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
            finally 
            {
                IsInitializing = false;
            }
            return false;
        }
        private static bool IsTokenValid(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;
            var jwtHandler = new JwtSecurityTokenHandler();
            if(!jwtHandler.CanReadToken(token))
                return false;
            var jwt = jwtHandler.ReadJwtToken(token);
            var expClam = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
            if(expClam == null)
                return false;

            var expTime = long.Parse(expClam.Value);
            var expTimeUtc = DateTimeOffset.FromUnixTimeSeconds(expTime).UtcDateTime;

            return expTimeUtc > DateTime.UtcNow;

        }
    }
}
