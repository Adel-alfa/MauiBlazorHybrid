
using MauiBlazorHybrid.SharedComponents.Auth;
using MauiBlazorHybrid.SharedLibrary;

using MauiBlazorHybrid.web.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


namespace MauiBlazorHybrid.web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            
            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddSingleton<QuizAuthStateProvider>();
            builder.Services.AddSingleton(sp => (AuthenticationStateProvider)sp.GetRequiredService<QuizAuthStateProvider>());
            builder.Services.AddSingleton<IStorageService, StorageService>();

            builder.Services.AddSingleton<IAppState, AppState>();
            builder.Services.AddSingleton<QuizState>()
                 .AddSingleton<IPlatform, WebPlatform>(); ;

            builder.Services.AddRefitConfiguration();

            await builder.Build().RunAsync();

            
        }
        
    }
}
