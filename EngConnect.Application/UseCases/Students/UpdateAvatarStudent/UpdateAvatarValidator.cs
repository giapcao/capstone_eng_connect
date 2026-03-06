using FluentValidation;

namespace EngConnect.Application.UseCases.Students.UpdateAvatarStudent;

public class UpdateAvatarValidator : AbstractValidator<UpdateAvatarStudentCommand>
{
    public UpdateAvatarValidator()
    {
        RuleFor(x => x.File).NotNull();
        RuleFor(x => x.File.FileName).NotEmpty();
        RuleFor(x => x.File.Content).NotNull();
        RuleFor(x => x.File.Length).GreaterThan(0);
    }
}