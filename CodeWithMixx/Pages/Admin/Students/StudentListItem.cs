using CodeWithMixx.Domain.Entities.AppUsers;

namespace CodeWithMixx.Pages.Admin.Students;

public record StudentListItem
{
    public string Initials { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string University { get; set; } = string.Empty;
    public int LessonCount { get; set; }
    public AccountStatus Status { get; set; }
    public bool ActiveProject { get; set; }
    public string RegisteredAt { get; set; } = null!;
};