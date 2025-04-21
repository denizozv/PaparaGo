using PaparaGo.DTO;

namespace PaparaGo.Application.Interfaces.Services;

public interface IExpenseService
{
    Task CreateAsync(CreateExpenseRequestDto dto, Guid userId);
    Task<IEnumerable<ExpenseRequestResponseDto>> GetMyExpensesAsync(Guid userId);
    Task<IEnumerable<ExpenseRequestResponseDto>> GetPendingRequestsAsync();
}
