using FluentValidation;

namespace Application.Features.StringProcessor.Command
{
    public class CreateProcessStringRequestCommandValidator : AbstractValidator<CreateProcessStringRequestCommand>
    {
        public CreateProcessStringRequestCommandValidator()
        {

            RuleFor(p => p.Input)
            .NotEmpty()
            .NotNull()
            .WithMessage("Enter a valid input string");
        }

    }
}
