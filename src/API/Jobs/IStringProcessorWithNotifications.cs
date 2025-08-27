using Domain.Procesor;

namespace Webly.Jobs
{
    public interface IStringProcessorWithNotifications
    {
        Task ProcessStringAndSendNotifications(string processedString, string userId, ProcessStringRequest request,
            CancellationToken cancellationToken);
    }
}
