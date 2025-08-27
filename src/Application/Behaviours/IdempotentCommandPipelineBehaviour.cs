using Application.Idempotency;
using MediatR;

namespace Application.Behaviours
{
    internal sealed class IdempotentCommandPipelineBehaviour<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse> where TRequest : IdempotentCommand
    {
        private readonly IIdempotencyService _idempotencyService;

        public IdempotentCommandPipelineBehaviour(IIdempotencyService idempotencyService)
        {
            _idempotencyService = idempotencyService;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (await _idempotencyService.RequestExistsAsync(request.requestId))
            {
                return default!; //default response for the TResponse type
            }

            await _idempotencyService.CreateRequestAsync(request.requestId, typeof(TRequest).Name);

            //run command and return response
            var response = await next();
            return response;

        }
    }
}
