using CourseCatalog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseCatalog.Api.Data
{
    public class CourseDbContext : DbContext
    {
        public CourseDbContext(DbContextOptions<CourseDbContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Classroom> Classrooms { get; set; } = null!;
        public DbSet<Class> Classes { get; set; } = null!;
        public DbSet<ClassSchedule> ClassSchedules { get; set; } = null!;
        public DbSet<OutboxEvent> OutboxEvents { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Course Fee precision
            modelBuilder.Entity<Course>()
                .Property(c => c.Fee)
                .HasPrecision(18, 2);

            // Configure relationships
            modelBuilder.Entity<Class>()
                .HasOne(c => c.Course)
                .WithMany()
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Class)
                .WithMany()
                .HasForeignKey(cs => cs.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Classroom)
                .WithMany()
                .HasForeignKey(cs => cs.ClassroomId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
