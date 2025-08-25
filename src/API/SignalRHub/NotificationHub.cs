using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Webly.SignalRHub
{
    [Authorize]
    public sealed class NotificationHub : Hub<INotificationsClient>
    {

    }

}
