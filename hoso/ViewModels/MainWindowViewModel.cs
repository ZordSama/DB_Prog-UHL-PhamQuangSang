using Avalonia.Interactivity;
using hoso.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace hoso.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly KhmtK8bContext _context;
    private string _searchText = "";
    private ObservableCollection<Sinhvien>? _sinhviens;
    private ObservableCollection<Sinhvien>? _searchResults;

    public ObservableCollection<Sinhvien>? Sinhviens
    {
        get => _sinhviens;
        set => this.RaiseAndSetIfChanged(ref _sinhviens, value);
    }
    public ObservableCollection<Sinhvien>? SearchResults
    {
        get => _searchResults;
        set => this.RaiseAndSetIfChanged(ref _searchResults, value);
    }
    public string SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }

    public ReactiveCommand<Unit, Unit> SearchCommand { get; }

    public MainWindowViewModel()
    {
        _context = new KhmtK8bContext();
        SearchCommand = ReactiveCommand.CreateFromTask(SearchCmdHandler);
        LoadSinhviens();
    }

    private async Task SearchCmdHandler()
    {
        if (string.IsNullOrEmpty(SearchText))
        {
            SearchResults = Sinhviens;
        }else{
            SearchResults = new ObservableCollection<Sinhvien>(Sinhviens.Where(s => s.Hoten.ToLower().Contains(SearchText.ToLower())));
        }
    }

    private async void LoadSinhviens()
    {
        var sinhviens = await LoadSinhviensAsync();
        Sinhviens = new ObservableCollection<Sinhvien>(sinhviens);
        SearchResults = Sinhviens;
    }

    private async Task<List<Sinhvien>> LoadSinhviensAsync()
    {
        return await _context.Sinhviens.Include(s => s.LopNavigation).ToListAsync();
    }
}
