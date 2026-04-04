using FluentValidation;

namespace EngConnect.Application.UseCases.Courses.UpdateThumbnailCourse;

public class UpdateThumbnailCourseValidator : AbstractValidator<UpdateThumbnailCourseCommand>
{
    public UpdateThumbnailCourseValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống");

        RuleFor(x => x.File)
            .NotNull().WithMessage("File không được null");

        RuleFor(x => x.File.FileName)
            .NotEmpty().WithMessage("Tên file không được để trống")
            .Must(filename => filename.EndsWith(".jpg") || filename.EndsWith(".jpeg") ||
                             filename.EndsWith(".png") || filename.EndsWith(".webp"))
            .WithMessage("File thumbnail phải có định dạng .jpg, .jpeg, .png hoặc .webp");

        RuleFor(x => x.File.Content)
            .NotNull().WithMessage("Nội dung file không được null");

        RuleFor(x => x.File.Length)
            .GreaterThan(0).WithMessage("Kích thước file phải lớn hơn 0")
            .LessThanOrEqualTo(5 * 1024 * 1024).WithMessage("Kích thước file không được vượt quá 5MB");
    }
}
