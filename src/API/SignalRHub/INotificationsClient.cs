namespace Webly.SignalRHub
{
    public interface INotificationsClient
    {
        Task ReceiveNotification(string content);
        Task MessageLength(int totalLength);
        Task ProcessingCancelled();
        Task ProcessingCompleted();
    }
}
