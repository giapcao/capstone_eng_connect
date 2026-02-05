using EngConnect.BuildingBlock.Application.Validation.Validation;
using FluentValidation;

namespace EngConnect.Application.UseCases.Users.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        // FirstName validation
        When(x => !string.IsNullOrWhiteSpace(x.FirstName), () =>
        {
            RuleFor(x => x.FirstName)
                .MaximumLength(100).WithMessage("Tên không được vượt quá 100 ký tự");
        });

        // LastName validation
        When(x => !string.IsNullOrWhiteSpace(x.LastName), () =>
        {
            RuleFor(x => x.LastName)
                .MaximumLength(100).WithMessage("Họ không được vượt quá 100 ký tự");
        });

        // Phone validation - use Vietnamese phone validator
        When(x => !string.IsNullOrWhiteSpace(x.Phone), () =>
        {
            RuleFor(x => x.Phone!)
                .VietnamesePhoneNumber();
        });

        // AddressNum validation
        When(x => !string.IsNullOrWhiteSpace(x.AddressNum), () =>
        {
            RuleFor(x => x.AddressNum)
                .MaximumLength(255).WithMessage("Số nhà không được vượt quá 255 ký tự");
        });

        // ProvinceId validation
        When(x => !string.IsNullOrWhiteSpace(x.ProvinceId), () =>
        {
            RuleFor(x => x.ProvinceId)
                .MaximumLength(20).WithMessage("Mã tỉnh/thành không được vượt quá 20 ký tự");
        });

        // ProvinceName validation
        When(x => !string.IsNullOrWhiteSpace(x.ProvinceName), () =>
        {
            RuleFor(x => x.ProvinceName)
                .MaximumLength(100).WithMessage("Tên tỉnh/thành không được vượt quá 100 ký tự");
        });

        // WardId validation
        When(x => !string.IsNullOrWhiteSpace(x.WardId), () =>
        {
            RuleFor(x => x.WardId)
                .MaximumLength(20).WithMessage("Mã phường/xã không được vượt quá 20 ký tự");
        });

        // WardName validation
        When(x => !string.IsNullOrWhiteSpace(x.WardName), () =>
        {
            RuleFor(x => x.WardName)
                .MaximumLength(100).WithMessage("Tên phường/xã không được vượt quá 100 ký tự");
        });

        // Cross-field validation: If ProvinceId is provided, ProvinceName should also be provided
        RuleFor(x => x.ProvinceName)
            .NotEmpty()
            .When(x => !string.IsNullOrWhiteSpace(x.ProvinceId))
            .WithMessage("Tên tỉnh/thành phải được cung cấp khi có mã tỉnh/thành");

        RuleFor(x => x.ProvinceId)
            .NotEmpty()
            .When(x => !string.IsNullOrWhiteSpace(x.ProvinceName))
            .WithMessage("Mã tỉnh/thành phải được cung cấp khi có tên tỉnh/thành");

        // Cross-field validation: If WardId is provided, WardName should also be provided
        RuleFor(x => x.WardName)
            .NotEmpty()
            .When(x => !string.IsNullOrWhiteSpace(x.WardId))
            .WithMessage("Tên phường/xã phải được cung cấp khi có mã phường/xã");

        RuleFor(x => x.WardId)
            .NotEmpty()
            .When(x => !string.IsNullOrWhiteSpace(x.WardName))
            .WithMessage("Mã phường/xã phải được cung cấp khi có tên phường/xã");
    }
}





