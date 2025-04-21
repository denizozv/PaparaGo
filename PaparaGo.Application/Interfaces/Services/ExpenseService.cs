using PaparaGo.Application.Interfaces.Services;
using PaparaGo.DTO;
using PaparaGo.Domain.Entities;
using PaparaGo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PaparaGo.Application.Services;

public class ExpenseService : IExpenseService
{
    private readonly PaparaGoDbContext _context;

    public ExpenseService(PaparaGoDbContext context)
    {
        _context = context;
    }


    // post expense
    public async Task CreateAsync(CreateExpenseRequestDto dto, Guid userId)
    {
        var category = await _context.Categories.FindAsync(dto.CategoryId);
        if (category is null || !category.IsActive)
            throw new Exception("Geçersiz kategori.");

        var expense = new ExpenseRequest
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CategoryId = dto.CategoryId,
            Amount = dto.Amount,
            Description = dto.Description,
            DocumentPath = dto.DocumentPath,
            RequestDate = DateTime.UtcNow,
            Status = ExpenseStatus.Pending
        };

        _context.ExpenseRequests.Add(expense);
        await _context.SaveChangesAsync();
    }


    public async Task<IEnumerable<ExpenseRequestResponseDto>> GetMyExpensesAsync(Guid userId)
    {
        return await _context.ExpenseRequests
            .Include(e => e.Category)
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.RequestDate)
            .Select(e => new ExpenseRequestResponseDto
            {
                Id = e.Id,
                CategoryName = e.Category.Name,
                Amount = e.Amount,
                Description = e.Description,
                DocumentPath = e.DocumentPath,
                RequestDate = e.RequestDate,
                Status = e.Status.ToString(),
                RejectionReason = e.RejectionReason
            })
            .ToListAsync();
    }


    public async Task<IEnumerable<ExpenseRequestResponseDto>> GetPendingRequestsAsync()
    {
        return await _context.ExpenseRequests
            .Include(e => e.User)
            .Include(e => e.Category)
            .Where(e => e.Status == ExpenseStatus.Pending)
            .OrderBy(e => e.RequestDate)
            .Select(e => new ExpenseRequestResponseDto
            {
                Id = e.Id,
                CategoryName = e.Category.Name,
                Amount = e.Amount,
                Description = e.Description,
                DocumentPath = e.DocumentPath,
                RequestDate = e.RequestDate,
                Status = e.Status.ToString()
            })
            .ToListAsync();
    }


    public async Task ApproveAsync(Guid expenseRequestId)
    {
        var expense = await _context.ExpenseRequests.FindAsync(expenseRequestId);
        if (expense is null || expense.Status != ExpenseStatus.Pending)
            throw new Exception("Talep bulunamadı veya zaten işlem görmüş.");

        expense.Status = ExpenseStatus.Approved;

        var paymentLog = new PaymentLog
        {
            Id = Guid.NewGuid(),
            ExpenseRequestId = expense.Id,
            PaymentDate = DateTime.UtcNow,
            Amount = expense.Amount
        };

        _context.PaymentLogs.Add(paymentLog);
        await _context.SaveChangesAsync();
    }

    public async Task RejectAsync(Guid expenseRequestId, string reason)
    {
        var expense = await _context.ExpenseRequests.FindAsync(expenseRequestId);
        if (expense is null || expense.Status != ExpenseStatus.Pending)
            throw new Exception("Talep bulunamadı veya zaten işlem görmüş.");

        expense.Status = ExpenseStatus.Rejected;
        expense.RejectionReason = reason;

        await _context.SaveChangesAsync();
    }
}