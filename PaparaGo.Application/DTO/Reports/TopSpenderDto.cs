namespace PaparaGo.DTO.Reports
{
    public class TopSpenderDto
    {
        public string UserFullName { get; set; } = string.Empty;
        public decimal TotalSpent { get; set; }
        public int ExpenseCount { get; set; }
    }
}
