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
            
        }
    }
}
