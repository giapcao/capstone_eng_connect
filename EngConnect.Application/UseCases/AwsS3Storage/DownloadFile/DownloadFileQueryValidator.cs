using FluentValidation;

namespace EngConnect.Application.UseCases.AwsS3Storage.DownloadFile;

public class DownloadFileQueryValidator : AbstractValidator<DownloadFileQuery>
{
    public DownloadFileQueryValidator()
    {
        RuleFor(x => x.FileName).NotEmpty();
    }
}