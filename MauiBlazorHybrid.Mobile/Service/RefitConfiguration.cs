using MauiBlazorHybrid.SharedComponents.Apis;
using MauiBlazorHybrid.SharedComponents.Auth;
using Refit;

using System;
using System.Collections.Generic;
using System.Linq;


using System.Text;
using System.Threading.Tasks;
#if ANDROID
using Xamarin.Android.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
#elif IOS
using Security;
#endif

namespace MauiBlazorHybrid.Mobile.Service
{
    public static class RefitConfiguration
    {
        static readonly string ApiBaseUrl = DeviceInfo.Platform == DevicePlatform.Android
                                   ? "https://10.0.2.2:7004"
                                   : "https://localhost:7004";
        public static IServiceCollection AddRefitConfiguration(this IServiceCollection services)
        {
            
            services.AddRefitClient<IAuthApi>(GetRefitSettings).ConfigureHttpClient(SetHttpClient);
            
            services.AddRefitClient<ICategoryApi>(GetRefitSettings).ConfigureHttpClient(SetHttpClient);

            //services.AddRefitClient<IQuizApi>(GetRefitSettings).ConfigureHttpClient(SetHttpClient);

           

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
                AuthorizationHeaderValueGetter = (_, __) => Task.FromResult(authStateProvider.User?.Token ?? ""),
                HttpMessageHandlerFactory = () =>
                {
                    // Android
#if ANDROID
                    var androidMessageHandler = new AndroidMessageHandler();
                    androidMessageHandler.ServerCertificateCustomValidationCallback =
                    (HttpRequestMessage requestMessage, X509Certificate2? certificate2, X509Chain? chain, SslPolicyErrors sslPolicyErrors) =>
                    {
                        return certificate2?.Issuer == "CN=localhost" || sslPolicyErrors == SslPolicyErrors.None;
                    };
                    return androidMessageHandler;
#elif IOS
            var nsUrlSessionHandler = new NSUrlSessionHandler
            {
                TrustOverrideForUrl =
                (NSUrlSessionHandler sender, string url, SecTrust trust) =>
                {
                    return url.StartsWith("https://localhost");
                }
            };
            return nsUrlSessionHandler;
#else
            // Default HTTP handler for other platforms
            return new HttpClientHandler();
#endif
                }
            };
        }

    }
}
