using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.CourseVerification.ReviewCourseVerificationRequest
{
    public class ReviewCourseVerificationRequestCommandValidator
        : AbstractValidator<ReviewCourseVerificationRequestCommand>
    {
        public ReviewCourseVerificationRequestCommandValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage(CommonErrors.ValidationFailed("Dữ liệu không thể null.").Message);
            RuleFor(x => x.Request.RequestId)
                .NotEmpty()
                .WithMessage(CourseErrors.InvalidVerificationRequestId().Message);

            RuleFor(x => x.Request.AdminUserId)
                .NotEmpty()
                .WithMessage(CommonErrors.ValidationFailed("AdminUserId không thể null.").Message);

            When(x => !x.Request.Approved, () =>
            {
                RuleFor(x => x.Request.RejectionReason)
                    .NotEmpty()
                    .WithMessage(CourseErrors.InvalidRejectionReason().Message);
            });
        }
    }
}