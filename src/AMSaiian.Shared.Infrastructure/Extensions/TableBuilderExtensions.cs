using AMSaiian.Shared.Infrastructure.Constraints;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMSaiian.Shared.Infrastructure.Extensions;

public static class TableBuilderExtensions
{
    public static TableBuilder<TEntity> AddGreaterThanZeroCheckConstraint<TEntity>(
        this TableBuilder<TEntity> tableBuilder,
        string constraintNamePrefix,
        string sqlColumnName)
        where TEntity : class
    {
        tableBuilder.HasCheckConstraint(
            string.Format(CheckConstraints.GreaterThenZeroFormat.Name, constraintNamePrefix),
            string.Format(CheckConstraints.GreaterThenZeroFormat.SqlCode, sqlColumnName));

        return tableBuilder;
    }
}
