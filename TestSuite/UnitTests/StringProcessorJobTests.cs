namespace TestSuite.UnitTests
{
    using Application.Abstractions.Data;
    using Application.Abstractions.Services;
    using Domain.Procesor;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using System.Threading;
    using System.Threading.Tasks;
    using Webly.SignalR.Abstractions;
    using Webly.SignalR.CustomClients;
    using Webly.SignalR.Hubs;
    using Webly.SignalR.Jobs;
    using Xunit;

    public class StringProcessorJobTests
    {
        private readonly Mock<IProcessStringRequestRepository> repositoryMock = new();
        private readonly Mock<IStringProcessorWithNotifications> processorNotificationyMock = new();
        private readonly Mock<ILogger<StringProcessorJob>> _mockLogger = new();
        private readonly Mock<IHubContext<NotificationHub, INotificationsClient>> _mockHubContext = new();
        private readonly Mock<IStringProcessor> _mockStringProcessor = new();
        private readonly StringProcessorJob _serviceToTest;

        public StringProcessorJobTests()
        {

            _serviceToTest = new StringProcessorJob(processorNotificationyMock.Object, _mockStringProcessor.Object,
                repositoryMock.Object, _mockHubContext.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WhenGivenValidUserId_ShouldProcessSavedRequestAndSendNotifications()
        {


            // Arrange
            var userId = Guid.NewGuid().ToString();
            var request = new ProcessStringRequest
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                InputString = "Hello World",
                IsCompleted = false
            };

            var cancellationToken = CancellationToken.None;

            repositoryMock.Setup(x => x.GetUnCompletedRequestByUserIdAsync(userId, cancellationToken)).ReturnsAsync(request);

            _mockStringProcessor.Setup(sp => sp.ProcessString("Hello World"))
                                .Returns("Processed string");

            processorNotificationyMock.Setup(p => p.ProcessStringAndSendNotifications(It.IsAny<string>(), userId,
                request, It.IsAny<CancellationToken>()));


            request.IsCompleted = true;
            repositoryMock.Setup(x => x.UpdateAsync(request, cancellationToken));

            // Act
            await _serviceToTest.ExecuteAsync(userId, cancellationToken);

            // Assert
            processorNotificationyMock.Verify(p => p.ProcessStringAndSendNotifications(It.IsAny<string>(), userId,
                request, It.IsAny<CancellationToken>()), Times.Once);

        }




        [Fact]
        public async Task ExecuteAsync_WhenOperationIsCancelled_ShouldStopProcessingAndSendProcessingCancelledNotification()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var request = new ProcessStringRequest
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                InputString = "Hello World",
                IsCompleted = false,
                IsCancelled = false
            };
            var cancellationToken = CancellationToken.None;

            repositoryMock.Setup(x => x.GetUnCompletedRequestByUserIdAsync(userId, cancellationToken)).ReturnsAsync(request);


            processorNotificationyMock.Setup(p => p.ProcessStringAndSendNotifications(It.IsAny<string>(), userId, request,
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(new OperationCanceledException());

            _mockStringProcessor.Setup(sp => sp.ProcessString("Hello World"))
                                .Returns("Processed string");


            _mockHubContext.Setup(x => x.Clients.User(userId).ProcessingCancelled());

            request.IsCancelled = true;
            repositoryMock.Setup(x => x.UpdateAsync(request, CancellationToken.None));


            // Act
            await _serviceToTest.ExecuteAsync(userId, cancellationToken);

            // Assert
            _mockHubContext.Verify(x => x.Clients.User(userId).ProcessingCancelled(), Times.Once);
            repositoryMock.Verify(x => x.UpdateAsync(request, CancellationToken.None), Times.Once);

        }
    }
}
