using EngConnect.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.TutorVerification.Extensions
{
    public static class TutorVerificationStatusExtensions
    {
        public static bool IsValidTutorVerificationRequestStatus(string? status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return false;
            }

            return Enum.GetNames<TutorVerificationRequestStatus>()
                .Any(n => string.Equals(n, status, StringComparison.OrdinalIgnoreCase));
        }
    }
}