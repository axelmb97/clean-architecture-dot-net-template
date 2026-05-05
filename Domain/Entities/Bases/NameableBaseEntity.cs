namespace Domain.Entities.Bases
{
    public class NameableBaseEntity : IdentifiableBaseEntity
    {
        public string Name { get; set; } = null!;
    }
}
