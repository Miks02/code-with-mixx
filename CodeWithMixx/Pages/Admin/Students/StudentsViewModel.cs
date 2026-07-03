using CodeWithMixx.Common.Results;

namespace CodeWithMixx.Pages.Admin.Students;

public record StudentsViewModel
{
    public PagedResult<StudentListItem> StudentsPage { get; init; } = null!;
    
}