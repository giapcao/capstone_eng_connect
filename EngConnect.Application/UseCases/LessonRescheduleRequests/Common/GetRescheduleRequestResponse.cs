using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.LessonRescheduleRequests.Common
{
    public sealed class GetRescheduleRequestResponse
    {
        public Guid Id { get; set; }
        public Guid LessonId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime ProposedStartTime { get; set; }
        public DateTime ProposedEndTime { get; set; }
        public string? Status { get; set; }
        public string? TutorNote { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
