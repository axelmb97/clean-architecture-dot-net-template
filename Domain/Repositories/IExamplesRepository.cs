using Domain.Entities;

namespace Domain.Repositories
{
    public interface IExamplesRepository
    {
        Task<List<Example>> GetAll(CancellationToken cancellationToken);
    }
}
