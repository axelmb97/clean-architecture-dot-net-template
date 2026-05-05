

using Application.Examples.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Examples.Mappers
{
    public class ExampleDtoMapperConfiguration : Profile
    {
        public ExampleDtoMapperConfiguration()
        {
            CreateMap<Example, ExampleDto>().ReverseMap();
        }
    }
}
