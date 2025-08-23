using Contacts.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Api.Reports;

public class MonthlyReportService(AppDbContext db) : IMonthlyReportService
{
    public async Task<IReadOnlyList<MonthlyReportRow>> GetMonthlyReportAsync(int year, CancellationToken ct = default)
    {
        // שולפים נתונים נקיים מה-DB (ללא מעקב) ואז מסכמים בזיכרון
        var raw = await db.ContactRequests
            .AsNoTracking()
            .Where(r => r.CreatedAtUtc.Year == year)
            .Select(r => new { r.CreatedAtUtc, r.Departments })
            .ToListAsync(ct);

        // הגנה מלאה מ-null + פריסה של מחלקות
        var rows = raw
            .SelectMany(r => (r.Departments ?? new List<string>()), (r, d) => new { r.CreatedAtUtc, Department = d })
            .GroupBy(x => new { x.CreatedAtUtc.Year, Month = x.CreatedAtUtc.Month, x.Department })
            .Select(g => new MonthlyReportRow(g.Key.Year, g.Key.Month, g.Key.Department, g.Count()))
            .OrderBy(x => x.Month).ThenBy(x => x.Department)
            .ToList();

        return rows;
    }
}
