namespace PaparaGo.DTO.Reports
{    public class PersonalMonthlyExpenseSummaryDto
    {
        public Guid UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;
        public int ExpenseCount { get; set; }
        public decimal TotalAmount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
    }
}