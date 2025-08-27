using Application.Idempotency;
using MediatR;
using Shared;

namespace Application.Features.StringProcessor.Command
{
    public record CreateProcessStringRequestCommand(Guid RequestId, string Input) : IdempotentCommand(RequestId), IRequest<Result>;


}
