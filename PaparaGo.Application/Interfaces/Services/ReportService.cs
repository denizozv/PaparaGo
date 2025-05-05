using Dapper;
using Npgsql;
using PaparaGo.DTO.Reports;
using PaparaGo.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

public class ReportService : IReportService
{
    private readonly IConfiguration _configuration;

    public ReportService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    }

    public async Task<IEnumerable<CompanyDailyExpenseSummaryDto>> GetCompanyDailyExpensesAsync()
    {
        const string sql = "SELECT * FROM vw_company_daily_expense_summary";
        using var connection = GetConnection();
        return (await connection.QueryAsync<CompanyDailyExpenseSummaryDto>(sql)).ToList();
    }

    public async Task<IEnumerable<CompanyWeeklyExpenseSummaryDto>> GetCompanyWeeklyExpensesAsync()
    {
        const string sql = "SELECT * FROM vw_company_weekly_expense_summary";
        using var connection = GetConnection();
        return (await connection.QueryAsync<CompanyWeeklyExpenseSummaryDto>(sql)).ToList();
    }

    public async Task<IEnumerable<CompanyMonthlyExpenseSummaryDto>> GetCompanyMonthlyExpensesAsync()
    {
        const string sql = "SELECT * FROM vw_company_monthly_expense_summary";
        using var connection = GetConnection();
        return (await connection.QueryAsync<CompanyMonthlyExpenseSummaryDto>(sql)).ToList();
    }

    // Personel günlük raporu
    public async Task<IEnumerable<PersonalDailyExpenseSummaryDto>> GetPersonalDailyExpensesAsync(Guid userId)
    {
        const string sql = "SELECT * FROM vw_personal_daily_expense_summary WHERE \"UserId\" = @UserId";
        using var connection = GetConnection();
        return (await connection.QueryAsync<PersonalDailyExpenseSummaryDto>(sql, new { UserId = userId })).ToList();
    }

    // Personel haftalık raporu
    public async Task<IEnumerable<PersonalWeeklyExpenseSummaryDto>> GetPersonalWeeklyExpensesAsync(Guid userId)
    {
        const string sql = "SELECT * FROM vw_personal_weekly_expense_summary WHERE \"UserId\" = @UserId";
        using var connection = GetConnection();
        return (await connection.QueryAsync<PersonalWeeklyExpenseSummaryDto>(sql, new { UserId = userId })).ToList();
    }

    // Personel aylık raporu
    public async Task<IEnumerable<PersonalMonthlyExpenseSummaryDto>> GetPersonalMonthlyExpensesAsync(Guid userId)
    {
        const string sql = "SELECT * FROM vw_personal_monthly_expense_summary WHERE \"UserId\" = @UserId";
        using var connection = GetConnection();
        return (await connection.QueryAsync<PersonalMonthlyExpenseSummaryDto>(sql, new { UserId = userId })).ToList();
    }

    public async Task<IEnumerable<CategoryDistributionDto>> GetCategoryDistributionAsync()
    {
        const string sql = "SELECT * FROM vw_categorydistribution";
        using var connection = GetConnection();
        return (await connection.QueryAsync<CategoryDistributionDto>(sql)).ToList();
    }


    public async Task<IEnumerable<TopSpenderDto>> GetTopSpendersAsync()
    {
        const string sql = "SELECT * FROM vw_topspenders";
        using var connection = GetConnection();
        return (await connection.QueryAsync<TopSpenderDto>(sql)).ToList();
    }

    // daily avarege
    public async Task<IEnumerable<DailyAverageExpenseDto>> GetDailyAveragesAsync()
    {
        const string sql = "SELECT * FROM vw_dailyaverageexpense";
        using var connection = GetConnection();
        return (await connection.QueryAsync<DailyAverageExpenseDto>(sql)).ToList();
    }
}
