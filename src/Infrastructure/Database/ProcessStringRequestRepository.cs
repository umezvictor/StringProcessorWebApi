using Application.Abstractions.Data;
using Domain.Procesor;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{

    public class ProcessStringRequestRepository(IApplicationDbContext _context) : IProcessStringRequestRepository
    {
        public async Task<int> CreateRequestAsync(ProcessStringRequest request, CancellationToken cancellationToken)
        {
            await _context.ProcessStringRequests.AddAsync(request);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<ProcessStringRequest?> GetUnCompletedRequestByUserIdAsync(string userId, CancellationToken? cancellationToken)
        {
            return await _context.ProcessStringRequests.
                   Where(x => x.UserId == userId && x.IsCompleted == false && x.IsCancelled == false).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(ProcessStringRequest request, CancellationToken cancellationToken)
        {
            _context.ProcessStringRequests.Update(request);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
