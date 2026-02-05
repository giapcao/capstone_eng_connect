using EngConnect.Domain.Constants;
using FluentValidation;

namespace EngConnect.Application.UseCases.Students.GetListStudents;

public class GetListStudentQueryValidator : AbstractValidator<GetListStudentQuery>
{
    public GetListStudentQueryValidator()
    {
        RuleFor(x => x.Status)
            .Must(status => string.IsNullOrEmpty(status) || Enum.IsDefined(typeof(StudentStatus), status))
            .WithMessage("Trạng thái phải là Active hoặc Inactive");
    }
}