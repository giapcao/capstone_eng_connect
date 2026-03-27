using FluentValidation;

namespace EngConnect.Application.UseCases.CourseResources.CreateCourseResource;

public class CreateCourseResourceCommandValidator : AbstractValidator<CreateCourseResourceCommand>
{
    public CreateCourseResourceCommandValidator()
    {
        RuleFor(x => x.CourseSessionId)
            .NotEmpty().WithMessage("CourseSessionId không được để trống");

        RuleFor(x => x.TutorId)
            .NotEmpty().WithMessage("TutorId không được để trống");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title không được để trống");
        RuleFor(x => x.ResourceType)
            .NotEmpty().WithMessage("ResourceType không được để trống");
        RuleFor(x => x.ResourceFile)
            .NotNull().WithMessage("ResourceFile không được để trống");
    }
}