using FluentValidation;
using PaparaGo.DTO;

namespace PaparaGo.Application.Validators;

public class ApproveExpenseRequestDtoValidator : AbstractValidator<ApproveExpenseRequestDto>
{
    public ApproveExpenseRequestDtoValidator()
    {
        RuleFor(x => x.ExpenseRequestId)
            .NotEmpty().WithMessage("Masraf talep ID'si boş olamaz.");
    }
}
