using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.Procesor;
using MediatR;
using Shared;

namespace Application.Features.StringProcessor.Command
{
    public class CreateProcessStringRequestCommandHandler(IUserContext userContext,
        IProcessStringRequestRepository processStringRequestRepository) : IRequestHandler<CreateProcessStringRequestCommand, Result>
    {

        public async Task<Result> Handle(CreateProcessStringRequestCommand command, CancellationToken cancellationToken)
        {

            //check if user has any pending request
            var request = await processStringRequestRepository.GetUnCompletedRequestByUserIdAsync
                (userContext.UserId.ToString(), cancellationToken);

            if (request != null || string.IsNullOrEmpty(command.Input) || userContext.UserId == Guid.Empty)
                return Result.Failure(ProcessStringErrors.TooManyRequests);

            await processStringRequestRepository.CreateRequestAsync(new ProcessStringRequest
            {
                Id = Guid.NewGuid().ToString(),
                IsCompleted = false,
                IsCancelled = false,
                UserId = userContext.UserId.ToString(),
                InputString = command.Input
            }, cancellationToken);

            return Result.Success();
        }

    }
}