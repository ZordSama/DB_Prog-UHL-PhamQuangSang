using Avalonia.Interactivity;
using hoso.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
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
    private ObservableCollection<Lop>? _lops;
    private string _newMasv = "";
    private string _newHoten = "";
    private ObservableCollection<string> _genders = ["Nam", "Nữ", "Không xác định"];
    private string _selectedGender = "Nam";
    private DateTimeOffset? _newNgaysinh;
    private string _newQuequan = "";
    private Lop? _selectedLop;
    private ObservableCollection<string>? _tenLops; private Sinhvien? _editingSinhvien; // Track the student being edited
    private bool _isEditing; // Track if we're in edit mode
    public bool IsEditing
    {
        get => _isEditing;
        set => this.RaiseAndSetIfChanged(ref _isEditing, value);
    }
    public bool IsMasvEnabled => !IsEditing;

    public ObservableCollection<string>? TenLops { get => _tenLops; set => this.RaiseAndSetIfChanged(ref _tenLops, value); }
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
    public ObservableCollection<Lop>? Lops
    {
        get => _lops;
        set => this.RaiseAndSetIfChanged(ref _lops, value);
    }
    public string NewMasv
    {
        get => _newMasv;
        set => this.RaiseAndSetIfChanged(ref _newMasv, value);
    }
    public string NewHoten
    {
        get => _newHoten;
        set => this.RaiseAndSetIfChanged(ref _newHoten, value);
    }
    public ObservableCollection<string> Genders
    {
        get => _genders;
        set => this.RaiseAndSetIfChanged(ref _genders, value);
    }
    public string SelectedGender
    {
        get => _selectedGender;
        set => this.RaiseAndSetIfChanged(ref _selectedGender, value);
    }
    public DateTimeOffset? NewNgaysinh
    {
        get => _newNgaysinh;
        set => this.RaiseAndSetIfChanged(ref _newNgaysinh, value);
    }
    public string NewQuequan
    {
        get => _newQuequan;
        set => this.RaiseAndSetIfChanged(ref _newQuequan, value);
    }
    public string? SelectedLop
    {
        get => _selectedLop?.Tenlop;
        set => this.RaiseAndSetIfChanged(ref _selectedLop, _lops.FirstOrDefault(l => l.Tenlop == value));
    }

    public ReactiveCommand<Unit, Unit> SearchCommand { get; }
    public ReactiveCommand<Unit, Unit> AddSinhvienCommand { get; }
    public ReactiveCommand<Sinhvien, Unit> EditSinhvienCommand { get; }
    public ReactiveCommand<Sinhvien, Unit> DeleteSinhvienCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelEditCommand { get; }

    public MainWindowViewModel(KhmtK8bContext context)
    {
        _context = context;
        Console.WriteLine("Loaded MainWindowViewModel");
        SearchCommand = ReactiveCommand.CreateFromTask(SearchCmdHandler);
        AddSinhvienCommand = ReactiveCommand.CreateFromTask(AddSinhvienAsync);
        EditSinhvienCommand = ReactiveCommand.CreateFromTask<Sinhvien>(EditSinhvienAsync);
        DeleteSinhvienCommand = ReactiveCommand.CreateFromTask<Sinhvien>(DeleteSinhvienAsync);
        CancelEditCommand = ReactiveCommand.Create(CancelEdit);
        this.WhenAnyValue(x => x.IsEditing).Subscribe(_ => this.RaisePropertyChanged(nameof(IsMasvEnabled)));
        LoadSinhviens();
    }

    private async Task SearchCmdHandler()
    {
        if (string.IsNullOrEmpty(SearchText))
        {
            SearchResults = Sinhviens;
        }
        else
        {
            SearchResults = new ObservableCollection<Sinhvien>(Sinhviens.Where(s => s.Hoten.ToLower().Contains(SearchText.ToLower())));
        }
    }

    private async void LoadSinhviens()
    {
        var sinhviens = await LoadSinhviensAsync();
        Sinhviens = new ObservableCollection<Sinhvien>(sinhviens);
        SearchResults = Sinhviens;
        var lops = await _context.Lops.ToListAsync();
        Lops = new ObservableCollection<Lop>(lops);
        TenLops = new ObservableCollection<string>(lops.Select(l => l.Tenlop));
    }

    private async Task<List<Sinhvien>> LoadSinhviensAsync()
    {
        return await _context.Sinhviens.Include(s => s.LopNavigation).ToListAsync();
    }



    private async Task EditSinhvienAsync(Sinhvien sinhvien)
    {
        _editingSinhvien = sinhvien;
        IsEditing = true;

        // Populate form with selected student's data
        NewMasv = sinhvien.Masv;
        NewHoten = sinhvien.Hoten;
        SelectedGender = sinhvien.Gioitinh switch
        {
            0 => "Nam",
            1 => "Nữ",
            _ => "Không xác định"
        };
        NewNgaysinh = sinhvien.Ngaysinh.HasValue ? new DateTimeOffset(sinhvien.Ngaysinh.Value.ToDateTime(TimeOnly.MinValue)) : null;
        NewQuequan = sinhvien.Quequan;
        SelectedLop = sinhvien.LopNavigation?.Tenlop;
    }

    private async Task DeleteSinhvienAsync(Sinhvien sinhvien)
    {
        try
        {
            var warningBox = MessageBoxManager.GetMessageBoxStandard("WARNING!", "Bạn có chắc chắn muốn xóa sinh viên này?", MsBox.Avalonia.Enums.ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Warning);
            var result = await warningBox.ShowAsync();
            if (result != MsBox.Avalonia.Enums.ButtonResult.Yes)
            {
                return;
            }
            _context.Sinhviens.Remove(sinhvien);
            await _context.SaveChangesAsync();

            Sinhviens.Remove(sinhvien);
            SearchResults = string.IsNullOrEmpty(SearchText)
                ? Sinhviens
                : new ObservableCollection<Sinhvien>(Sinhviens.Where(s => s.Hoten.ToLower().Contains(SearchText.ToLower())));
        }
        catch (Exception ex)
        {
            var errorBox = MessageBoxManager.GetMessageBoxStandard("ERROR!", ex.Message, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            await errorBox.ShowAsync();
        }
    }

    private void CancelEdit()
    {
        // Clear form and reset editing state
        NewMasv = "";
        NewHoten = "";
        SelectedGender = "Nam";
        NewNgaysinh = null;
        NewQuequan = "";
        SelectedLop = null;
        _editingSinhvien = null;
        IsEditing = false;
    }

    private async Task AddSinhvienAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(NewMasv) || string.IsNullOrWhiteSpace(NewHoten) || SelectedLop == null)
            {
                throw new Exception("Vui lòng nhập đầy đủ thông tin");
            }
            if (_isEditing && _editingSinhvien != null)
            {
                // Update existing student
                _editingSinhvien.Masv = NewMasv;
                _editingSinhvien.Hoten = NewHoten;
                _editingSinhvien.Gioitinh = SelectedGender switch
                {
                    "Nam" => 0,
                    "Nữ" => 1,
                    _ => null
                };
                _editingSinhvien.Ngaysinh = NewNgaysinh.HasValue ? DateOnly.FromDateTime(NewNgaysinh.Value.Date) : null;
                _editingSinhvien.Quequan = NewQuequan;
                _editingSinhvien.LopNavigation = _selectedLop;

                await _context.SaveChangesAsync();
                CancelEdit();
            }
            else
            {
                // Check if Masv already exists
                if (_context.Sinhviens.Any(s => s.Masv == NewMasv))
                {
                    throw new Exception("Mã sinh viên đã tồn tại");
                }

                var newSv = new Sinhvien
                {
                    Masv = NewMasv,
                    Hoten = NewHoten,
                    Gioitinh = SelectedGender switch
                    {
                        "Nam" => 0,
                        "Nữ" => 1,
                        _ => null
                    },
                    Ngaysinh = NewNgaysinh.HasValue ? DateOnly.FromDateTime(NewNgaysinh.Value.Date) : null,
                    Quequan = NewQuequan,
                    LopNavigation = _selectedLop
                };

                _context.Sinhviens.Add(newSv);
                await _context.SaveChangesAsync();

                Sinhviens.Add(newSv);
                CancelEdit();
            }
            LoadSinhviens();
            // Clear form fields
            NewMasv = "";
            NewHoten = "";
            SelectedGender = "Không xác định";
            NewNgaysinh = null;
            NewQuequan = "";
            SelectedLop = null;
        }
        catch (Exception ex)
        {
            // MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            var errorBox = MessageBoxManager.GetMessageBoxStandard("ERROR!", ex.Message, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            await errorBox.ShowAsync();
        }
    }

}