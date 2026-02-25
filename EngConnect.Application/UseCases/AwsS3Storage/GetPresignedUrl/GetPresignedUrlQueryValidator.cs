using FluentValidation;

namespace EngConnect.Application.UseCases.AwsS3Storage.GetPresignedUrl;

public class GetPresignedUrlQueryValidator : AbstractValidator<GetPresignedUrlQuery>
{
    public GetPresignedUrlQueryValidator()
    {
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.DurationMinutes).GreaterThan(0);
    }
}