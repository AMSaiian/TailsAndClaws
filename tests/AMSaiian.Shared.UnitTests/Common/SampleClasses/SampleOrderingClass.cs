using System.Collections.ObjectModel;
using System.Linq.Expressions;
using AMSaiian.Shared.Domain.Interfaces;

namespace AMSaiian.Shared.UnitTests.Common.SampleClasses;

public class SampleOrderingClass : IOrdering
{
    public static ReadOnlyDictionary<string, dynamic> OrderedBy { get; } = new(
        new Dictionary<string, dynamic>
        {
            {
                SampleOrderingClassConstants.OrderingBy.Textual, 
                (Expression<Func<SampleOrderingClass, string>>)(x => x.Textual)
            },
            {
                SampleOrderingClassConstants.OrderingBy.TextualNested,
                (Expression<Func<SampleOrderingClass, string>>)(x => x.Nested.Textual)
            },
            {
                SampleOrderingClassConstants.OrderingBy.Number,
                (Expression<Func<SampleOrderingClass, int>>)(x => x.Number)
            },
            {
                SampleOrderingClassConstants.OrderingBy.NumberNested,
                (Expression<Func<SampleOrderingClass, int>>)(x => x.Nested.Number)
            },
            {
                SampleOrderingClassConstants.OrderingBy.Pointed,
                (Expression<Func<SampleOrderingClass, double>>)(x => x.Pointed)
            },
            {
                SampleOrderingClassConstants.OrderingBy.PointedNested,
                (Expression<Func<SampleOrderingClass, double>>)(x => x.Nested.Pointed)
            },
            {
                SampleOrderingClassConstants.OrderingBy.Precised,
                (Expression<Func<SampleOrderingClass, decimal>>)(x => x.Precised)
            },
            {
                SampleOrderingClassConstants.OrderingBy.PrecisedNested,
                (Expression<Func<SampleOrderingClass, decimal>>)(x => x.Nested.Precised)
            }
        });

    public string Textual { get; set; }

    public int Number { get; set; }

    public double Pointed { get; set; }

    public decimal Precised { get; set; }

    public NestedClass Nested { get; set; }

    public class NestedClass
    {
        public string Textual { get; set; }

        public int Number { get; set; }

        public double Pointed { get; set; }

        public decimal Precised { get; set; }
    }
}
