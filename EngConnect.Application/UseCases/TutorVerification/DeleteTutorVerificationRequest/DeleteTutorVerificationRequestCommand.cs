using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.TutorVerification.DeleteTutorVerificationRequest
{
    public record DeleteTutorVerificationRequestCommand(Guid RequestId) : ICommand;
}
