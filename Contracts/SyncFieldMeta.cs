namespace DungeonCompanion.Contracts;

public sealed record SyncFieldMeta
{
    public required DateTime Timestamp { get; init; }
    public required string Source { get; init; }
    public object? Value { get; init; }
}
