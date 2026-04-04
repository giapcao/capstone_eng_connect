 using EngConnect.Application.UseCases.Tutors.Extensions;
using EngConnect.Domain.DomainErrors;
using FluentValidation;

namespace EngConnect.Application.UseCases.Tutors.CreateTutor
{
    public class CreateTutorCommandValidator : AbstractValidator<CreateTutorCommand>
    {
        public CreateTutorCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(TutorErrors.InvalidUserId().Message);

            RuleFor(x => x.Headline)
                .NotEmpty()
                .WithMessage(TutorErrors.InvalidHeadline().Message)
                .MaximumLength(255)
                .WithMessage(TutorErrors.InvalidHeadline().Message);

            RuleFor(x => x.Bio)
                .NotEmpty()
                .WithMessage(TutorErrors.InvalidBio().Message);
        }
    }
}
