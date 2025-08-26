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
    using Webly.Jobs;
    using Webly.SignalRHub;
    using Xunit;

    public class StringProcessorJobTests
    {
        private readonly Mock<IProcessStringRequestRepository> repositoryMock = new();
        private readonly Mock<ILogger<StringProcessorJob>> _mockLogger = new();
        private readonly Mock<IHubContext<NotificationHub, INotificationsClient>> _mockHubContext = new();
        private readonly Mock<IStringProcessor> _mockStringProcessor = new();
        private readonly StringProcessorJob _serviceToTest;

        public StringProcessorJobTests()
        {

            _serviceToTest = new StringProcessorJob(_mockLogger.Object, _mockHubContext.Object, repositoryMock.Object, _mockStringProcessor.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WhenGivenValidUserId_ShouldProcessSavedRequestAndSendNotifications()
        {


            // Arrange
            var userId = Guid.NewGuid().ToString();
            var request = new ProcessStringRequest { Id = Guid.NewGuid().ToString(), UserId = userId, InputString = "Hello World", IsCompleted = false };
            var cancellationToken = CancellationToken.None;

            repositoryMock.Setup(x => x.GetUnCompletedRequestByUserIdAsync(userId, cancellationToken)).ReturnsAsync(request);

            _mockStringProcessor.Setup(sp => sp.ProcessString("Hello World"))
                                .Returns("Processed string");

            _mockHubContext.Setup(x => x.Clients.User(userId).MessageLength(It.IsAny<int>()));
            _mockHubContext.Setup(x => x.Clients.User(userId).ReceiveNotification(It.IsAny<string>()));
            request.IsCompleted = true;
            repositoryMock.Setup(x => x.UpdateAsync(request, cancellationToken));

            _mockHubContext.Setup(x => x.Clients.User(userId).ProcessingCompleted());


            // Act
            await _serviceToTest.ExecuteAsync(userId, cancellationToken);

            // Assert
            _mockHubContext.Verify(x => x.Clients.User(userId).MessageLength(It.IsAny<int>()), Times.Once);
            _mockHubContext.Verify(x => x.Clients.User(userId).ProcessingCompleted(), Times.Once);

        }

        [Fact]
        public async Task ExecuteAsync_WhenOperationIsCancelled_ShouldStopProcessingAndSendProcessingCancelledNotification()
        {


            // Arrange
            var userId = Guid.NewGuid().ToString();
            var request = new ProcessStringRequest { Id = Guid.NewGuid().ToString(), UserId = userId, InputString = "Hello World", IsCompleted = false };
            var cancellationToken = CancellationToken.None;

            repositoryMock.Setup(x => x.GetUnCompletedRequestByUserIdAsync(userId, cancellationToken))
                .ThrowsAsync(new OperationCanceledException());

            _mockStringProcessor.Setup(sp => sp.ProcessString("Hello World"))
                                .Returns("Processed string");

            _mockHubContext.Setup(x => x.Clients.User(userId).MessageLength(It.IsAny<int>()));
            _mockHubContext.Setup(x => x.Clients.User(userId).ReceiveNotification(It.IsAny<string>()));
            _mockHubContext.Setup(x => x.Clients.User(userId).ProcessingCancelled());

            request.IsCompleted = true;
            repositoryMock.Setup(x => x.UpdateAsync(request, cancellationToken));




            // Act
            await _serviceToTest.ExecuteAsync(userId, cancellationToken);

            // Assert
            _mockHubContext.Verify(x => x.Clients.User(userId).ProcessingCancelled(), Times.Once);

        }
    }
}
