using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Features.StringProcessor.Command;
using Domain.Procesor;
using Moq;
using Shared;
using Shouldly;

namespace TestSuite.UnitTests
{

    public class StringProcessorTests
    {
        private readonly Mock<IProcessStringRequestRepository> repositoryMock = new();
        private readonly Mock<IUserContext> _userContextMock = new();

        private readonly CreateProcessStringRequestCommandHandler _serviceToTest;

        public StringProcessorTests()
        {

            _serviceToTest = new CreateProcessStringRequestCommandHandler(_userContextMock.Object, repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenGivenValidRequest_ShouldReturnTrue()
        {
            var userId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var command = new CreateProcessStringRequestCommand(Guid.NewGuid(), "Hello World");

            _userContextMock.Setup(x => x.UserId).Returns(userId);

            var request = new ProcessStringRequest
            {
                Id = Guid.NewGuid().ToString(),
                IsCompleted = false,
                IsCancelled = false,
                UserId = userId.ToString(),
                InputString = command.Input
            };

            repositoryMock.Setup(x => x.CreateRequestAsync(It.IsAny<ProcessStringRequest>(), cancellationToken)).ReturnsAsync(1);

            var result = await _serviceToTest.Handle(command, cancellationToken);


            result.IsSuccess.ShouldBeTrue();
            result.ShouldBeOfType<Result>();
        }


        [Fact]
        public async Task Handle_WhenGivenNullOrEmptyInputString_ShouldReturnFalse()
        {

            var cancellationToken = CancellationToken.None;
            var command = new CreateProcessStringRequestCommand(Guid.NewGuid(), "Hello World");

            repositoryMock.Setup(x => x.GetUnCompletedRequestByUserIdAsync(It.IsAny<string>(), cancellationToken))
                .ReturnsAsync(It.IsAny<ProcessStringRequest>());

            var result = await _serviceToTest.Handle(command, cancellationToken);

            result.IsFailure.ShouldBeTrue();
            result.ShouldBeOfType<Result>();
        }


        [Fact]
        public async Task Handle_WhenUserIdNotPresent_ShouldReturnFalse()
        {

            var cancellationToken = CancellationToken.None;
            var command = new CreateProcessStringRequestCommand(Guid.NewGuid(), "Hello World");

            _userContextMock.Setup(x => x.UserId).Returns(Guid.Empty);

            repositoryMock.Setup(x => x.GetUnCompletedRequestByUserIdAsync(It.IsAny<string>(), cancellationToken))
                .ReturnsAsync(It.IsAny<ProcessStringRequest>());


            var result = await _serviceToTest.Handle(command, cancellationToken);

            result.IsFailure.ShouldBeTrue();
            result.ShouldBeOfType<Result>();
        }


        [Fact]
        public async Task Handle_WhenUserIdHasPendingRequest_ShouldReturnFalse()
        {

            var cancellationToken = CancellationToken.None;
            var command = new CreateProcessStringRequestCommand(Guid.NewGuid(), "Hello World");

            var pendingRequest = new ProcessStringRequest
            {
                Id = Guid.NewGuid().ToString(),
                InputString = "Hello",
                IsCompleted = false,
                IsCancelled = false,
                UserId = Guid.NewGuid().ToString()
            };

            repositoryMock.Setup(x => x.GetUnCompletedRequestByUserIdAsync(It.IsAny<string>(),
                cancellationToken)).ReturnsAsync(pendingRequest);


            var result = await _serviceToTest.Handle(command, cancellationToken);

            result.IsFailure.ShouldBeTrue();
            result.ShouldBeOfType<Result>();
        }
    }
}


