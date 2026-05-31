using DungeonCompanion.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DungeonCompanion.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<CharacterEntity> Characters => Set<CharacterEntity>();
    public DbSet<SkillEntity> Skills => Set<SkillEntity>();
    public DbSet<SpellEntity> Spells => Set<SpellEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CharacterEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.CampaignId, e.OwnerId, e.Name }).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.RuleSetId).HasMaxLength(64);
            entity.Property(e => e.ConflictState).HasMaxLength(128);

            entity.Property(e => e.Attributes).AsJsonb();
            entity.Property(e => e.SpellIds).AsJsonb();
            entity.Property(e => e.SkillIds).AsJsonb();
            entity.Property(e => e.FeatIds).AsJsonb();
            entity.Property(e => e.WeaponIds).AsJsonb();
            entity.Property(e => e.SystemData).AsJsonb();
            entity.Property(e => e.FieldMeta).AsJsonb();
        });

        modelBuilder.Entity<SkillEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.RuleSetId, e.Name }).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(128);
            entity.Property(e => e.RuleSetId).HasMaxLength(64);
            entity.Property(e => e.KeyAbility).HasMaxLength(8);

            entity.Property(e => e.NameTranslations).AsJsonb();
            entity.Property(e => e.DescriptionTranslations).AsJsonb();
        });

        modelBuilder.Entity<SpellEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.RuleSetId, e.Name }).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.RuleSetId).HasMaxLength(64);
            entity.Property(e => e.School).HasMaxLength(64);
            entity.Property(e => e.CastingTime).HasMaxLength(64);
            entity.Property(e => e.Component).HasMaxLength(32);
            entity.Property(e => e.Duration).HasMaxLength(128);
            entity.Property(e => e.SavingThrow).HasMaxLength(64);

            entity.Property(e => e.Tags).AsJsonb();
            entity.Property(e => e.NameTranslations).AsJsonb();
            entity.Property(e => e.DescriptionTranslations).AsJsonb();
        });
    }
}
