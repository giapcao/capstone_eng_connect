using FluentValidation;

namespace EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor;

public class UpdateAvatarTutorValidator : AbstractValidator<UpdateAvatarTutorCommand>
{
    public UpdateAvatarTutorValidator()
    {
        RuleFor(x => x.File).NotNull();
        RuleFor(x => x.File.FileName).NotEmpty();
        RuleFor(x => x.File.Content).NotNull();
        RuleFor(x => x.File.Length).GreaterThan(0);
    }
}
