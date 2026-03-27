using FluentValidation;

namespace EngConnect.Application.UseCases.Courses.CreateCourse;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.TutorId)
            .NotEmpty().WithMessage("TutorId không được để trống");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Tiêu đề khóa học không được để trống")
            .MaximumLength(500).WithMessage("Tiêu đề không được vượt quá 500 ký tự");
        
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Giá phải lớn hơn hoặc bằng 0");
        
        RuleFor(x => x.NumsSessionInWeek)
            .GreaterThan(0)
            .WithMessage("Số buổi học trong tuần phải lớn hơn 0");
        
        // Validate Thumbnail File if provided
        When(x => x.ThumbnailFile != null, () =>
        {
            RuleFor(x => x.ThumbnailFile!.FileName)
                .NotEmpty().WithMessage("Tên file thumbnail không được để trống")
                .Must(filename => filename.EndsWith(".jpg") || filename.EndsWith(".jpeg") || 
                                 filename.EndsWith(".png") || filename.EndsWith(".webp"))
                .WithMessage("File thumbnail phải có định dạng .jpg, .jpeg, .png hoặc .webp");

            RuleFor(x => x.ThumbnailFile!.Content)
                .NotNull().WithMessage("Nội dung file thumbnail không được null");

            RuleFor(x => x.ThumbnailFile!.Length)
                .GreaterThan(0).WithMessage("Kích thước file thumbnail phải lớn hơn 0")
                .LessThanOrEqualTo(5 * 1024 * 1024).WithMessage("Kích thước file thumbnail không được vượt quá 5MB");
        });
        
        // Validate Demo Video File if provided
        When(x => x.DemoVideoFile != null, () =>
        {
            RuleFor(x => x.DemoVideoFile!.FileName)
                .NotEmpty().WithMessage("Tên file video không được để trống")
                .Must(filename => filename.EndsWith(".mp4") || filename.EndsWith(".mov") || 
                                 filename.EndsWith(".avi") || filename.EndsWith(".webm"))
                .WithMessage("File video phải có định dạng .mp4, .mov, .avi hoặc .webm");

            RuleFor(x => x.DemoVideoFile!.Content)
                .NotNull().WithMessage("Nội dung file video không được null");

            RuleFor(x => x.DemoVideoFile!.Length)
                .GreaterThan(0).WithMessage("Kích thước file video phải lớn hơn 0")
                .LessThanOrEqualTo(50 * 1024 * 1024).WithMessage("Kích thước file video không được vượt quá 50MB");
        });
    }
}
