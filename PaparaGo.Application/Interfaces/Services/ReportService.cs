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

    public async Task<IEnumerable<WeeklyExpenseSummaryDto>> GetWeeklyExpenseSummaryAsync()
    {
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        var sql = "SELECT * FROM vw_WeeklyExpenseSummary";
        return (await connection.QueryAsync<WeeklyExpenseSummaryDto>(sql)).ToList();
    }
}
