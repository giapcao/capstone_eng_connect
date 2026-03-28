using Microsoft.AspNetCore.Http;

namespace EngConnect.Application.UseCases.Authentication.RegisterTutor;

public class RegisterTutorRequest
{
    public string? Headline { get; set; }
    public string? Bio { get; set; }
    public int? MonthExperience { get; set; }
    public IFormFile? IntroVideoFile { get; set; }
    public string? IntroVideoFileName { get; set; }
    public IFormFile? CvFile { get; set; }
    public string? CvFileName { get; set; }
}
