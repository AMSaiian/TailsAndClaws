namespace AMSaiian.Shared.Infrastructure.Constraints;

public record CheckConstraint(string Name, string SqlCode);

public static class CheckConstraints
{
    public static readonly CheckConstraint GreaterThenZeroFormat =
        new("CHK_{ConstraintPrefix}_must_be_greater_than_zero",
            "{SqlColumnName} > 0");
}
