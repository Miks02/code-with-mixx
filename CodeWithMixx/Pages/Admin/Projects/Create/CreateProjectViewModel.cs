using CodeWithMixx.Pages.Admin.Classes.Create;

namespace CodeWithMixx.Pages.Admin.Projects.Create;

public record CreateProjectViewModel
{
    public IReadOnlyList<SubjectDropdownItem> Subjects { get; init; } = [];
    public StudentSearchResultItem? SelectedStudent { get; init; }

    public record SubjectDropdownItem
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
    }
}
