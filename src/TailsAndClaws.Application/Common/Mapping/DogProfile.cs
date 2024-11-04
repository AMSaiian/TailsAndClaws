using AutoMapper;
using TailsAndClaws.Application.Dogs.Commands.CreateDog;
using TailsAndClaws.Application.Dogs.ViewModels;
using TailsAndClaws.Domain.Entities;

namespace TailsAndClaws.Application.Common.Mapping;

public sealed class DogProfile : Profile
{
    public DogProfile()
    {
        CreateMap<Dog, DogViewModel>();
        CreateMap<CreateDogCommand, Dog>();
    }
}
