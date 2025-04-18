using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using hoso.Models;
using hoso.ViewModels;
using hoso.Views;
using Microsoft.Extensions.DependencyInjection;

namespace hoso;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            // desktop.MainWindow = Program.ServiceProvider!.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }

}