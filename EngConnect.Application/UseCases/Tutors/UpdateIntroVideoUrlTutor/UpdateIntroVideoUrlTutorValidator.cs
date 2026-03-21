using FluentValidation;

namespace EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor;

public class UpdateIntroVideoUrlTutorValidator : AbstractValidator<UpdateIntroVideoUrlTutorCommand>
{
    public UpdateIntroVideoUrlTutorValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống");

        RuleFor(x => x.File)
            .NotNull().WithMessage("File không được null");

        RuleFor(x => x.File.FileName)
            .NotEmpty().WithMessage("Tên file không được để trống")
            .Must(filename => filename.EndsWith(".mp4") || filename.EndsWith(".mov") || filename.EndsWith(".avi"))
            .WithMessage("File video phải có định dạng .mp4, .mov hoặc .avi");

        RuleFor(x => x.File.Content)
            .NotNull().WithMessage("Nội dung file không được null");

        RuleFor(x => x.File.Length)
            .GreaterThan(0).WithMessage("Kích thước file phải lớn hơn 0")
            .LessThanOrEqualTo(100 * 1024 * 1024).WithMessage("Kích thước file không được vượt quá 100MB");
    }
}
