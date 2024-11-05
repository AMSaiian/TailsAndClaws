using System.Collections.ObjectModel;
using System.Linq.Expressions;
using AMSaiian.Shared.Domain.Entities;
using AMSaiian.Shared.Domain.Interfaces;
using TailsAndClaws.Domain.Constants;

namespace TailsAndClaws.Domain.Entities;

public class Dog : BaseEntity, IOrdering
{
    private string _name = default!;

    public static ReadOnlyDictionary<string, dynamic> OrderedBy { get; } = new(
        new Dictionary<string, dynamic>
        {
            { DogConstants.OrderingBy.Name, (Expression<Func<Dog, string>>)(dog => dog.Name) },
            { DogConstants.OrderingBy.Color, (Expression<Func<Dog, string>>)(dog => dog.Color) },
            { DogConstants.OrderingBy.Weight, (Expression<Func<Dog, decimal>>)(dog => dog.WeightInKg) },
            { DogConstants.OrderingBy.TailLength, (Expression<Func<Dog, decimal>>)(dog => dog.TailLengthInMeters) }
        });

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            NormalizedName = value.ToUpperInvariant();
        }
    }

    public string NormalizedName { get; private set; } = default!;

    public decimal TailLengthInMeters { get; set; }

    public decimal WeightInKg { get; set; }

    public string Color { get; set; } = default!;
}
