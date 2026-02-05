using EngConnect.Application.UseCases.Tutor.Common;
using EngConnect.BuildingBlock.Application.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.Tutor.GetTutorById
{
    public record GetTutorByIdQuery(Guid Id) : BaseQuery<GetTutorResponse>;
}
