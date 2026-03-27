using FluentValidation;

namespace EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument;

public class RemoveTutorDocumentCommandValidator : AbstractValidator<RemoveTutorDocumentCommand>
{
    public RemoveTutorDocumentCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty().WithMessage("DocumentId không được để trống");

        RuleFor(x => x.TutorId)
            .NotEmpty().WithMessage("TutorId không được để trống");
    }
}
