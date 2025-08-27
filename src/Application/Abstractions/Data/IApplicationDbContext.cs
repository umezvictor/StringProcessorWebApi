using Domain.Idempotency;
using Domain.Procesor;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Abstractions.Data
{
    public interface IApplicationDbContext
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ProcessStringRequest> ProcessStringRequests { get; set; }
        public DbSet<IdempotencyRequest> IdempotencyRequest { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DatabaseFacade Database { get; }
    }
}
