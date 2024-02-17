using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace ArchiveInfrastructure;


public partial class DbfacultyArchiveContext : DbContext
{
    public DbfacultyArchiveContext()
    {
    }

    public DbfacultyArchiveContext(DbContextOptions<DbfacultyArchiveContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Date> Dates { get; set; }

    public virtual DbSet<Reference> References { get; set; }

    public virtual DbSet<SearchHistory> SearchHistories { get; set; }

    public virtual DbSet<SearchObject> SearchObjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=Denis\\SQLEXPRESS; Database=DBFacultyArchive; Trusted_Connection=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Date>(entity =>
        {
            entity.ToTable("Date");

            entity.Property(e => e.DateId).HasColumnName("date_id");
            entity.Property(e => e.Date1).HasColumnName("date");
            entity.Property(e => e.Department)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("department");
            entity.Property(e => e.ExtentOfMaterial)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("extent_of_material");
            entity.Property(e => e.Faculty)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("faculty");
            entity.Property(e => e.Format)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("format");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Reference>(entity =>
        {
            entity.ToTable("Reference");

            entity.Property(e => e.ReferenceId).HasColumnName("reference_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Searchable)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("searchable");
            entity.Property(e => e.SoId).HasColumnName("so_id");

            entity.HasOne(d => d.So).WithMany(p => p.References)
                .HasForeignKey(d => d.SoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reference_SearchObject");

            entity.HasMany(d => d.Dates).WithMany(p => p.References)
                .UsingEntity<Dictionary<string, object>>(
                    "DateReference",
                    r => r.HasOne<Date>().WithMany()
                        .HasForeignKey("DateId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DateReference_Date"),
                    l => l.HasOne<Reference>().WithMany()
                        .HasForeignKey("ReferenceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DateReference_Reference"),
                    j =>
                    {
                        j.HasKey("ReferenceId", "DateId");
                        j.ToTable("DateReference");
                        j.IndexerProperty<int>("ReferenceId").HasColumnName("reference_id");
                        j.IndexerProperty<int>("DateId")
                            .ValueGeneratedOnAdd()
                            .HasColumnName("date_id");
                    });
        });

        modelBuilder.Entity<SearchHistory>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("SearchHistory");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.SearchDate).HasColumnName("search_date");
            entity.Property(e => e.SearchSuccess).HasColumnName("search_success");
            entity.Property(e => e.SoId).HasColumnName("so_id");

            entity.HasOne(d => d.So).WithMany(p => p.SearchHistories)
                .HasForeignKey(d => d.SoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SearchHistory_SearchObject");
        });

        modelBuilder.Entity<SearchObject>(entity =>
        {
            entity.HasKey(e => e.SoId);

            entity.ToTable("SearchObject");

            entity.Property(e => e.SoId).HasColumnName("so_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Faculty)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("faculty");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.ReferenceId).HasColumnName("reference_id");
            entity.Property(e => e.SearchTime).HasColumnName("search_time");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId)
                .ValueGeneratedOnAdd()
                .HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");

            entity.HasOne(d => d.UserNavigation).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_SearchHistory");

            entity.HasMany(d => d.References).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserReference",
                    r => r.HasOne<Reference>().WithMany()
                        .HasForeignKey("ReferenceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UserReference_Reference"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UserReference_User"),
                    j =>
                    {
                        j.HasKey("UserId", "ReferenceId");
                        j.ToTable("UserReference");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("ReferenceId").HasColumnName("reference_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
