using System.Text.Json.Serialization;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.Files;

namespace EngConnect.Application.UseCases.Authentication.RegisterTutor
{
    public class RegisterTutorCommand : ICommand
    {
        [JsonIgnore]
        public Guid UserId { get; set; }

        public string? Headline { get; set; }

        public string? Bio { get; set; }
        
        public int? MonthExperience { get; set; }
        
        public FileUpload? IntroVideoFile { get; set; }
        
        public FileUpload? CvFile { get; set; }
    }
}
