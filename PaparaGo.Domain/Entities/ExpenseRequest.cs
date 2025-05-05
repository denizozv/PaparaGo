namespace PaparaGo.Domain.Entities;

public enum ExpenseStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2
}

public class ExpenseRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public Guid CategoryId { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; } = string.Empty;

    public string? DocumentPath { get; set; } // recipt

    public DateTime RequestDate { get; set; } = DateTime.UtcNow;

    public ExpenseStatus Status { get; set; } = ExpenseStatus.Pending;

    public string? RejectionReason { get; set; }

    public DateTime? PaymentDate { get; set; }

    // Navigation Properties
    public User? User { get; set; }

    public Category? Category { get; set; }
}
