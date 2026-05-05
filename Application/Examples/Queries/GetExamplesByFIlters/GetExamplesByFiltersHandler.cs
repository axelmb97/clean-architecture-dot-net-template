

using Application.Common.Dtos.Filters;
using Application.Examples.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Examples.Queries.GetExamplesByFIlters
{
    public class GetExamplesByFiltersHandler : IRequestHandler<GetExamplesByFiltersQuery, List<ExampleDto>>
    {
        private readonly IExamplesRepository _examplesRepository;
        private readonly IMapper _mapper;

        public GetExamplesByFiltersHandler(IExamplesRepository examplesRepository, IMapper mapper)
        {
            _examplesRepository = examplesRepository;
            _mapper = mapper;
        }

        public async Task<List<ExampleDto>> Handle(GetExamplesByFiltersQuery request, CancellationToken cancellationToken)
        {
            var examples = await _examplesRepository.GetAll(cancellationToken);
            return _mapper.Map<List<ExampleDto>>(examples);
        }
    }
}
