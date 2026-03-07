using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.TutorSchedules.Common
{
    public sealed class GetTutorScheduleResponse
    {
        public Guid Id { get; set; }
        public Guid TutorId { get; set; }
        public string? Weekday { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
