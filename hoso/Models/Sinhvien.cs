using System;
using System.Collections.Generic;

namespace hoso.Models;

public partial class Sinhvien
{
    public string Masv { get; set; } = null!;

    public string? Hoten { get; set; }

    public int? Gioitinh { get; set; }

    public DateOnly? Ngaysinh { get; set; }

    public string? Quequan { get; set; }

    public string? Lop { get; set; }

    public virtual Lop? LopNavigation { get; set; }
}
