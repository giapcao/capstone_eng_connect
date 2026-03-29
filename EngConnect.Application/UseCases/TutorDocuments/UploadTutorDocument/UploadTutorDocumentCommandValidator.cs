using EngConnect.Domain.Constants;
using FluentValidation;

namespace EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument;

public class UploadTutorDocumentCommandValidator : AbstractValidator<UploadTutorDocumentCommand>
{
    public UploadTutorDocumentCommandValidator()
    {
        RuleFor(x => x.TutorId)
            .NotEmpty().WithMessage("TutorId không được để trống");

        RuleFor(x => x.File)
            .NotNull().WithMessage("File không được null");

        When(x => x.File is not null, () =>
        {
            RuleFor(x => x.File.FileName)
                .NotEmpty().WithMessage("Tên file không được để trống");

            RuleFor(x => x.File.Content)
                .NotNull().WithMessage("Nội dung file không được null");

            RuleFor(x => x.File.Length)
                .GreaterThan(0).WithMessage("Kích thước file phải lớn hơn 0")
                .LessThanOrEqualTo(20 * 1024 * 1024).WithMessage("Kích thước file không được vượt quá 20MB");
        });

        RuleFor(x => x.Name)
            .MaximumLength(255)
            .WithMessage("Tên tài liệu không được vượt quá 255 ký tự")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.DocType)
            .MaximumLength(100)
            .WithMessage("Loại tài liệu không được vượt quá 100 ký tự")
            .When(x => !string.IsNullOrWhiteSpace(x.DocType));

        RuleFor(x => x.IssuedBy)
            .MaximumLength(255)
            .WithMessage("Nơi cấp không được vượt quá 255 ký tự")
            .When(x => !string.IsNullOrWhiteSpace(x.IssuedBy));

        RuleFor(x => x.Status)
            .Must(status => string.IsNullOrWhiteSpace(status) || Enum.TryParse<CommonStatus>(status, true, out _))
            .WithMessage("Trạng thái tài liệu không hợp lệ");

        RuleFor(x => x)
            .Must(x => !x.IssuedAt.HasValue || !x.ExpiredAt.HasValue || x.ExpiredAt.Value >= x.IssuedAt.Value)
            .WithMessage("Ngày hết hạn phải lớn hơn hoặc bằng ngày cấp");
    }
}
