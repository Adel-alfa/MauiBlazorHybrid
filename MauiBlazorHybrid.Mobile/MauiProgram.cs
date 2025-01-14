using MauiBlazorHybrid.Mobile.Service;
using MauiBlazorHybrid.SharedComponents.Apis;
using MauiBlazorHybrid.SharedComponents.Auth;
using MauiBlazorHybrid.SharedLibrary;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Refit;

namespace MauiBlazorHybrid.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
           
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddSingleton<QuizAuthStateProvider>();
            builder.Services.AddSingleton(sp => (AuthenticationStateProvider)sp.GetRequiredService<QuizAuthStateProvider>());
            builder.Services.AddAuthorizationCore();

            builder.Services.AddSingleton<IStorageService, StorageService>()
                .AddSingleton<IAppState, AppState>()
                .AddSingleton<QuizState>()
                .AddSingleton<IPlatform,MobilePlatform>(); 

            builder.Services.AddRefitConfiguration();

            return builder.Build();
        }

        static void RefitConfiguration(IServiceCollection services)
        {

             var ApiBaseUrl = "https://localhost:7004";
            if(DeviceInfo.DeviceType == DeviceType.Physical || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                ApiBaseUrl = "https://14cpz2qr-7004.uks1.devtunnels.ms/";
            }
            else
            {
                if(DeviceInfo.Platform == DevicePlatform.Android)
                     ApiBaseUrl = "https://10.0.2.2:7004";                
            }
            services.AddRefitClient<IAuthApi>().ConfigureHttpClient(SetHttpClient);
            
             void SetHttpClient(HttpClient httpClient) =>
              httpClient.BaseAddress = new Uri(ApiBaseUrl);
            
            static RefitSettings GetRefitSettings(IServiceProvider serviceProvider)
            {
                var authStateProvider = serviceProvider.GetRequiredService<QuizAuthStateProvider>();
                return new RefitSettings
                {
                    AuthorizationHeaderValueGetter = (_, __) => Task.FromResult(authStateProvider.User?.Token ?? "")
                };
            }
    }
    }
}
