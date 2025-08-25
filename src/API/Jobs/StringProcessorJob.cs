using Application.Abstractions.Data;
using Application.Abstractions.Services;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Shared.Enums;
using Webly.SignalRHub;

namespace Webly.Jobs
{
    public class StringProcessorJob(ILogger<StringProcessorJob> logger,
        IHubContext<NotificationHub, INotificationsClient> hubContext,
        IProcessStringRequestRepository processStringRequestRepository, IStringProcessor stringProcessor)
    {
        [AutomaticRetry(Attempts = 5)]
        public async Task ExecuteAsync(string userId, CancellationToken cancellationToken)
        {

            try
            {
                await hubContext.Clients.User(userId).ReceiveNotification(JobStatusTypes.PROCESSING_STARTED.ToString());

                var request = await processStringRequestRepository.GetUnCompletedRequestByUserIdAsync(userId, cancellationToken);

                if (request != null)
                {
                    string processedString = stringProcessor.ProcessString(request.InputString);
                    if (!string.IsNullOrEmpty(processedString))
                    {
                        foreach (char character in processedString)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            await hubContext.Clients.User(userId).ReceiveNotification(character.ToString());
                            await Task.Delay(1000, cancellationToken);
                        }
                        request.IsCompleted = true;
                        await processStringRequestRepository.UpdateAsync(request, cancellationToken);

                        logger.LogInformation("Processing completed");
                        await hubContext.Clients.User(userId).ReceiveNotification(JobStatusTypes.PROCESSING_COMPLETED.ToString());

                    }
                }

            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("User cancelled the operation");
                await hubContext.Clients.User(userId).ReceiveNotification(JobStatusTypes.PROCESSING_CANCELLED.ToString());

            }
        }

    }
}
