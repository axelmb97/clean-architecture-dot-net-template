using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Persistence.Data
{
    internal partial class AppDbContext { public DbSet<Example> Examples { get; set; } = null!; }
    internal class ExampleEntityConfiguration : IEntityTypeConfiguration<Example>
    {
        public void Configure(EntityTypeBuilder<Example> builder)
        {
            builder.ToTable("examples");

            builder
                .Property(ent => ent.ExampleField)
                .HasPrecision(6, 2);
        }

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Example>().HasData(
                new Example
                {
                    Id = "asd-asd-ads",
                    Name = "Example 1",
                    ExampleField = 12.34
                }
            );
        }
    }
}
