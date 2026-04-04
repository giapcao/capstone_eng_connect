using EngConnect.Application.UseCases.Tutors.Extensions;
using EngConnect.Domain.DomainErrors;
using FluentValidation;

namespace EngConnect.Application.UseCases.Tutors.UpdateTutor
{
    public class UpdateTutorCommandValidator : AbstractValidator<UpdateTutorCommand>
    {
        public UpdateTutorCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(TutorErrors.TutorNotFound().Message);

            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage(TutorErrors.InvalidUserId().Message);

            When(x => x.Request is not null, () =>
            {
                RuleFor(x => x.Request.Headline)
                    .MaximumLength(255)
                    .When(x => x.Request.Headline is not null)
                    .WithMessage(TutorErrors.InvalidHeadline().Message);

                RuleFor(x => x.Request.Bio)
                    .NotEmpty()
                    .When(x => x.Request.Bio is not null)
                    .WithMessage(TutorErrors.InvalidBio().Message);

                RuleFor(x => x.Request.Status)
                    .Must(status =>
                        string.IsNullOrWhiteSpace(status) ||
                        TutorStatusExtensions.IsValidTutorStatus(status))
                    .When(x => x.Request.Status is not null)
                    .WithMessage(x => TutorErrors.InvalidStatus(x.Request.Status ?? string.Empty).Message);
                
            });
        }
    }
}