namespace DungeonCompanion.Contracts;

public sealed record Skill
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string RuleSetId { get; init; }

    public required string KeyAbility { get; init; }
    public required bool TrainedOnly { get; init; }
    public required bool ArmorCheckPenalty { get; init; }

    public string? Description { get; init; }

    public IReadOnlyDictionary<string, string> NameTranslations { get; init; }
        = new Dictionary<string, string>();
    public IReadOnlyDictionary<string, string> DescriptionTranslations { get; init; }
        = new Dictionary<string, string>();

    public required DateTime DatasetVersion { get; init; }
}
