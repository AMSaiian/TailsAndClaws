using AMSaiian.Shared.Application.Interfaces;
using AMSaiian.Shared.Application.Models.Pagination;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TailsAndClaws.Application.Common.Interfaces;
using TailsAndClaws.Application.Dogs.ViewModels;
using TailsAndClaws.Domain.Entities;

namespace TailsAndClaws.Application.Dogs.Queries.GetDogs;

public record GetDogsQuery : IRequest<Paginated<DogViewModel>>
{
    public required PaginationContext PaginationContext { get; init; }
}

public sealed class GetDogsHandler(
    IAppDbContext dbContext,
    IMapper mapper,
    IPaginationService paginationService)
    : IRequestHandler<GetDogsQuery, Paginated<DogViewModel>>
{
    private readonly IAppDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;
    private readonly IPaginationService _paginationService = paginationService;

    public async Task<Paginated<DogViewModel>> Handle(GetDogsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Dog> dogsQuery = _dbContext.Dogs
            .AsNoTracking();

        Paginated<Dog> paginatedEntities = await _paginationService
            .PaginateAsync(
                dogsQuery,
                request.PaginationContext,
                cancellationToken);

        Paginated<DogViewModel> paginatedViewModels = new()
        {
            PaginationInfo = paginatedEntities.PaginationInfo,
            Records = _mapper.Map<List<DogViewModel>>(paginatedEntities.Records)
        };

        return paginatedViewModels;
    }
}
