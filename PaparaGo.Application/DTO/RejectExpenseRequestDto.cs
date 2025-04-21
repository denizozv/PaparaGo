namespace PaparaGo.DTO;

public class RejectExpenseRequestDto
{
    public Guid ExpenseRequestId { get; set; }
    public string RejectionReason { get; set; } = string.Empty;
}
