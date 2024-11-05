using AMSaiian.Shared.Application.Models.Pagination;
using FluentValidation;

namespace TailsAndClaws.Application.Dogs.Queries.GetDogs;

public sealed class GetDogsQueryValidator : AbstractValidator<GetDogsQuery>
{
    public GetDogsQueryValidator(IValidator<PaginationContext> validator)
    {
        RuleFor(query => query.PaginationContext)
            .NotNull()
            .SetValidator(validator);
    }
}
