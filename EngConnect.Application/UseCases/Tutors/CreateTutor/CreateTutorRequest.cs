namespace EngConnect.Application.UseCases.Tutors.CreateTutor
{
    public class CreateTutorRequest
    {
        public Guid UserId { get; set; }

        public string? Headline { get; set; }

        public string? Bio { get; set; }
        
        public int? YearsExperience { get; set; }
        
        public List<string>? Tags { get; set; }

        public int? SlotsCount { get; set; }

        public string? Status { get; set; }

        public string? VerifiedStatus { get; set; }
    }
}
