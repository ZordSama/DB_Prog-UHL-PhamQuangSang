using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace test.Models;

public partial class KhmtK8bContext : DbContext
{
    public KhmtK8bContext()
    {
    }

    public KhmtK8bContext(DbContextOptions<KhmtK8bContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Lop> Lops { get; set; }

    public virtual DbSet<Sinhvien> Sinhviens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=ZORD_ACER_A7\\ZORD_DB; Database=khmt_k8b; User Id=sa; Password=Sang123@; Trusted_Connection=True; Encrypt=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lop>(entity =>
        {
            entity.HasKey(e => e.Malop).HasName("PK__lop__15F456FD04C2EA27");

            entity.ToTable("lop");

            entity.Property(e => e.Malop)
                .HasMaxLength(255)
                .HasColumnName("malop");
            entity.Property(e => e.Tenlop)
                .HasMaxLength(255)
                .HasColumnName("tenlop");
        });

        modelBuilder.Entity<Sinhvien>(entity =>
        {
            entity.HasKey(e => e.Masv).HasName("PK__sinhvien__7A21767C2722D55E");

            entity.ToTable("sinhvien");

            entity.Property(e => e.Masv)
                .HasMaxLength(255)
                .HasColumnName("masv");
            entity.Property(e => e.Gioitinh).HasColumnName("gioitinh");
            entity.Property(e => e.Hoten)
                .HasMaxLength(255)
                .HasColumnName("hoten");
            entity.Property(e => e.Lop)
                .HasMaxLength(255)
                .HasColumnName("lop");
            entity.Property(e => e.Ngaysinh).HasColumnName("ngaysinh");
            entity.Property(e => e.Quequan)
                .HasMaxLength(500)
                .HasColumnName("quequan");

            entity.HasOne(d => d.LopNavigation).WithMany(p => p.Sinhviens)
                .HasForeignKey(d => d.Lop)
                .HasConstraintName("FK__sinhvien__lop__4316F928");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
