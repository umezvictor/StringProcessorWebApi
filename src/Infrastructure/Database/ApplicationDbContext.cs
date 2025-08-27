using Application.Abstractions.Data;
using Domain.Idempotency;
using Domain.Procesor;
using Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public sealed class ApplicationDbContext(
       DbContextOptions<ApplicationDbContext> options
   ) : IdentityDbContext<User>(options), IApplicationDbContext
    {

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ProcessStringRequest> ProcessStringRequests { get; set; }
        public DbSet<IdempotencyRequest> IdempotencyRequest { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

    }
}
