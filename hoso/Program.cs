using Avalonia;
using Avalonia.ReactiveUI;
using hoso.Models;
using hoso.ViewModels;
using hoso.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace hoso;

sealed class Program
{
    public static IConfiguration Configuration {get; private set;}
    public static IServiceProvider? ServiceProvider { get; private set; }

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // => BuildAvaloniaApp()
        // .StartWithClassicDesktopLifetime(args);

        Configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json").Build();

        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static void ConfigureServices(IServiceCollection services){
        services.AddDbContext<KhmtK8bContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        services.AddTransient<MainWindowViewModel>();
        // services.AddSingleton<IServiceProvider>(sp => sp);
    }
    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}
