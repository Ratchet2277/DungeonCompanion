namespace DungeonCompanion.Contracts;

public sealed record Character
{
    public required Guid Id { get; init; }
    public required Guid CampaignId { get; init; }
    public required Guid OwnerId { get; init; }
    public required string Name { get; init; }
    public required string RuleSetId { get; init; }

    public required int Level { get; init; }
    public required int Experience { get; init; }
    public required int CurrentHp { get; init; }
    public required int MaxHp { get; init; }

    public required IReadOnlyDictionary<string, int> Attributes { get; init; }

    public IReadOnlyList<Guid> SpellIds { get; init; } = [];
    public IReadOnlyList<Guid> SkillIds { get; init; } = [];
    public IReadOnlyList<Guid> FeatIds { get; init; } = [];
    public IReadOnlyList<Guid> WeaponIds { get; init; } = [];

    public IReadOnlyDictionary<string, object?> SystemData { get; init; }
        = new Dictionary<string, object?>();

    public required DateTime UpdatedAt { get; init; }
    public string? ConflictState { get; init; }
    public IReadOnlyDictionary<string, SyncFieldMeta> FieldMeta { get; init; }
        = new Dictionary<string, SyncFieldMeta>();

    public string? Notes { get; init; }
    public string? PortraitUrl { get; init; }
}
