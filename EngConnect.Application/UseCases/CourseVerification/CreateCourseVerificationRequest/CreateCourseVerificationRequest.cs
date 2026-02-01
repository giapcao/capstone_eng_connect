using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.CourseVerification.CreateCourseVerificationRequest
{
    public record CreateCourseVerificationRequest
    {
        public Guid CourseId { get; init; }
        public string? Note { get; init; }
    }
}
