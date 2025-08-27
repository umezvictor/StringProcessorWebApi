using Domain.Idempotency;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class IdempotencyRequestConfiguration : IEntityTypeConfiguration<IdempotencyRequest>
    {
        public void Configure(EntityTypeBuilder<IdempotencyRequest> builder)
        {
            builder.ToTable("IdempotentRequests");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired();
        }
    }
}
