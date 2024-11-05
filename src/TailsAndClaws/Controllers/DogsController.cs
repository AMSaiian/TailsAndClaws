using AMSaiian.Shared.Application.Models.Pagination;
using AMSaiian.Shared.Web.Contract.Queries;
using AMSaiian.Shared.Web.Extensions;
using AMSaiian.Shared.Web.Options;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TailsAndClaws.Application.Dogs.Commands.CreateDog;
using TailsAndClaws.Application.Dogs.Queries.GetDogs;
using TailsAndClaws.Application.Dogs.ViewModels;
using TailsAndClaws.Common.Contract.Requests.Dogs;
using TailsAndClaws.Domain.Constants;

namespace TailsAndClaws.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status429TooManyRequests)]
public class DogsController(
    ISender mediator,
    IMapper mapper,
    IOptions<RequestQueryOptions> queryOptions)
    : ControllerBase
{
    private readonly IOptions<RequestQueryOptions> _queryOptions = queryOptions;
    private readonly ISender _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Paginated<DogViewModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDogs(
        [FromQuery] PageQuery page,
        [FromQuery] OrderQuery order,
        CancellationToken cancellationToken = default)
    {
        string defaultOrderPropertyName = DogConstants.OrderingBy.Name;

        PaginationContext paginationContext = new()
        {
            OrderContext = this.ProcessOrderQuery(
                order,
                defaultOrderPropertyName,
                _mapper,
                _queryOptions.Value),

            PageContext = this.ProcessPageQuery(
                page,
                _mapper,
                _queryOptions.Value)
        };

        var command = new GetDogsQuery
        {
            PaginationContext = paginationContext
        };

        Paginated<DogViewModel> result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateDog([FromBody] CreateDogRequest request,
                                               CancellationToken cancellationToken = default)
    {
        var command = _mapper.Map<CreateDogCommand>(request);

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}
