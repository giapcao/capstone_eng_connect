using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.CourseVerification.CreateCourseVerificationRequest
{
    public class CreateCourseVerificationRequestCommandValidator
        : AbstractValidator<CreateCourseVerificationRequestCommand>
    {
        public CreateCourseVerificationRequestCommandValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage(CommonErrors.ValidationFailed("Dữ liệu không thể null.").Message);

            When(x => x.Request is not null, () =>
            {
                RuleFor(x => x.Request.CourseId)
                    .NotEmpty()
                    .WithMessage(CourseErrors.InvalidCourseId().Message);
            });
        }
    }
}