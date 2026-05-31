using DungeonCompanion.Contracts;

namespace DungeonCompanion.Data.Entities;

public class CharacterEntity
{
    public Guid Id { get; set; }
    public Guid CampaignId { get; set; }
    public Guid OwnerId { get; set; }
    public string Name { get; set; } = null!;
    public string RuleSetId { get; set; } = null!;

    public int Level { get; set; }
    public int Experience { get; set; }
    public int CurrentHp { get; set; }
    public int MaxHp { get; set; }

    public Dictionary<string, int> Attributes { get; set; } = new();

    public List<Guid> SpellIds { get; set; } = new();
    public List<Guid> SkillIds { get; set; } = new();
    public List<Guid> FeatIds { get; set; } = new();
    public List<Guid> WeaponIds { get; set; } = new();

    public Dictionary<string, object?> SystemData { get; set; } = new();

    public DateTime UpdatedAt { get; set; }
    public string? ConflictState { get; set; }
    public Dictionary<string, SyncFieldMeta> FieldMeta { get; set; } = new();

    public string? Notes { get; set; }
    public string? PortraitUrl { get; set; }
}
