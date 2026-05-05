using Application.Common.Dtos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Examples.Dtos
{
    public class ExampleDto : BaseDto
    {
        public string Name { get; set; } = null!;
    }
}
