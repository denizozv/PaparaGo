namespace PaparaGo.DTO.Reports
{
  public class PersonalWeeklyExpenseSummaryDto
    {
        public Guid UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public DateTime Week { get; set; }
        public int ExpenseCount { get; set; }
        public decimal TotalAmount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
    }
}