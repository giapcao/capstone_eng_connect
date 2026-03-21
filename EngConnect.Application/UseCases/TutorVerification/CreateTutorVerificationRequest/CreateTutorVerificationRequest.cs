using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest
{
    public record CreateTutorVerificationRequest
    {
        [JsonIgnore]
        public Guid TutorId { get; init; }
    }
}
