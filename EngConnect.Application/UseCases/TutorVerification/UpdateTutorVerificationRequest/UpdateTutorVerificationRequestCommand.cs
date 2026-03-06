using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.TutorVerification.UpdateTutorVerificationRequest
{
    public record UpdateTutorVerificationRequestCommand(UpdateTutorVerificationRequest Request) : ICommand;
}
