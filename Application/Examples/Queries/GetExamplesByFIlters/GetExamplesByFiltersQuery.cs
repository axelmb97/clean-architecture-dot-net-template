using Application.Common.Dtos.Filters;
using Application.Examples.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Examples.Queries.GetExamplesByFIlters
{
    public class GetExamplesByFiltersQuery : IRequest<List<ExampleDto>>
    {
    }
}
