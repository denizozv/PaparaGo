using FluentValidation;
using PaparaGo.DTO;

namespace PaparaGo.Application.Validators;

public class RejectExpenseRequestDtoValidator : AbstractValidator<RejectExpenseRequestDto>
{
    public RejectExpenseRequestDtoValidator()
    {
        RuleFor(x => x.ExpenseRequestId)
            .NotEmpty().WithMessage("Masraf talep ID'si boş olamaz.");

        RuleFor(x => x.RejectionReason)
            .NotEmpty().WithMessage("Red açıklaması boş olamaz.")
            .MaximumLength(250).WithMessage("Red açıklaması en fazla 250 karakter olabilir.");
    }
}
