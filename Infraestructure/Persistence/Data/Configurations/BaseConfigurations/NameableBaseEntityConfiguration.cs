using Domain.Entities.Bases;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Persistence.Data.Configurations.BaseConfigurations
{
    internal static class NameableBaseEntityConfiguration
    {
        public static void Configure<T>(EntityTypeBuilder<T> entityTypeConfiguration) where T : NameableBaseEntity
        {
            IdentifiableBaseEntityConfiguration.Configure(entityTypeConfiguration);

            entityTypeConfiguration
                .Property(ent => ent.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
