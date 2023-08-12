using Automation.Alura.Domain.Entities.v1;

namespace Automation.Alura.Domain.Contracts.Repositories.v1;

public interface ICourseRepository
{
    Task SaveAsync(IEnumerable<Course> courses, CancellationToken cancellationToken = default);
    Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default);
}