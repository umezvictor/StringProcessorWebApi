using Domain.Procesor;

namespace Application.Abstractions.Data
{
    public interface IProcessStringRequestRepository
    {
        Task<int> CreateRequestAsync(ProcessStringRequest request, CancellationToken cancellationToken);
        Task<ProcessStringRequest?> GetUnCompletedRequestByUserIdAsync(string userId, CancellationToken? cancellationToken);
        Task UpdateAsync(ProcessStringRequest request, CancellationToken cancellationToken);
    }
}
