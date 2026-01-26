using EngConnect.Application.UseCases.Tutor.Extensions;
using EngConnect.Domain.DomainErrors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.Tutor.CreateTutor
{
    public class CreateTutorCommandValidator : AbstractValidator<CreateTutorCommand>
    {
        public CreateTutorCommandValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage(TutorErrors.InvalidUserId().Message);

            When(x => x.Request is not null, () =>
            {
                RuleFor(x => x.Request.UserId)
                    .NotEmpty()
                    .WithMessage(TutorErrors.InvalidUserId().Message);

                RuleFor(x => x.Request.Headline)
                    .NotEmpty()
                    .WithMessage(TutorErrors.InvalidHeadline().Message)
                    .MaximumLength(255)
                    .WithMessage(TutorErrors.InvalidHeadline().Message);

                RuleFor(x => x.Request.Bio)
                    .NotEmpty()
                    .WithMessage(TutorErrors.InvalidBio().Message);

                RuleFor(x => x.Request.YearsExperience)
                    .GreaterThanOrEqualTo(0)
                    .When(x => x.Request.YearsExperience.HasValue)
                    .WithMessage(TutorErrors.InvalidYearsExperience().Message);

                RuleFor(x => x.Request.SlotsCount)
                    .GreaterThanOrEqualTo(0)
                    .When(x => x.Request.SlotsCount.HasValue)
                    .WithMessage(TutorErrors.InvalidSlotsCount().Message);

                RuleFor(x => x.Request.Status)
                    .Must(status =>
                        string.IsNullOrWhiteSpace(status) ||
                        TutorStatusExtensions.IsValidTutorStatus(status))
                    .WithMessage(x => TutorErrors.InvalidStatus(x.Request.Status ?? string.Empty).Message);

                RuleFor(x => x.Request.VerifiedStatus)
                    .Must(status =>
                        string.IsNullOrWhiteSpace(status) ||
                        TutorStatusExtensions.IsValidTutorVerifiedStatus(status))
                    .WithMessage(x => TutorErrors.InvalidVerifiedStatus(x.Request.VerifiedStatus ?? string.Empty).Message);
            });
        }
    }
}