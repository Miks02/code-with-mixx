namespace CodeWithMixx.Pages.Admin.Dashboard;

public record DashboardViewModel
{
    public string MonthlyIncome { get; init; } = null!;
    public int Lessons { get; init; }
    public int ActiveStudents { get; init; }
    public int ProjectsCount { get; init; }
    public IReadOnlyList<UpcomingLessonListItem> UpcomingLessons { get; init; } = [];
}