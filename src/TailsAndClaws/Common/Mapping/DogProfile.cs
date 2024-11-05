using AutoMapper;
using TailsAndClaws.Application.Dogs.Commands.CreateDog;
using TailsAndClaws.Common.Contract.Requests.Dogs;

namespace TailsAndClaws.Common.Mapping;

public sealed class DogProfile : Profile
{
    public DogProfile()
    {
        CreateMap<CreateDogRequest, CreateDogCommand>();
    }
}
