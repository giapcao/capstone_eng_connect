using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest
{
    public class ReviewTutorVerificationRequestCommandValidator
        : AbstractValidator<ReviewTutorVerificationRequestCommand>
    {
        public ReviewTutorVerificationRequestCommandValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage(CommonErrors.ValidationFailed("Dữ liệu không thể null.").Message);

            When(x => x.Request is not null, () =>
            {
                RuleFor(x => x.Request.RequestId)
                    .NotEmpty()
                    .WithMessage(TutorErrors.InvalidVerificationRequestId().Message);

                RuleFor(x => x.Request.AdminUserId)
                    .NotEmpty()
                    .WithMessage(CommonErrors.ValidationFailed("Dữ liệu không thể null.").Message);

                When(x => x.Request.Approved == false, () =>
                {
                    RuleFor(x => x.Request.RejectionReason)
                        .NotEmpty()
                        .WithMessage(TutorErrors.InvalidRejectionReason().Message);
                });
            });
        }
    }
}