using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.Tutor.UpdateTutor
{
    public class UpdateTutorRequest
    {
        public string? Headline { get; set; }

        public string? Bio { get; set; }

        public string? IntroVideoUrl { get; set; }

        public int? YearsExperience { get; set; }

        public string? CvUrl { get; set; }

        public List<string>? Tags { get; set; }

        public int? SlotsCount { get; set; }

        public string? Status { get; set; }

        public string? VerifiedStatus { get; set; }
    }
}
