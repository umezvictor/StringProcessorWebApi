namespace Application.Idempotency
{
    public abstract record IdempotentCommand(Guid requestId);

}
