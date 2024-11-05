using FluentValidation;
using TailsAndClaws.Application.Dogs.Constants;

namespace TailsAndClaws.Application.Dogs.Commands.CreateDog;

public sealed class CreateDogCommandValidator : AbstractValidator<CreateDogCommand>
{
    public CreateDogCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.NameLength);

        RuleFor(command => command.Color)
            .NotEmpty()
            .MaximumLength(ValidationConstants.ColorLength);

        RuleFor(command => command.WeightInKg)
            .GreaterThan(0);

        RuleFor(command => command.TailLengthInMeters)
            .GreaterThan(0);
    }
}
