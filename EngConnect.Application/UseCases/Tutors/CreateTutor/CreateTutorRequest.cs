using Microsoft.AspNetCore.Http;

namespace EngConnect.Application.UseCases.Tutors.CreateTutor
{
    public class CreateTutorRequest
    {
        public Guid UserId { get; set; }

        public string? Headline { get; set; }

        public string? Bio { get; set; }

        public int? MonthExperience { get; set; }

        // Add file properties for intro video and CV
        public IFormFile? IntroVideoFile { get; set; }
        public string? IntroVideoFileName { get; set; }
        public IFormFile? CvFile { get; set; }
        public string? CvFileName { get; set; }
    }
}