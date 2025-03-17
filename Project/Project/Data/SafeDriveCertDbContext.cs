using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Project.Models
{
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
        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<Result> Results { get; set; }
        public virtual DbSet<User> Users { get; set; }

        // Thêm DbSet cho các bảng mới
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Option> Options { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                var configuration = builder.Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Certificate>(entity =>
            {
                entity.HasKey(e => e.CertificateId).HasName("PK__Certific__BBF8A7C1EAE83E02");

                entity.HasIndex(e => e.CertificateCode, "UQ__Certific__9B8558304474748D").IsUnique();

                entity.Property(e => e.CertificateCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User).WithMany(p => p.Certificates)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Certifica__UserI__49C3F6B7");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A78A6A110A");

                entity.Property(e => e.CourseName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Teacher).WithMany(p => p.Courses)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Courses__Teacher__3A81B327");
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.HasKey(e => e.ExamId).HasName("PK__Exams__297521C719A6BC22");

                entity.Property(e => e.Room)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Course).WithMany(p => p.Exams)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Exams__CourseId__4222D4EF");
            });

            // Cấu hình cho bảng Questions
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(e => e.QuestionId).HasName("PK_Questions");

                entity.Property(e => e.Content)
                    .IsRequired();

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(250)
                    .IsUnicode(true); // Bạn có thể thay IsUnicode(false) nếu không cần Unicode

                entity.HasOne(e => e.Exam)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(e => e.ExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Questions_Exams");
            });

            // Cấu hình cho bảng Options
            modelBuilder.Entity<Option>(entity =>
            {
                entity.HasKey(e => e.OptionId).HasName("PK_Options");

                entity.Property(e => e.OptionText)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(true);

                entity.Property(e => e.IsCorrect)
                    .IsRequired();

                entity.HasOne(e => e.Question)
                    .WithMany(p => p.Options)
                    .HasForeignKey(e => e.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Options_Questions");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E129778BF24");

                entity.Property(e => e.IsRead).HasDefaultValue(false);
                entity.Property(e => e.Message).HasColumnType("text");
                entity.Property(e => e.SentDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__UserI__4E88ABD4");
            });

            modelBuilder.Entity<Registration>(entity =>
            {
                entity.HasKey(e => e.RegistrationId).HasName("PK__Registra__6EF58810183F3700");

                entity.Property(e => e.Comments).HasColumnType("text");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("Pending");

                entity.HasOne(d => d.Course).WithMany(p => p.Registrations)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Registrat__Cours__3F466844");

                entity.HasOne(d => d.User).WithMany(p => p.Registrations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Registrat__UserI__3E52440B");
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.HasKey(e => e.ResultId).HasName("PK__Results__9769020828CC03BD");

                entity.Property(e => e.Score).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.Exam).WithMany(p => p.Results)
                    .HasForeignKey(d => d.ExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Results__ExamId__44FF419A");

                entity.HasOne(d => d.User).WithMany(p => p.Results)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Results__UserId__45F365D3");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C4A26D203");

                entity.HasIndex(e => e.Email, "UQ__Users__A9D10534A8E49ECF").IsUnique();

                entity.Property(e => e.Class)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);
                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.School)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
