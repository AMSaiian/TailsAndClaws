using AMSaiian.Shared.Application.Models.Pagination;
using AMSaiian.Shared.Web.Contract.Queries;
using AutoMapper;

namespace AMSaiian.Shared.Web.Mapping;

public sealed class QueryProfile : Profile
{
    public QueryProfile()
    {
        CreateMap<OrderQuery, OrderContext>();

        CreateMap<PageQuery, PageContext>();
    }
}
