namespace PaparaGo.DTO;

public class ExpenseRequestResponseDto
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? DocumentPath { get; set; }
    public DateTime RequestDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? RejectionReason { get; set; }
}
