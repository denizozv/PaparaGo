namespace PaparaGo.DTO;

public class CreateExpenseRequestDto
{
    public Guid CategoryId { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; } = string.Empty;

    public string? DocumentPath { get; set; } // document type 
}
