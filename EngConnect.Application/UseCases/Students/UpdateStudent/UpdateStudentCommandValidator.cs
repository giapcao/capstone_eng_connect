using FluentValidation;

namespace EngConnect.Application.UseCases.Students.UpdateStudent;

public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator()
    {
        RuleForEach(x => new[] { x.Class, x.Grade, x.School })
            .MaximumLength(50)
            .WithMessage("Không được nhập quá 50 kí tự")
            .OverridePropertyName("Fields");
        RuleFor(x => x.Notes).MaximumLength(200).WithMessage("Không được nhập quá 200 kí tự");
    }
}