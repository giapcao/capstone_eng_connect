using EngConnect.Application.UseCases.TutorVerification.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest
{
    public record GetListTutorVerificationRequestQuery() : BaseQuery<PaginationResult<GetTutorVerificationRequestResponse>>
    {
        [BindNever]
        public Guid? TutorId { get; set; }
        public string? Status { get; set; }
        public Guid? ReviewedBy { get; set; }
    }
}
