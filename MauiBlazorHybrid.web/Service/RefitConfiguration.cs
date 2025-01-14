using MauiBlazorHybrid.SharedComponents.Apis;
using MauiBlazorHybrid.SharedComponents.Auth;
using Refit;

namespace MauiBlazorHybrid.web.Service
{
    public static class RefitConfiguration
    {
        //builder.Services.AddRefitConfiguration();
        public static IServiceCollection AddRefitConfiguration(this IServiceCollection services)
        {
            const string ApiBaseUrl = "https://localhost:7004";
            services.AddRefitClient<IAuthApi>().ConfigureHttpClient(SetHttpClient);
            services.AddRefitClient<ICategoryApi>(GetRefitSettings).ConfigureHttpClient(SetHttpClient);

            services.AddRefitClient<IQuizApi>(GetRefitSettings).ConfigureHttpClient(SetHttpClient);

            services.AddRefitClient<IAdminApi>(GetRefitSettings).ConfigureHttpClient(SetHttpClient);

            services.AddRefitClient<IStudentQuizApi>(GetRefitSettings).ConfigureHttpClient(SetHttpClient);


            static void SetHttpClient(HttpClient httpClient) =>
                httpClient.BaseAddress = new Uri(ApiBaseUrl);
            return services;
        }
        public static RefitSettings GetRefitSettings(IServiceProvider serviceProvider) 
        {
            var authStateProvider = serviceProvider.GetRequiredService<QuizAuthStateProvider>();
            return new RefitSettings
            {
                AuthorizationHeaderValueGetter = (_, __) => Task.FromResult(authStateProvider.User?.Token ?? "")
            };
        }
    }
}
