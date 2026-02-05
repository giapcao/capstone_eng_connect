using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest
{
    public record CreateTutorVerificationRequest
    {
        public Guid TutorId { get; init; }
        public string? Note { get; init; }
    }
}
