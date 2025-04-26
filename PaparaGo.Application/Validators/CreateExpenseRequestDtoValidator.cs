using FluentValidation;
using PaparaGo.DTO;

namespace PaparaGo.Application.Validators;

public class CreateExpenseRequestDtoValidator : AbstractValidator<CreateExpenseRequestDto>
{
    public CreateExpenseRequestDtoValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Kategori seçimi zorunludur.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Tutar 0'dan büyük olmalıdır.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Açıklama alanı zorunludur.")
            .MaximumLength(250).WithMessage("Açıklama en fazla 250 karakter olabilir.");
    }
}
