namespace PaparaGo.DTO.Reports
{
    public class CompanyWeeklyExpenseSummaryDto
    {
        public DateTime Week { get; set; }
        public int ExpenseCount { get; set; }
        public decimal TotalAmount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
    }
}