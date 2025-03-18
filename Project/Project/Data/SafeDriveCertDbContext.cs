using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project.Models;

public partial class SafeDriveCertDbContext : DbContext
{
    public SafeDriveCertDbContext()
    {
    }

    public SafeDriveCertDbContext(DbContextOptions<SafeDriveCertDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Option> Options { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);uid=sa;password=123;database=SafeDriveCertDB;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.HasKey(e => e.CertificateId).HasName("PK__Certific__BBF8A7C15E67A8D1");

            entity.HasIndex(e => e.CertificateCode, "UQ__Certific__9B855830C003AC0F").IsUnique();

            entity.Property(e => e.CertificateCode)
                .HasMaxLength(50)
                .IsUnicode(true);  // NVARCHAR

            entity.HasOne(d => d.Exam).WithMany(p => p.Certificates)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("FK_Certificates_Exams");

            entity.HasOne(d => d.User).WithMany(p => p.Certificates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Certifica__UserI__17036CC0");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A7EBAEAD8E");

            entity.Property(e => e.CourseName)
                .HasMaxLength(100)
                .IsUnicode(true);  // NVARCHAR

            entity.HasOne(d => d.Teacher).WithMany(p => p.Courses)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Courses__Teacher__02084FDA");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.ExamId).HasName("PK__Exams__297521C739BC45A1");

            entity.Property(e => e.Room)
                .HasMaxLength(50)
                .IsUnicode(true);  // NVARCHAR

            entity.HasOne(d => d.Course).WithMany(p => p.Exams)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exams__CourseId__04E4BC85");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E3212C1CA9C");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.SentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            // Nếu Message đang dùng kiểu text, bạn có thể chuyển sang nvarchar(max)
            entity.Property(e => e.Message)
                .HasColumnType("nvarchar(max)")
                .IsUnicode(true);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__UserI__1BC821DD");
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK__Options__92C7A1FF2CF95A3D");

            entity.Property(e => e.OptionText)
                .HasMaxLength(500)
                .IsUnicode(true);  // NVARCHAR

            entity.HasOne(d => d.Question).WithMany(p => p.Options)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Options__Questio__0A9D95DB");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06FAC9B2A22D8");

            entity.Property(e => e.ImagePath)
                .HasMaxLength(250)
                .IsUnicode(true);  // NVARCHAR

            entity.HasOne(d => d.Exam).WithMany(p => p.Questions)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Questions__ExamI__07C12930");
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__Registra__6EF588105D91F922");

            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(true)  // NVARCHAR
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Course).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Cours__0F624AF8");

            entity.HasOne(d => d.User).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__UserI__0E6E26BF");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK__Results__97690208E77856AD");

            entity.Property(e => e.Score)
                .HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Exam).WithMany(p => p.Results)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Results__ExamId__123EB7A3");

            entity.HasOne(d => d.User).WithMany(p => p.Results)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Results__UserId__1332DBDC");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C88F0EB0B");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534329D5B8A").IsUnique();

            entity.Property(e => e.Class)
                .HasMaxLength(50)
                .IsUnicode(true);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(true);
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(true);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(true);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(true);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(true);
            entity.Property(e => e.School)
                .HasMaxLength(100)
                .IsUnicode(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
