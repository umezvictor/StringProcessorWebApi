using Application.Abstractions.Data;
using Application.Abstractions.Services;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
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

                var request = await processStringRequestRepository.GetUnCompletedRequestByUserIdAsync(userId, cancellationToken);

                if (request != null)
                {
                    string processedString = stringProcessor.ProcessString(request.InputString);
                    if (!string.IsNullOrEmpty(processedString))
                    {
                        await hubContext.Clients.User(userId).MessageLength(processedString.Length);

                        foreach (char character in processedString)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            await hubContext.Clients.User(userId).ReceiveNotification(character.ToString());
                            await Task.Delay(1000, cancellationToken);
                        }
                        request.IsCompleted = true;
                        await processStringRequestRepository.UpdateAsync(request, cancellationToken);

                        logger.LogInformation("Processing completed");
                        await hubContext.Clients.User(userId).ProcessingCompleted();

                    }
                }

            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("User cancelled the operation");
                await hubContext.Clients.User(userId).ProcessingCancelled();

            }
        }

    }
}
