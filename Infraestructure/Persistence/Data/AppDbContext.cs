using Domain.Entities.Bases;
using Infraestructure.Persistence.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infraestructure.Persistence.Data
{
    internal partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly(),
                t => t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>) &&
                    typeof(BaseEntity).IsAssignableFrom(i.GenericTypeArguments[0])));

            ExampleEntityConfiguration.Seed(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
