using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Webly.SignalR.CustomClients;

namespace Webly.SignalR.Hubs
{
    [Authorize]
    public sealed class NotificationHub : Hub<INotificationsClient>
    {

    }

}
