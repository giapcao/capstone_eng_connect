using FluentValidation;

namespace EngConnect.Application.UseCases.Students.UpdateAvatarStudent;

public class UpdateAvatarValidator : AbstractValidator<UpdateAvatarStudentCommand>
{
    public UpdateAvatarValidator()
    {
        RuleFor(x => x.File).NotNull();
        RuleFor(x => x.File.FileName)
            .NotEmpty()
            .Must(Path.HasExtension)
            .WithMessage("Tên file bắt buộc phải có phần mở rộng.");        
        RuleFor(x => x.File.Content).NotNull();
        RuleFor(x => x.File.Length).GreaterThan(0);
    }
}