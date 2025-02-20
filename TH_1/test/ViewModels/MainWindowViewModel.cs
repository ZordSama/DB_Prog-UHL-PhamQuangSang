using test.Models;

namespace test.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly KhmtK8bContext _context;
    
    public string Greeting { get; } = "Welcome to Avalonia!";


}
