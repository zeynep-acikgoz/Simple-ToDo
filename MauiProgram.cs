using Microsoft.Extensions.Logging;
using CetTodoApp.Data; 

namespace CetTodoApp;

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

#if DEBUG
        builder.Logging.AddDebug();
#endif

       
        builder.Services.AddSingleton<TodoItemDatabase>();

       
        builder.Services.AddSingleton<MainPage>();

       
        return builder.Build();
    }
}