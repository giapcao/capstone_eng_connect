using FluentValidation;

namespace EngConnect.Application.UseCases.Authentication.RegisterTutor
{
    public class RegisterTutorCommandValidator : AbstractValidator<RegisterTutorCommand>
    {
        public RegisterTutorCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId không được để trống");

            When(x => x.YearsExperience.HasValue, () =>
            {
                RuleFor(x => x.YearsExperience)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Số năm kinh nghiệm phải lớn hơn hoặc bằng 0");
            });

            When(x => x.SlotsCount.HasValue, () =>
            {
                RuleFor(x => x.SlotsCount)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Số lượng slot phải lớn hơn hoặc bằng 0");
            });
        }
    }
}
