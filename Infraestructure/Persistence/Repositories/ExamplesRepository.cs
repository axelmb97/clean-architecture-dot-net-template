using Domain.Entities;
using Domain.Repositories;
using Infraestructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Persistence.Repositories
{
    internal class ExamplesRepository : IExamplesRepository
    {
        private readonly AppDbContext _context;

        public ExamplesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Example>> GetAll(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await this._context.Examples.ToListAsync();
        }
    }
}
