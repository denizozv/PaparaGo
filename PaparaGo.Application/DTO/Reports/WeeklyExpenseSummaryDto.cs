namespace PaparaGo.DTO.Reports
{
    public class WeeklyExpenseSummaryDto
    {
        public DateTime WeekStart { get; set; }
        public int ExpenseCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
