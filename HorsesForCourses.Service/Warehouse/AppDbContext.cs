using Microsoft.EntityFrameworkCore;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Service.Warehouse.Coaches;
using HorsesForCourses.Service.Warehouse.Courses;
using HorsesForCourses.Core.Domain.Accounts;
using HorsesForCourses.Service.Warehouse.Accounts;
using HorsesForCourses.Core.Domain;

namespace HorsesForCourses.Service.Warehouse;

public class AppDbContext : DbContext
{
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<UnavailableFor> UnavailableFor { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new CoachesDataConfiguration());
        modelBuilder.ApplyConfiguration(new CourseDataConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationUserDataConfiguration());
        modelBuilder.ApplyConfiguration(new UnavailableForDataConfiguration());
    }
}
