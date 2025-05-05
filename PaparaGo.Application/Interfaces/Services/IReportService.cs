using PaparaGo.DTO.Reports;

public interface IReportService
{

    Task<IEnumerable<CategoryDistributionDto>> GetCategoryDistributionAsync();
    Task<IEnumerable<TopSpenderDto>> GetTopSpendersAsync();
    Task<IEnumerable<DailyAverageExpenseDto>> GetDailyAveragesAsync();
    Task<IEnumerable<CompanyDailyExpenseSummaryDto>> GetCompanyDailyExpensesAsync();
    Task<IEnumerable<CompanyWeeklyExpenseSummaryDto>> GetCompanyWeeklyExpensesAsync();
    Task<IEnumerable<CompanyMonthlyExpenseSummaryDto>> GetCompanyMonthlyExpensesAsync();
}
