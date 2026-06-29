namespace CodeWithMixx.Pages.Admin.Dashboard;

public record UpcomingLessonListItem
{
    public string Initials { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string LessonName { get; init; } = string.Empty;
    public string LessonDate { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
}