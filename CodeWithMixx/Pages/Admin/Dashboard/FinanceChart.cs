namespace CodeWithMixx.Pages.Admin.Dashboard;

public record FinanceChart
{
    public int? SelectedYear { get; init; }
    public IReadOnlyList<decimal> IncomeByMonth { get; init; } = [];
    public IReadOnlyList<int> StudentsCountByMonth { get; init; } = [];
};