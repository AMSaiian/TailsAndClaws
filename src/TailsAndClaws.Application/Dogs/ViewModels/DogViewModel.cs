namespace TailsAndClaws.Application.Dogs.ViewModels;

public record DogViewModel
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required decimal TailLengthInMeters { get; init; }

    public required decimal WeightInKg { get; init; }

    public required string Color { get; init; }
}
