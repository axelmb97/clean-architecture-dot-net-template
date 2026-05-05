using Domain.Entities.Bases;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Persistence.Data.Configurations.BaseConfigurations
{
    internal static class IdentifiableBaseEntityConfiguration
    {
        public static void Configure<T>(EntityTypeBuilder<T> entityTypeConfiguration) where T : IdentifiableBaseEntity
        {
            entityTypeConfiguration.HasKey(ent => ent.Id);
        }
    }
}
