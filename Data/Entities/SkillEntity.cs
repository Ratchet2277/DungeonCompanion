namespace DungeonCompanion.Data.Entities;

public class SkillEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string RuleSetId { get; set; } = null!;

    public string KeyAbility { get; set; } = null!;
    public bool TrainedOnly { get; set; }
    public bool ArmorCheckPenalty { get; set; }

    public string? Description { get; set; }

    public Dictionary<string, string> NameTranslations { get; set; } = new();
    public Dictionary<string, string> DescriptionTranslations { get; set; } = new();

    public DateTime DatasetVersion { get; set; }
}
