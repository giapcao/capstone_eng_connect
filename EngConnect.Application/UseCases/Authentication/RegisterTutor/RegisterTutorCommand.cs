using System.Text.Json.Serialization;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Authentication.RegisterTutor
{
    public class RegisterTutorCommand : ICommand
    {
        [JsonIgnore]
        public Guid UserId { get; set; }

        public string? Headline { get; set; }

        public string? Bio { get; set; }
        
        public int? YearsExperience { get; set; }
    }
}
