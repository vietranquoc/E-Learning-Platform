using ELearningPlatform_Entity.EntityModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Data.DBContext
{
    public class ELearningPlatformDbContext : DbContext
    {
        public ELearningPlatformDbContext(DbContextOptions<ELearningPlatformDbContext> options) : base(options) { }
        
        #region DbSets
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserDetail> UserDetails { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<UserQuizResult> UserQuizResults { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Table User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.HashedPassword).IsRequired().HasMaxLength(255);
            });

            // Table UserDetail (One-to-One with User)
            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.HasKey(ud => ud.Id);
                entity.Property(ud => ud.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(ud => ud.Email).IsUnique(); // Email is unique
                entity.HasOne(ud => ud.User)
                      .WithOne(u => u.UserDetail)
                      .HasForeignKey<UserDetail>(ud => ud.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // Table Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.RoleName).IsRequired().HasMaxLength(50);
            });

            // Table UserRole (Many-to-Many: User <-> Role)
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.User)
                      .WithMany(u => u.UserRole)
                      .HasForeignKey(ur => ur.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(ur => ur.Role)
                      .WithMany(r => r.UserRole)
                      .HasForeignKey(ur => ur.RoleId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // Table Course
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Title).IsRequired().HasMaxLength(255);
                entity.Property(c => c.Description).HasMaxLength(int.MaxValue);
            });

            // Table Enrollment (Many-to-Many: User <-> Course)
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CourseId });

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Enrollment)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Course)
                      .WithMany(c => c.Enrollment)
                      .HasForeignKey(e => e.CourseId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // Table Lesson (One-to-Many: Course -> Lesson)
            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Title).IsRequired().HasMaxLength(255);

                entity.HasOne(l => l.Course)
                      .WithMany(c => c.Lesson)
                      .HasForeignKey(l => l.CourseId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // Table Quiz
            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Title).IsRequired().HasMaxLength(255);

                entity.HasOne(q => q.Course)
                      .WithMany(c => c.Quiz)
                      .HasForeignKey(q => q.CourseId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // Table Question (One-to-Many: Quiz -> Question)
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Content).IsRequired();

                entity.HasOne(q => q.Quiz)
                      .WithMany(qz => qz.Question)
                      .HasForeignKey(q => q.QuizId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // Table Answer (One-to-Many: Question -> Answer)
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Content).IsRequired();

                entity.HasOne(a => a.Question)
                      .WithMany(q => q.Answer)
                      .HasForeignKey(a => a.QuestionId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // Table UserQuizResult (Many-to-Many: User <-> Quiz)
            modelBuilder.Entity<UserQuizResult>(entity =>
            {
                entity.HasKey(uqr => new { uqr.UserId, uqr.QuizId });

                entity.HasOne(uqr => uqr.User)
                      .WithMany(u => u.UserQuizResult)
                      .HasForeignKey(uqr => uqr.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(uqr => uqr.Quiz)
                      .WithMany(q => q.UserQuizResult)
                      .HasForeignKey(uqr => uqr.QuizId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // Table Notification
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Message).IsRequired().HasMaxLength(int.MaxValue);
                entity.Property(n => n.CreatedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(n => n.User)
                      .WithMany(u => u.Notification)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
