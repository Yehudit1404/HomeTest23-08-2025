namespace Contacts.Api.Reports;

public record MonthlyReportRow(int Year, int Month, string Department, int Count);

public interface IMonthlyReportService
{
    Task<IReadOnlyList<MonthlyReportRow>> GetMonthlyReportAsync(int year, CancellationToken ct = default);
}
