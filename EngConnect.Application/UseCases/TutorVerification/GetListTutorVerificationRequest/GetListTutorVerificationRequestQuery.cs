using EngConnect.Application.UseCases.TutorVerification.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest
{
    public record GetListTutorVerificationRequestQuery() : BaseQuery<PaginationResult<GetTutorVerificationRequestResponse>>
    {
        public Guid? TutorId { get; set; }
        public string? Status { get; set; }
        public Guid? ReviewedBy { get; set; }
    }
}
