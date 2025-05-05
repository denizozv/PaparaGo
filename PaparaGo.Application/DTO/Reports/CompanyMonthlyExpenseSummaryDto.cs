namespace PaparaGo.DTO.Reports
{
    public class CompanyMonthlyExpenseSummaryDto
    {
        public string Month { get; set; } = string.Empty;
        public int ExpenseCount { get; set; }
        public decimal TotalAmount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
    }
}