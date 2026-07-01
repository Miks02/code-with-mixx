using CodeWithMixx.Domain.Entities.AppUsers;

namespace CodeWithMixx.Pages.Admin.Students;

public record StudentListItem
{
    public string Id { get; init; }
    public string Initials { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string University { get; init; } = string.Empty;
    public int LessonCount { get; init; }
    public AccountStatus Status { get; init; }
    public bool ActiveProject { get; init; }
    public string RegisteredAt { get; init; } = null!;
};