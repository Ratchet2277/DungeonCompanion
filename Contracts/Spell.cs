namespace DungeonCompanion.Contracts;

public sealed record Spell
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string RuleSetId { get; init; }

    public required int Level { get; init; }
    public required string School { get; init; }
    public required string CastingTime { get; init; }
    public required int Range { get; init; }

    public string? Component { get; init; }
    public string? Duration { get; init; }
    public string? SavingThrow { get; init; }
    public string? Description { get; init; }
    public IReadOnlyList<string>? Tags { get; init; }

    public IReadOnlyDictionary<string, string> NameTranslations { get; init; }
        = new Dictionary<string, string>();
    public IReadOnlyDictionary<string, string> DescriptionTranslations { get; init; }
        = new Dictionary<string, string>();

    public required DateTime DatasetVersion { get; init; }
}
