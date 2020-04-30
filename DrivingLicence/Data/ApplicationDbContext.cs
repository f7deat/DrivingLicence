using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DrivingLicence.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Consequence> Consequences { get; set; }
    }
}
