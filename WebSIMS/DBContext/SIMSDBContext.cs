using Microsoft.EntityFrameworkCore;
using WebSIMS.DBContext.Entities;
using WebSIMS.Models;

namespace WebSIMS.DBContext 
{
    public class SIMSDBContext : DbContext
    {
        public SIMSDBContext(DbContextOptions<SIMSDBContext> options) : base(options) { }

        public DbSet<Courses> CoursesDb { get; set; }
        public DbSet<Users> UsersDb { get; set; }
        public DbSet<Student> StudentsDb { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<StudentCourses> StudentCoursesDb { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table Users
            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<Users>().HasKey("UserID");
            modelBuilder.Entity<Users>().HasIndex("Username").IsUnique();
            modelBuilder.Entity<Users>().Property("Role").HasDefaultValue("Student");

            // Table Courses
            modelBuilder.Entity<Courses>().ToTable("Courses");
            modelBuilder.Entity<Courses>().HasKey("CourseID");
            modelBuilder.Entity<Courses>().HasIndex("CourseCode").IsUnique();

            // Table Students
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Student>().HasKey(s => s.StudentID);

            // Table Faculty
            modelBuilder.Entity<Faculty>().ToTable("Faculties");
            modelBuilder.Entity<Faculty>().HasKey(f => f.FacultyID);

            // Relationship User and Student
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey("UserID");

            // Relationship User and Faculty
            modelBuilder.Entity<Faculty>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserID);

            // Table StudentCourses
            modelBuilder.Entity<StudentCourses>().ToTable("StudentCourses");
            modelBuilder.Entity<StudentCourses>().HasKey(sc => sc.StudentCourseID);

            // Relationship Student and StudentCourses
            modelBuilder.Entity<StudentCourses>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentID);

            // Relationship Courses and StudentCourses
            modelBuilder.Entity<StudentCourses>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseID);
        }
    }
}