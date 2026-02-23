using EngConnect.Application.UseCases.TutorVerification.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.TutorVerification.GetTutorVerificationRequestById
{
    public record GetTutorVerificationRequestByIdQuery(Guid RequestId) : IQuery<GetTutorVerificationRequestResponse>;
}
