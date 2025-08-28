using Application.Abstractions.Data;
using Application.Abstractions.Services;
using Domain.Procesor;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Webly.SignalR.Abstractions;
using Webly.SignalR.CustomClients;
using Webly.SignalR.Hubs;

namespace Webly.SignalR.Jobs
{
    public class StringProcessorJob(IStringProcessorWithNotifications processorWithNotifications, IStringProcessor stringProcessor,
        IProcessStringRequestRepository processStringRequestRepository, IHubContext<NotificationHub, INotificationsClient> hubContext,
        ILogger<StringProcessorJob> logger)
    {
        [AutomaticRetry(Attempts = 5)]
        public async Task ExecuteAsync(string userId, CancellationToken cancellationToken)
        {

            ProcessStringRequest? request = null;

            try
            {
                request = await processStringRequestRepository.GetUnCompletedRequestByUserIdAsync(userId, cancellationToken);
                if (request != null)
                {
                    string processedString = stringProcessor.ProcessString(request.InputString);
                    if (processedString != null)
                    {
                        await processorWithNotifications.ProcessStringAndSendNotifications(processedString, userId, request, cancellationToken);
                        request.IsCompleted = true;
                        await processStringRequestRepository.UpdateAsync(request, cancellationToken);
                    }
                }

            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("User cancelled the operation");
                if (request != null)
                {
                    request.IsCancelled = true;
                    await processStringRequestRepository.UpdateAsync(request, CancellationToken.None);
                }
                await hubContext.Clients.User(userId).ProcessingCancelled();

            }
        }



    }
}
