using EngConnect.Application.UseCases.TutorVerification.Extensions;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.TutorVerification.UpdateTutorVerificationRequest
{
    public class UpdateTutorVerificationRequestCommandValidator
    : AbstractValidator<UpdateTutorVerificationRequestCommand>
    {
        public UpdateTutorVerificationRequestCommandValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage(CommonErrors.ValidationFailed("Dữ liệu không thể null.").Message);

            When(x => x.Request is not null, () =>
            {
                RuleFor(x => x.Request.RequestId)
                    .NotEmpty()
                    .WithMessage(TutorErrors.InvalidVerificationRequestId().Message);

                RuleFor(x => x.Request.TutorId)
                    .NotEmpty()
                    .WithMessage(TutorErrors.InvalidUserId().Message);

                RuleFor(x => x.Request.Status)
                    .NotEmpty()
                    .WithMessage(CommonErrors.ValidationFailed("Dữ liệu không thể null.").Message)
                    .Must(TutorVerificationStatusExtensions.IsValidTutorVerificationRequestStatus)
                    .WithMessage(CommonErrors.ValidationFailed("Trạng thái không hợp lệ.").Message);

                When(x => string.Equals(x.Request.Status, "Rejected", StringComparison.OrdinalIgnoreCase), () =>
                {
                    RuleFor(x => x.Request.RejectionReason)
                        .NotEmpty()
                        .WithMessage(TutorErrors.InvalidRejectionReason().Message);
                });
            });
        }
    }
}