using Automation.Alura.Domain.Entities.v1;
using Automation.Alura.Infrastructure.Repository.Configurations.v1;
using Microsoft.EntityFrameworkCore;

namespace Automation.Alura.Infrastructure.Repository;

public class AluraDbContext : DbContext
{
    public AluraDbContext(DbContextOptions<AluraDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CourseConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public required DbSet<Course> Courses { get; set; }
}