namespace AMSaiian.Shared.Infrastructure.Constraints;

public record CheckConstraint(string Name, string SqlCode);

public static class CheckConstraints
{
    public static readonly CheckConstraint GreaterThenZeroFormat =
        new("CHK_{0}_must_be_greater_than_zero",
            "{0} > 0");
}
