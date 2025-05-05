using PaparaGo.DTO.Reports;

public interface IReportService
{

    Task<IEnumerable<CategoryDistributionDto>> GetCategoryDistributionAsync();
    Task<IEnumerable<TopSpenderDto>> GetTopSpendersAsync();
    Task<IEnumerable<DailyAverageExpenseDto>> GetDailyAveragesAsync();
    Task<IEnumerable<CompanyDailyExpenseSummaryDto>> GetCompanyDailyExpensesAsync();
    Task<IEnumerable<CompanyWeeklyExpenseSummaryDto>> GetCompanyWeeklyExpensesAsync();
    Task<IEnumerable<CompanyMonthlyExpenseSummaryDto>> GetCompanyMonthlyExpensesAsync();
    Task<IEnumerable<PersonalDailyExpenseSummaryDto>> GetPersonalDailyExpensesAsync(Guid userId);
    Task<IEnumerable<PersonalWeeklyExpenseSummaryDto>> GetPersonalWeeklyExpensesAsync(Guid userId);
    Task<IEnumerable<PersonalMonthlyExpenseSummaryDto>> GetPersonalMonthlyExpensesAsync(Guid userId);

}
