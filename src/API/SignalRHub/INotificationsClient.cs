namespace Webly.SignalRHub
{
    public interface INotificationsClient
    {
        Task ReceiveNotification(string content);
    }
}
