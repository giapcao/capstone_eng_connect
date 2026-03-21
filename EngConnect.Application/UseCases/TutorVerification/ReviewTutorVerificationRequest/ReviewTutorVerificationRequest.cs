using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest
{
    public record ReviewTutorVerificationRequest
    {
        public Guid RequestId { get; init; }
        [BindNever]
        public Guid? AdminUserId { get; init; }
        public bool Approved { get; init; }
        public string? RejectionReason { get; init; }
    }
}