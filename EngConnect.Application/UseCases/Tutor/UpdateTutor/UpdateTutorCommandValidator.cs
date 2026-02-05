using EngConnect.Application.UseCases.Tutor.Extensions;
using EngConnect.Domain.DomainErrors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.Tutor.UpdateTutor
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
                    .When(x => x.Request.Status is not null)
                    .WithMessage(x => TutorErrors.InvalidStatus(x.Request.Status ?? string.Empty).Message);

                RuleFor(x => x.Request.VerifiedStatus)
                    .Must(status =>
                        string.IsNullOrWhiteSpace(status) ||
                        TutorStatusExtensions.IsValidTutorVerifiedStatus(status))
                    .When(x => x.Request.VerifiedStatus is not null)
                    .WithMessage(x => TutorErrors.InvalidVerifiedStatus(x.Request.VerifiedStatus ?? string.Empty).Message);
            });
        }
    }
}