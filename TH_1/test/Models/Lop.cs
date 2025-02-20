using System;
using System.Collections.Generic;

namespace test.Models;

public partial class Lop
{
    public string Malop { get; set; } = null!;

    public string? Tenlop { get; set; }

    public virtual ICollection<Sinhvien> Sinhviens { get; set; } = new List<Sinhvien>();
}
