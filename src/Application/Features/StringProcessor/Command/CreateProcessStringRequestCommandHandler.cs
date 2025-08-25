using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.Procesor;
using MediatR;

namespace Application.Features.StringProcessor.Command
{
    public sealed class CreateProcessStringRequestCommandHandler(IUserContext userContext,
        IProcessStringRequestRepository processStringRequestRepository) : IRequestHandler<CreateProcessStringRequestCommand, bool>
    {

        public async Task<bool> Handle(CreateProcessStringRequestCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Input))
                return false;

            if (userContext.UserId == Guid.Empty)
                return false;


            var result = await processStringRequestRepository.CreateRequestAsync(new ProcessStringRequest
            {
                Id = Guid.NewGuid().ToString(),
                IsCompleted = false,
                UserId = userContext.UserId.ToString(),
                InputString = command.Input
            }, cancellationToken);


            return result > 0 ? true : false;

        }

    }
}