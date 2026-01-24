using EngConnect.Application.UseCases.Tutor.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.Tutor.GetListTutor
{
    public record GetListTutorQuery(string? Status, string? VerifiedStatus) : BaseQuery<PaginationResult<GetTutorResponse>>;

}
