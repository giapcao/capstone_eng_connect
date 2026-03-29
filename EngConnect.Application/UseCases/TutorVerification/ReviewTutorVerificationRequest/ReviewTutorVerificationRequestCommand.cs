using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest
{
    public record ReviewTutorVerificationRequestCommand(ReviewTutorVerificationRequest Request) : ICommand;
}
