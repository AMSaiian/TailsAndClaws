using System.Diagnostics.CodeAnalysis;

namespace TailsAndClaws.Common.Contract.Requests.Dogs;

public record CreateDogRequest
{
    private readonly string _name;
    private readonly string _color;

    public required string Name
    {
        get => _name;
        [MemberNotNull(nameof(_name))] init => _name = value.Trim();
    }

    public required string Color
    {
        get => _color;
        [MemberNotNull(nameof(_color))] init => _color = value.Trim();
    }

    public required decimal TailLengthInMeters { get; init; }

    public required decimal WeightInKg { get; init; }
}
