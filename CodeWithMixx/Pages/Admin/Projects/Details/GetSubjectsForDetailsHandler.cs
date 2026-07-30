using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Projects.Details;

public class GetSubjectsForDetailsHandler(AppDbContext context) : IHandler
{
    public async Task<ProjectDetailsViewModel> HandleAsync(CancellationToken ct)
    {
        var subjects = await context.Subjects
            .Select(s => new ProjectDetailsViewModel.SubjectDropdownItem
            {
                Id = s.Id,
                Name = s.Name
            })
            .ToListAsync(ct);

        return new ProjectDetailsViewModel { Subjects = subjects };
    }
}
