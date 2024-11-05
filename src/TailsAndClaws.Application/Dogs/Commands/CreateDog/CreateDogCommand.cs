using AMSaiian.Shared.Application.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TailsAndClaws.Application.Common.Constants;
using TailsAndClaws.Application.Common.Interfaces;
using TailsAndClaws.Domain.Entities;

namespace TailsAndClaws.Application.Dogs.Commands.CreateDog;

public record CreateDogCommand : IRequest<Guid>
{
    public required string Name { get; init; } = default!;

    public required decimal TailLengthInMeters { get; init; }

    public required decimal WeightInKg { get; init; }

    public required string Color { get; init; } = default!;
}

public class CreateDogHandler(
    IAppDbContext dbContext,
    IMapper mapper,
    ILogger<CreateDogHandler> logger)
    : IRequestHandler<CreateDogCommand, Guid>
{
    private readonly IAppDbContext _dbContext = dbContext;
    private readonly ILogger<CreateDogHandler> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<Guid> Handle(CreateDogCommand request, CancellationToken cancellationToken)
    {
        cancellationToken = CancellationToken.None;

        bool dogWithSameNameAlreadyExists = await _dbContext.Dogs
            .AsNoTracking()
            .AnyAsync(dog => dog.NormalizedName == request.Name.ToUpper(),
                      cancellationToken);

        if (dogWithSameNameAlreadyExists)
        {
            throw new ConflictException(string.Format(
                                            ErrorMessagesConstants.DogWithSameNameAlreadyExistsFormat,
                                            request.Name));
        }

        Dog newDog = _mapper.Map<Dog>(request);
        await _dbContext.Dogs.AddAsync(newDog, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            LoggingTemplates.DogCreatedFormat,
            newDog.Id);

        return newDog.Id;
    }
}
