using PaparaGo.DTO.Reports;

public interface IReportService
{
    Task<IEnumerable<WeeklyExpenseSummaryDto>> GetWeeklyExpenseSummaryAsync();
}
