using EngConnect.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.TutorSchedules.Extensions
{
    public static class ScheduleStatusExtensions
    {
        public static bool IsValidLessonRescheduleRequestStatus(string? status)
        {
            return status is nameof(LessonRescheduleRequestStatus.Pending)
                or nameof(LessonRescheduleRequestStatus.Approved)
                or nameof(LessonRescheduleRequestStatus.Rejected);
        }

        public static bool IsPendingLessonRescheduleRequestStatus(string? status)
        {
            return status == nameof(LessonRescheduleRequestStatus.Pending);
        }
    }
}