using Microsoft.Extensions.Logging;
//using Plugin.Maui.Audio;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // var config = new ConfigurationBuilder()
            //     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //     .Build();

            //builder.Configuration.AddConfiguration(config);


            //builder.Services.AddSingleton(AudioManager.Current);

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
