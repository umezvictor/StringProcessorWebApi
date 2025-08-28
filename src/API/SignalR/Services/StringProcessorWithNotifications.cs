using Domain.Procesor;
using Microsoft.AspNetCore.SignalR;
using Webly.SignalR.Abstractions;
using Webly.SignalR.CustomClients;
using Webly.SignalR.Hubs;
using Webly.SignalR.Jobs;

namespace Webly.SignalR.Services
{

    public class StringProcessorWithNotifications(ILogger<StringProcessorJob> logger,
        IHubContext<NotificationHub, INotificationsClient> hubContext
        ) : IStringProcessorWithNotifications
    {

        public async Task ProcessStringAndSendNotifications(string processedString, string userId, ProcessStringRequest request,
            CancellationToken cancellationToken)
        {

            await hubContext.Clients.User(userId).MessageLength(processedString.Length);

            foreach (char character in processedString)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await hubContext.Clients.User(userId).ReceiveNotification(character.ToString());
                await Task.Delay(1000, cancellationToken);
            }

            logger.LogInformation("Processing completed");
            await hubContext.Clients.User(userId).ProcessingCompleted();


        }
    }
}
