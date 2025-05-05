namespace PaparaGo.DTO.Reports
{
    public class CompanyDailyExpenseSummaryDto
    {
        public DateTime Date { get; set; }
        public int ExpenseCount { get; set; }
        public decimal TotalAmount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
    }
}