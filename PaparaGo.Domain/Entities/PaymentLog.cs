namespace PaparaGo.Domain.Entities;

public class PaymentLog
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ExpenseRequestId { get; set; }

    public decimal Amount { get; set; }

    public string IBAN { get; set; } = string.Empty;

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    public string PaymentReference { get; set; } = string.Empty; // Sanal işlem no

    public string? Note { get; set; }

    // Navigation Property
    public ExpenseRequest? ExpenseRequest { get; set; }
}
