using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GuraruRepository.Models;

public partial class GuraruContext : DbContext
{
    public GuraruContext()
    {
    }

    public GuraruContext(DbContextOptions<GuraruContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GuraruExit> GuraruExits { get; set; }

    public virtual DbSet<Machine> Machines { get; set; }

    public virtual DbSet<MachineOperator> MachineOperators { get; set; }

    public virtual DbSet<ManufacturingStage1> ManufacturingStage1s { get; set; }

    public virtual DbSet<ManufacturingStage2> ManufacturingStage2s { get; set; }

    public virtual DbSet<RawQuality> RawQualities { get; set; }

    public virtual DbSet<RawThread> RawThreads { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=AYUSHLENOVO\\SQLEXPRESS;Database=Guraru;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GuraruExit>(entity =>
        {
            entity.HasKey(e => e.ExitId).HasName("PK_ExitId");

            entity.ToTable("GuraruExit");

            entity.Property(e => e.ChallanPhotoPath)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DestinationCity)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DriverName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SubmittedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SubmittedDate).HasColumnType("datetime");
            entity.Property(e => e.VehicleNumber)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.QualityNavigation).WithMany(p => p.GuraruExits)
                .HasForeignKey(d => d.Quality)
                .HasConstraintName("FK__GuraruExi__Quali__02FC7413");
        });

        modelBuilder.Entity<Machine>(entity =>
        {
            entity.HasKey(e => e.MachineId).HasName("PK__Machine__44EE5B383E586FED");

            entity.ToTable("Machine");

            entity.Property(e => e.MachineName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MachineOperator>(entity =>
        {
            entity.HasKey(e => e.OperatorId).HasName("PK__MachineO__7BB12FAEFB136D82");

            entity.ToTable("MachineOperator");

            entity.Property(e => e.AdhaarNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.LocationDetails)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OperatorName).IsUnicode(false);
        });

        modelBuilder.Entity<ManufacturingStage1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manufact__3214EC27AF23070A");

            entity.ToTable("ManufacturingStage1");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompletedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.SubmittedBy).IsUnicode(false);
            entity.Property(e => e.SubmittedDate).HasColumnType("datetime");

            entity.HasOne(d => d.MachineNavigation).WithMany(p => p.ManufacturingStage1s)
                .HasForeignKey(d => d.Machine)
                .HasConstraintName("FK__Manufactu__Machi__52593CB8");

            entity.HasOne(d => d.MachineOperatorNavigation).WithMany(p => p.ManufacturingStage1s)
                .HasForeignKey(d => d.MachineOperator)
                .HasConstraintName("FK__Manufactu__Machi__534D60F1");

            entity.HasOne(d => d.QualityNavigation).WithMany(p => p.ManufacturingStage1s)
                .HasForeignKey(d => d.Quality)
                .HasConstraintName("FK__Manufactu__Quali__5441852A");
        });

        modelBuilder.Entity<ManufacturingStage2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manufact__3214EC279A8C9AF2");

            entity.ToTable("ManufacturingStage2");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompletedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.SubmittedBy).IsUnicode(false);
            entity.Property(e => e.SubmittedDate).HasColumnType("datetime");

            entity.HasOne(d => d.QualityNavigation).WithMany(p => p.ManufacturingStage2s)
                .HasForeignKey(d => d.Quality)
                .HasConstraintName("FK__Manufactu__Quali__6FE99F9F");
        });

        modelBuilder.Entity<RawQuality>(entity =>
        {
            entity.HasKey(e => e.QualityId).HasName("PK__RawQuali__0B22BB4E0628F8B5");

            entity.ToTable("RawQuality");

            entity.Property(e => e.QualityCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.QualityName).IsUnicode(false);
        });

        modelBuilder.Entity<RawThread>(entity =>
        {
            entity.HasKey(e => e.ThreadId).HasName("PK__RawThrea__688356E465E10E34");

            entity.ToTable("RawThread");

            entity.Property(e => e.ThreadId).HasColumnName("ThreadID");
            entity.Property(e => e.BillDate).HasColumnType("datetime");
            entity.Property(e => e.CompanyName).IsUnicode(false);
            entity.Property(e => e.CreatedBy).IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.QualityNavigation).WithMany(p => p.RawThreads)
                .HasForeignKey(d => d.Quality)
                .HasConstraintName("FK__RawThread__Quali__4F7CD00D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
