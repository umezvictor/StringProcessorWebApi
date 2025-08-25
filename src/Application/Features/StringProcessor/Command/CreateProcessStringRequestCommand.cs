using MediatR;

namespace Application.Features.StringProcessor.Command
{
    public class CreateProcessStringRequestCommand : IRequest<bool>
    {
        public string Input { get; set; } = string.Empty;


    }
}
