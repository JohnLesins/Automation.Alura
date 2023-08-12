using Automation.Alura.Domain.Contracts.Repositories.v1;
using Automation.Alura.Domain.Entities.v1;
using Microsoft.EntityFrameworkCore;

namespace Automation.Alura.Infrastructure.Repository.Repositories.v1;

public class CourseRepository : ICourseRepository
{
    private readonly IDbContextFactory<AluraDbContext> _dbContextFactory;

    public CourseRepository(IDbContextFactory<AluraDbContext> contextoFactory)
    {
        _dbContextFactory = contextoFactory;
    }

    public async Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.Courses.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task SaveAsync(IEnumerable<Course> cursos, CancellationToken cancellationToken = default)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await dbContext.AddRangeAsync(cursos, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
