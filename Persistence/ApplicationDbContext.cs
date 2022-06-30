using Marc2.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marc2.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
         
        }
        public DbSet<Organization>? Organizations { get; set; }
        public DbSet<Role>? Roles { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Accident>? Accidents { get; set; }
        public DbSet<SmartBracelet>? SmartBracelets { get; set; }
        public DbSet<WorkShift>? WorkShifts { get; set; }
        public DbSet<Issue>? Issues { get; set; }
        public DbSet<Assignment>? UserAssignments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasAlternateKey(u => u.Email);
            modelBuilder.Entity<SmartBracelet>().HasAlternateKey(s => s.ManufacturerCode);

            modelBuilder.Entity<Assignment>().HasOne(u => u.Issue).WithMany(i => i.Assignments).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Assignment>().HasOne(u => u.WorkShift).WithMany(u => u.Assignments).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
