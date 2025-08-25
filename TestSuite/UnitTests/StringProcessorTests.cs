using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Features.StringProcessor.Command;
using Domain.Procesor;
using Moq;
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
            var command = new CreateProcessStringRequestCommand { Input = "Hello World" };

            _userContextMock.Setup(x => x.UserId).Returns(userId);

            var request = new ProcessStringRequest
            {
                Id = Guid.NewGuid().ToString(),
                IsCompleted = false,
                UserId = userId.ToString(),
                InputString = command.Input
            };

            repositoryMock.Setup(x => x.CreateRequestAsync(It.IsAny<ProcessStringRequest>(), cancellationToken)).ReturnsAsync(1);

            var result = await _serviceToTest.Handle(command, cancellationToken);


            result.ShouldBeTrue();
            result.ShouldBeOfType<bool>();
        }


        [Fact]
        public async Task Handle_WhenGivenNullOrEmptyInputString_ShouldReturnFalse()
        {

            var cancellationToken = CancellationToken.None;
            var command = new CreateProcessStringRequestCommand { Input = "" };

            var result = await _serviceToTest.Handle(command, cancellationToken);

            result.ShouldBeFalse();
            result.ShouldBeOfType<bool>();
        }


        [Fact]
        public async Task Handle_WhenUserIdNotPresent_ShouldReturnFalse()
        {

            var cancellationToken = CancellationToken.None;
            var command = new CreateProcessStringRequestCommand { Input = "Hello World" };
            _userContextMock.Setup(x => x.UserId).Returns(Guid.Empty);

            var result = await _serviceToTest.Handle(command, cancellationToken);

            result.ShouldBeFalse();
            result.ShouldBeOfType<bool>();
        }
    }
}


