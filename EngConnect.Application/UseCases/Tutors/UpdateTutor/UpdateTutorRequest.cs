namespace EngConnect.Application.UseCases.Tutors.UpdateTutor
{
    public class UpdateTutorRequest
    {
        public string? Headline { get; set; }

        public string? Bio { get; set; }
        
        public int? MonthExperience { get; set; }

        public string? Status { get; set; }
    }
}
