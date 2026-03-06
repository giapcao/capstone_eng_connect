namespace EngConnect.Application.UseCases.Tutors.UpdateTutor
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
