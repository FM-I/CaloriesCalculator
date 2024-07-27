using CaloriesCalculator.Interfaces;
using CaloriesCalculator.Pages;
using CaloriesCalculator.Repositories;
using CaloriesCalculator.ViewModel;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Microsoft.Extensions.Logging;

namespace CaloriesCalculator
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddTransient<ProductPage>();
            builder.Services.AddTransient<ProductViewModel>();
            
            builder.Services.AddTransient<CalculateCaloriesPage>();
            builder.Services.AddTransient<CalculateCaloriesViewModel>();

            builder.Services.AddTransient<AddProductPage>();
            builder.Services.AddTransient<AddProductViewModel>();

            builder.Services.AddSingleton<SignInPage>();
            builder.Services.AddSingleton<SignInViewModel>();

            builder.Services.AddScoped<SignUpPage>();
            builder.Services.AddScoped<SignUpViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = "AIzaSyCdp0zwXx0zPVZu7Ma6-v1HBWwVIm5NsKE",
                AuthDomain = "education-project-7ab74.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                },
                UserRepository = new FileUserRepository("Storage")
            }));

            builder.Services.AddSingleton<IDBContext, RealtimeDataBase>();

            return builder.Build();
        }
    }
}
