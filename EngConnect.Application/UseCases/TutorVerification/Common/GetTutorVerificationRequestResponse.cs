
namespace EngConnect.Application.UseCases.TutorVerification.Common
{
    public class GetTutorVerificationRequestResponse
    {
        public Guid Id { get; init; }
        public Guid TutorId { get; init; }
        public string? Status { get; init; }
        public DateTime? SubmittedAt { get; init; }
        public DateTime? ReviewedAt { get; init; }
        public Guid? ReviewedBy { get; init; }
        public string? RejectionReason { get; init; }
    }
}
