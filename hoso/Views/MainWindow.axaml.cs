using Avalonia.Controls;
using hoso.Models;
using hoso.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace hoso.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = Program.ServiceProvider?.GetRequiredService<MainWindowViewModel>();
    }
}