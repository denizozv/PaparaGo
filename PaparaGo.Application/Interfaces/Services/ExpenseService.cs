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

    // Post Expense
    public async Task CreateAsync(CreateExpenseRequestDto dto, Guid userId)
    {
        var category = await _context.Categories.FindAsync(dto.CategoryId);
        if (category is null || category.DeletedAt != null)
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

    // users own expens
    public async Task<IEnumerable<ExpenseRequestResponseDto>> GetMyExpensesAsync(Guid userId)
    {
        return await _context.ExpenseRequests
            .Include(e => e.Category)
            .Where(e => e.UserId == userId && e.Category.DeletedAt == null)
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

    // Admin: pending
    public async Task<IEnumerable<ExpenseRequestResponseDto>> GetPendingRequestsAsync()
    {
        return await _context.ExpenseRequests
            .Include(e => e.User)
            .Include(e => e.Category)
            .Where(e => e.Status == ExpenseStatus.Pending && e.Category.DeletedAt == null)
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

    // Admin: expense accept
    public async Task ApproveAsync(Guid expenseRequestId)
{
    using var transaction = await _context.Database.BeginTransactionAsync(); //negin transaction

    try
    {
        var expense = await _context.ExpenseRequests.FindAsync(expenseRequestId);
        if (expense is null || expense.Status != ExpenseStatus.Pending)
            throw new Exception("Talep bulunamadı veya zaten işlem görmüş.");

        var user = await _context.Users.FindAsync(expense.UserId);
        if (user is null || !user.IsActive)
            throw new Exception("Kullanıcı bulunamadı.");

        expense.Status = ExpenseStatus.Approved;

        
        user.Balance += expense.Amount;

         
        var paymentLog = new PaymentLog
        {
            Id = Guid.NewGuid(),
            ExpenseRequestId = expense.Id,
            PaymentDate = DateTime.UtcNow,
            Amount = expense.Amount
        };

        _context.PaymentLogs.Add(paymentLog);

        await _context.SaveChangesAsync(); 

        await transaction.CommitAsync();   
    }
    catch
    {
        await transaction.RollbackAsync(); // rollback
        throw; 
    }
}



    // Admin: rejection
    public async Task RejectAsync(Guid expenseRequestId, string reason)
    {
        var expense = await _context.ExpenseRequests.FindAsync(expenseRequestId);
        if (expense is null || expense.Status != ExpenseStatus.Pending)
            throw new Exception("Talep bulunamadı veya zaten işlem görmüş.");

        expense.Status = ExpenseStatus.Rejected;
        expense.RejectionReason = reason;

        await _context.SaveChangesAsync();
    }

    // Admin: get accepted
    public async Task<IEnumerable<ExpenseRequestResponseDto>> GetApprovedRequestsAsync()
    {
        return await _context.ExpenseRequests
            .Include(e => e.Category)
            .Include(e => e.User)
            .Where(e => e.Status == ExpenseStatus.Approved && e.Category.DeletedAt == null)
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

    // Admin: get rejection
    public async Task<IEnumerable<ExpenseRequestResponseDto>> GetRejectedRequestsAsync()
    {
        return await _context.ExpenseRequests
            .Include(e => e.Category)
            .Include(e => e.User)
            .Where(e => e.Status == ExpenseStatus.Rejected && e.Category.DeletedAt == null)
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
}
