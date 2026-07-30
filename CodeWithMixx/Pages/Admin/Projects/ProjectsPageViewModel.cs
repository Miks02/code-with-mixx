using CodeWithMixx.Common.Results;

namespace CodeWithMixx.Pages.Admin.Projects;

public record ProjectsPageViewModel
{
    public int TotalProjects { get; init; }
    public int CompletedProjects { get; init; }
    public int OngoingProjects { get; init; }
    public int CancelledProjects { get; init; }
    public int StandbyProjects { get; init; }
    public IReadOnlyList<SubjectDropdownItem> Subjects { get; init; } = [];
    public PagedResult<ProjectReservationListItem> ProjectsPage { get; init; } = null!;
    
    public record SubjectDropdownItem
    {
        public int Id { get; init; }
        public string SubjectName { get; set; } = null!;
    }
};