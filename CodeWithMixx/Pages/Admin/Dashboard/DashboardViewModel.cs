namespace CodeWithMixx.Pages.Admin.Dashboard;

public record DashboardViewModel
{
    public decimal TotalIncome { get; init; }
    public int CompletedClasses { get; init; }
    public int ActiveStudents { get; init; }
    public int ProjectsCount { get; init; }
    public Dictionary<string, int> SubjectsChart { get; init; } = [];
    public IReadOnlyList<UpcomingTermItem> UpcomingTerms { get; init; } = []; 
}