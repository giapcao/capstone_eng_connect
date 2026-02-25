using FluentValidation;

namespace EngConnect.Application.UseCases.AwsS3Storage.CheckFileExists;

public class CheckFileExistsQueryValidator : AbstractValidator<CheckFileExistsQuery>
{
    public CheckFileExistsQueryValidator()
    {
        RuleFor(x => x.FileName).NotEmpty();
    }
}