using ClassAssignmentApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassAssignmentApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<ClassSession> ClassSessions { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentClassSession> StudentClassSessions { get; set; }
        public DbSet<UnitedState> UnitedStates { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<CourseOfferings> CoursesOfferings { get; set; }
    }
}
