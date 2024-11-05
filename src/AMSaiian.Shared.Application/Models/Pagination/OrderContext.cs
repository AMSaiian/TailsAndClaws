﻿namespace AMSaiian.Shared.Application.Models.Pagination;

public record OrderContext
{
    public required string PropertyName { get; init; } = "";
    public required bool IsDescending { get; init; }
}
