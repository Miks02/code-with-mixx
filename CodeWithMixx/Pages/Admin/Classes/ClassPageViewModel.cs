using CodeWithMixx.Common.Results;

namespace CodeWithMixx.Pages.Admin.Classes;

public record ClassPageViewModel
{
    public int TotalClasses { get; init; }
    public int UpcomingClasses { get; init; }
    public int FinishedClasses { get; init; }
    public int CancelledClasses { get; init; }
    public IReadOnlyList<SubjectDropdownItem> Subjects { get; init; } = [];
    public PagedResult<ClassListItem> Classes { get; init; } = null!;

    public record SubjectDropdownItem
    {
        public int Id { get; init; }
        public string SubjectName { get; set; } = null!;
    }
}
    