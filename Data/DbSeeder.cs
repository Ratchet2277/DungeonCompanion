using DungeonCompanion.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DungeonCompanion.Data;

public static class DbSeeder
{
    private const string RuleSet = "dnd5e";
    private static readonly DateTime DatasetVersion = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static async Task SeedAsync(AppDbContext db, CancellationToken ct = default)
    {
        await db.Database.MigrateAsync(ct);

        await SeedSkillsAsync(db, ct);
        await SeedSpellsAsync(db, ct);
        await SeedSampleCharacterAsync(db, ct);

        await db.SaveChangesAsync(ct);
    }

    private static async Task SeedSkillsAsync(AppDbContext db, CancellationToken ct)
    {
        var skills = new[]
        {
            Skill("11111111-0000-0000-0000-000000000001", "Acrobatics", "DEX",
                fr: "Acrobaties",
                descEn: "Stay on your feet in tricky situations, perform stunts.",
                descFr: "Rester debout dans des situations délicates, réaliser des cascades."),
            Skill("11111111-0000-0000-0000-000000000002", "Animal Handling", "WIS",
                fr: "Dressage",
                descEn: "Calm or control a domesticated animal.",
                descFr: "Calmer ou contrôler un animal domestiqué."),
            Skill("11111111-0000-0000-0000-000000000003", "Arcana", "INT",
                fr: "Arcanes",
                descEn: "Recall lore about spells, magic items, and planes.",
                descFr: "Connaître les sorts, objets magiques et plans d'existence."),
            Skill("11111111-0000-0000-0000-000000000004", "Athletics", "STR",
                fr: "Athlétisme",
                descEn: "Climb, jump, swim, or grapple.",
                descFr: "Grimper, sauter, nager ou lutter."),
            Skill("11111111-0000-0000-0000-000000000005", "Deception", "CHA",
                fr: "Tromperie",
                descEn: "Convincingly hide the truth.",
                descFr: "Cacher la vérité de manière convaincante."),
            Skill("11111111-0000-0000-0000-000000000006", "History", "INT",
                fr: "Histoire",
                descEn: "Recall historical events, legendary people, ancient kingdoms.",
                descFr: "Se souvenir d'événements historiques, de figures légendaires, de royaumes anciens."),
            Skill("11111111-0000-0000-0000-000000000007", "Insight", "WIS",
                fr: "Perspicacité",
                descEn: "Determine the true intentions of a creature.",
                descFr: "Déterminer les véritables intentions d'une créature."),
            Skill("11111111-0000-0000-0000-000000000008", "Intimidation", "CHA",
                fr: "Intimidation",
                descEn: "Influence someone through threats.",
                descFr: "Influencer quelqu'un par la menace."),
            Skill("11111111-0000-0000-0000-000000000009", "Investigation", "INT",
                fr: "Investigation",
                descEn: "Look for clues and make deductions.",
                descFr: "Chercher des indices et en tirer des déductions."),
            Skill("11111111-0000-0000-0000-00000000000a", "Medicine", "WIS",
                fr: "Médecine",
                descEn: "Stabilize a dying companion or diagnose an illness.",
                descFr: "Stabiliser un compagnon mourant ou diagnostiquer une maladie."),
            Skill("11111111-0000-0000-0000-00000000000b", "Nature", "INT",
                fr: "Nature",
                descEn: "Recall lore about terrain, plants, animals, weather.",
                descFr: "Connaître les terrains, plantes, animaux et phénomènes naturels."),
            Skill("11111111-0000-0000-0000-00000000000c", "Perception", "WIS",
                fr: "Perception",
                descEn: "Spot, hear, or otherwise detect the presence of something.",
                descFr: "Repérer, entendre ou détecter la présence de quelque chose."),
            Skill("11111111-0000-0000-0000-00000000000d", "Performance", "CHA",
                fr: "Représentation",
                descEn: "Delight an audience with music, dance, acting, or storytelling.",
                descFr: "Charmer un public par la musique, la danse, le théâtre ou un récit."),
            Skill("11111111-0000-0000-0000-00000000000e", "Persuasion", "CHA",
                fr: "Persuasion",
                descEn: "Influence someone with tact, social graces, or good nature.",
                descFr: "Influencer quelqu'un par le tact, la courtoisie ou la bienveillance."),
            Skill("11111111-0000-0000-0000-00000000000f", "Religion", "INT",
                fr: "Religion",
                descEn: "Recall lore about deities, rites, prayers, religious hierarchies.",
                descFr: "Connaître les divinités, rites, prières et hiérarchies religieuses."),
            Skill("11111111-0000-0000-0000-000000000010", "Sleight of Hand", "DEX",
                fr: "Escamotage",
                descEn: "Perform an act of legerdemain or manual trickery.",
                descFr: "Réaliser un tour de passe-passe ou une manipulation discrète."),
            Skill("11111111-0000-0000-0000-000000000011", "Stealth", "DEX", acp: true,
                fr: "Discrétion",
                descEn: "Conceal yourself from enemies, slink past guards.",
                descFr: "Se dissimuler aux ennemis, se faufiler devant des gardes."),
            Skill("11111111-0000-0000-0000-000000000012", "Survival", "WIS",
                fr: "Survie",
                descEn: "Follow tracks, hunt, navigate the wilderness.",
                descFr: "Suivre une piste, chasser, s'orienter en pleine nature."),
        };

        var existing = await db.Skills
            .Where(s => s.RuleSetId == RuleSet)
            .Select(s => s.Id)
            .ToListAsync(ct);

        var existingSet = existing.ToHashSet();
        var toInsert = skills.Where(s => !existingSet.Contains(s.Id));
        await db.Skills.AddRangeAsync(toInsert, ct);
    }

    private static async Task SeedSpellsAsync(AppDbContext db, CancellationToken ct)
    {
        var spells = new[]
        {
            Spell(
                "22222222-0000-0000-0000-000000000001",
                "Fire Bolt", 0, "Evocation", "1 action", 120,
                component: "V, S", duration: "Instantaneous",
                descriptionEn: "You hurl a mote of fire at a creature or object within range.",
                fr: "Trait de feu",
                descriptionFr: "Vous projetez une particule de feu sur une créature ou un objet à portée.",
                tags: ["damage", "fire", "cantrip"]),
            Spell(
                "22222222-0000-0000-0000-000000000002",
                "Mage Hand", 0, "Conjuration", "1 action", 30,
                component: "V, S", duration: "1 minute",
                descriptionEn: "A spectral, floating hand appears at a point you choose within range.",
                fr: "Main du mage",
                descriptionFr: "Une main spectrale flottante apparaît en un point de votre choix à portée.",
                tags: ["utility", "cantrip"]),
            Spell(
                "22222222-0000-0000-0000-000000000003",
                "Magic Missile", 1, "Evocation", "1 action", 120,
                component: "V, S", duration: "Instantaneous",
                descriptionEn: "You create three glowing darts of magical force.",
                fr: "Projectile magique",
                descriptionFr: "Vous créez trois fléchettes scintillantes de force magique.",
                tags: ["damage", "force"]),
            Spell(
                "22222222-0000-0000-0000-000000000004",
                "Shield", 1, "Abjuration", "1 reaction", 0,
                component: "V, S", duration: "1 round",
                descriptionEn: "An invisible barrier of magical force appears and protects you.",
                fr: "Bouclier",
                descriptionFr: "Une barrière invisible de force magique apparaît et vous protège.",
                tags: ["defense", "reaction"]),
            Spell(
                "22222222-0000-0000-0000-000000000005",
                "Cure Wounds", 1, "Evocation", "1 action", 5,
                component: "V, S", duration: "Instantaneous",
                descriptionEn: "A creature you touch regains hit points.",
                fr: "Soin des blessures",
                descriptionFr: "Une créature que vous touchez récupère des points de vie.",
                tags: ["healing"]),
            Spell(
                "22222222-0000-0000-0000-000000000006",
                "Fireball", 3, "Evocation", "1 action", 150,
                component: "V, S, M", duration: "Instantaneous",
                savingThrow: "Dexterity",
                descriptionEn: "A bright streak flashes from your pointing finger to a point you choose.",
                fr: "Boule de feu",
                descriptionFr: "Un trait lumineux jaillit de votre doigt tendu vers un point de votre choix.",
                tags: ["damage", "fire", "area"]),
        };

        var existing = await db.Spells
            .Where(s => s.RuleSetId == RuleSet)
            .Select(s => s.Id)
            .ToListAsync(ct);

        var existingSet = existing.ToHashSet();
        var toInsert = spells.Where(s => !existingSet.Contains(s.Id));
        await db.Spells.AddRangeAsync(toInsert, ct);
    }

    private static async Task SeedSampleCharacterAsync(AppDbContext db, CancellationToken ct)
    {
        var characterId = Guid.Parse("33333333-0000-0000-0000-000000000001");
        if (await db.Characters.AnyAsync(c => c.Id == characterId, ct))
            return;

        db.Characters.Add(new CharacterEntity
        {
            Id = characterId,
            CampaignId = Guid.Parse("44444444-0000-0000-0000-000000000001"),
            OwnerId = Guid.Parse("55555555-0000-0000-0000-000000000001"),
            Name = "Thalindra Moonwhisper",
            RuleSetId = RuleSet,
            Level = 3,
            Experience = 900,
            CurrentHp = 22,
            MaxHp = 24,
            Attributes = new Dictionary<string, int>
            {
                ["STR"] = 10,
                ["DEX"] = 16,
                ["CON"] = 14,
                ["INT"] = 17,
                ["WIS"] = 12,
                ["CHA"] = 13,
            },
            SpellIds =
            [
                Guid.Parse("22222222-0000-0000-0000-000000000001"),
                Guid.Parse("22222222-0000-0000-0000-000000000002"),
                Guid.Parse("22222222-0000-0000-0000-000000000003"),
                Guid.Parse("22222222-0000-0000-0000-000000000004"),
            ],
            SkillIds =
            [
                Guid.Parse("11111111-0000-0000-0000-000000000003"),
                Guid.Parse("11111111-0000-0000-0000-000000000009"),
                Guid.Parse("11111111-0000-0000-0000-000000000011"),
            ],
            SystemData = new Dictionary<string, object?>
            {
                ["class"] = "Wizard",
                ["race"] = "High Elf",
                ["background"] = "Sage",
            },
            UpdatedAt = DateTime.UtcNow,
            Notes = "Seed character for local development.",
        });
    }

    private static SkillEntity Skill(
        string id, string nameEn, string keyAbility,
        string fr, string? descEn = null, string? descFr = null,
        bool trainedOnly = false, bool acp = false) =>
        new()
        {
            Id = Guid.Parse(id),
            Name = nameEn,
            RuleSetId = RuleSet,
            KeyAbility = keyAbility,
            TrainedOnly = trainedOnly,
            ArmorCheckPenalty = acp,
            Description = descEn,
            NameTranslations = new Dictionary<string, string>
            {
                ["en"] = nameEn,
                ["fr"] = fr,
            },
            DescriptionTranslations = BuildDescriptionTranslations(descEn, descFr),
            DatasetVersion = DatasetVersion,
        };

    private static SpellEntity Spell(
        string id, string nameEn, int level, string school, string castingTime, int range,
        string fr,
        string? component = null, string? duration = null, string? savingThrow = null,
        string? descriptionEn = null, string? descriptionFr = null, List<string>? tags = null) =>
        new()
        {
            Id = Guid.Parse(id),
            Name = nameEn,
            RuleSetId = RuleSet,
            Level = level,
            School = school,
            CastingTime = castingTime,
            Range = range,
            Component = component,
            Duration = duration,
            SavingThrow = savingThrow,
            Description = descriptionEn,
            Tags = tags ?? [],
            NameTranslations = new Dictionary<string, string>
            {
                ["en"] = nameEn,
                ["fr"] = fr,
            },
            DescriptionTranslations = BuildDescriptionTranslations(descriptionEn, descriptionFr),
            DatasetVersion = DatasetVersion,
        };

    private static Dictionary<string, string> BuildDescriptionTranslations(string? en, string? fr)
    {
        var dict = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(en)) dict["en"] = en;
        if (!string.IsNullOrEmpty(fr)) dict["fr"] = fr;
        return dict;
    }
}
