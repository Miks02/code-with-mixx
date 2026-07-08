using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Classes.Create;

public record CreatePageViewModel
{
    public IReadOnlyList<SubjectDropdownItem> Subjects { get; init; } = [];
    
    public StudentSearchResultItem? SelectedStudent { get; init; }

    public record SubjectDropdownItem
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
    }
}

public class GetCreatePageHandler(AppDbContext context) : IHandler
{
    public async Task<CreatePageViewModel> HandleAsync(CancellationToken ct)
    {
        var subjects = await context.Subjects
            .Select(s => new CreatePageViewModel.SubjectDropdownItem
            {
                Id = s.Id,
                Name = s.Name
            })
            .ToListAsync(ct);

        return new CreatePageViewModel { Subjects = subjects };
    }
}
