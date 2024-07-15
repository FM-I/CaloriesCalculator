using CaloriesCalculator.Interfaces;
using CaloriesCalculator.Pages;
using CaloriesCalculator.Repositories;
using CaloriesCalculator.ViewModel;
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

            builder.Services.AddSingleton<IDBContext>(
                new RealtimeDataBase("https://education-project-7ab74-default-rtdb.europe-west1.firebasedatabase.app/")
                );

            builder.Services.AddTransient<ProductPage>();
            builder.Services.AddTransient<ProductViewModel>();
            
            builder.Services.AddTransient<CalculateCaloriesPage>();
            builder.Services.AddTransient<CalculateCaloriesViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
