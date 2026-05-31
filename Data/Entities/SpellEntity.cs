namespace DungeonCompanion.Data.Entities;

public class SpellEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string RuleSetId { get; set; } = null!;

    public int Level { get; set; }
    public string School { get; set; } = null!;
    public string CastingTime { get; set; } = null!;
    public int Range { get; set; }

    public string? Component { get; set; }
    public string? Duration { get; set; }
    public string? SavingThrow { get; set; }
    public string? Description { get; set; }
    public List<string> Tags { get; set; } = new();

    public Dictionary<string, string> NameTranslations { get; set; } = new();
    public Dictionary<string, string> DescriptionTranslations { get; set; } = new();

    public DateTime DatasetVersion { get; set; }
}
