using Domain.Procesor;

namespace Webly.SignalR.Abstractions
{
    public interface IStringProcessorWithNotifications
    {
        Task ProcessStringAndSendNotifications(string processedString, string userId, ProcessStringRequest request,
            CancellationToken cancellationToken);
    }
}
