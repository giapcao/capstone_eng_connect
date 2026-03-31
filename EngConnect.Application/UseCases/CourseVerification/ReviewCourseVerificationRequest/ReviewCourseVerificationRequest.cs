using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngConnect.Application.UseCases.CourseVerification.ReviewCourseVerificationRequest
{
    public record ReviewCourseVerificationRequest
    {
        [BindNever]
        public Guid RequestId { get; init; }
        [JsonIgnore]
        public Guid AdminUserId { get; init; }
        public bool Approved { get; init; }
        public string? RejectionReason { get; init; }
    }
}