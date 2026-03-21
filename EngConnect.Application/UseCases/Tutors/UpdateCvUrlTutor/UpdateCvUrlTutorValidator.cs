using FluentValidation;

namespace EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor;

public class UpdateCvUrlTutorValidator : AbstractValidator<UpdateCvUrlTutorCommand>
{
    public UpdateCvUrlTutorValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống");

        RuleFor(x => x.File)
            .NotNull().WithMessage("File không được null");

        RuleFor(x => x.File.FileName)
            .NotEmpty().WithMessage("Tên file không được để trống")
            .Must(filename => filename.EndsWith(".pdf") || filename.EndsWith(".doc") || filename.EndsWith(".docx"))
            .WithMessage("File CV phải có định dạng .pdf, .doc hoặc .docx");

        RuleFor(x => x.File.Content)
            .NotNull().WithMessage("Nội dung file không được null");

        RuleFor(x => x.File.Length)
            .GreaterThan(0).WithMessage("Kích thước file phải lớn hơn 0")
            .LessThanOrEqualTo(10 * 1024 * 1024).WithMessage("Kích thước file không được vượt quá 10MB");
    }
}
