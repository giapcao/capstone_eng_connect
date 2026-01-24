using EngConnect.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.Tutor.Extensions
{
    public static class TutorStatusExtensions
    {
        public static bool IsValidTutorStatus(string? status)
        {
            if (string.IsNullOrWhiteSpace(status)) { return false; }
            return Enum.TryParse<TutorStatus>(status, ignoreCase: true, out _);
        }

        public static bool IsValidTutorVerifiedStatus(string? status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return false;
            }

            return Enum.TryParse<StudentStatus>(status, ignoreCase: true, out _);
        }
    }
}