namespace Webly.SignalR.CustomClients
{
    public interface INotificationsClient
    {
        Task ReceiveNotification(string content);
        Task MessageLength(int totalLength);
        Task ProcessingCancelled();
        Task ProcessingCompleted();
    }
}
