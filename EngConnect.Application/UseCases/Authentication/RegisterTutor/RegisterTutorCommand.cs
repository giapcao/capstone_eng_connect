using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Authentication.RegisterTutor
{
    public class RegisterTutorCommand : ICommand
    {
        public Guid UserId { get; set; }

        public string? Headline { get; set; }

        public string? Bio { get; set; }

        public string? IntroVideoUrl { get; set; }

        public int? YearsExperience { get; set; }

        public string? CvUrl { get; set; }

        public int? SlotsCount { get; set; }
    }
}
