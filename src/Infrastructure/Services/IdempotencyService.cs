using Application.Abstractions.Data;
using Application.Idempotency;
using Domain.Idempotency;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    internal sealed class IdempotencyService(IApplicationDbContext dbContext) : IIdempotencyService
    {
        public async Task CreateRequestAsync(Guid requestId, string name)
        {
            var idempotencyRequest = new IdempotencyRequest
            {
                Id = requestId,
                Name = name,
                CreatedOnUtc = DateTime.UtcNow
            };

            await dbContext.IdempotencyRequest.AddAsync(idempotencyRequest);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> RequestExistsAsync(Guid requestId)
        {
            return await dbContext.IdempotencyRequest.AnyAsync(r => r.Id == requestId);
        }
    }
}
