using Microsoft.EntityFrameworkCore;
using ClassAssignmentWEBAPI.Models;

namespace ClassAssignmentWEBAPI.Data
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
        public DbSet<StudentClassSession> StudentClassSessions { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<UnitedState> UnitedStates { get; set; }
        public DbSet<WorkingDailySchedule> WorkingDailySchedules { get; set; }
        public DbSet<WorkingDailyConflicts> WorkingDailyConflicts { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<CourseOfferings> CourseOfferings { get; set; }
        public DbSet<InstructorSchedule> InstructorSchedules { get; set; }
        public DbSet<StudentSchedule> StudentSchedules { get; set; }



    }
}
