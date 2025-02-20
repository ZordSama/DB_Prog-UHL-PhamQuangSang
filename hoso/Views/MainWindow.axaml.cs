using Avalonia.Controls;
using hoso.ViewModels;

namespace hoso.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = new MainWindowViewModel();
    }
}