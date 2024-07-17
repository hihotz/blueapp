using blueapp.Service;
using blueapp.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace blueapp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // DatabaseService를 싱글턴으로 등록
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<ProductViewModel>();

            // RecordViewModel을 트랜지언트로 등록
            builder.Services.AddTransient<GraphViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
