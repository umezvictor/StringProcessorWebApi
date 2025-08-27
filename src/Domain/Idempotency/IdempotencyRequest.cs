namespace Domain.Idempotency
{
    public sealed class IdempotencyRequest
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public DateTime CreatedOnUtc { get; init; }

    }
}
