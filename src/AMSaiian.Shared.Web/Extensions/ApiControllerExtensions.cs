using AMSaiian.Shared.Application.Models.Pagination;
using AMSaiian.Shared.Web.Contract.Queries;
using AMSaiian.Shared.Web.Options;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AMSaiian.Shared.Web.Extensions;

public static class ApiControllerExtensions
{
    public static PageContext ProcessPageQuery(this ControllerBase controller,
                                               PageQuery query,
                                               IMapper mapper,
                                               RequestQueryOptions? requestOptions = null)
    {
        requestOptions ??= new RequestQueryOptions();

        if (query.PageNumber is null
         || query.PageSize is null)
        {
            query = new PageQuery
            {
                PageNumber = query.PageNumber ?? requestOptions.DefaultPageNumber,
                PageSize = query.PageSize ?? requestOptions.DefaultPageSize
            };
        }

        PageContext context = mapper.Map<PageContext>(query);

        return context;
    }

    public static OrderContext ProcessOrderQuery(this ControllerBase controller,
                                                 OrderQuery query,
                                                 string defaultPropertyName,
                                                 IMapper mapper,
                                                 RequestQueryOptions? requestOptions = null)
    {
        requestOptions ??= new RequestQueryOptions();

        if (query.IsDescending is null
         || query.PropertyName is null)
        {
            query = new OrderQuery
            {
                PropertyName = query.PropertyName ?? defaultPropertyName,
                IsDescending = query.IsDescending ?? requestOptions.IsDescendingDefault
            };
        }

        OrderContext? context = mapper.Map<OrderContext>(query);

        return context;
    }
}
