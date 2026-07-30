using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Projects.Create;

public class GetSubjectsHandler(AppDbContext context) : IHandler
{
    public async Task<CreateProjectViewModel> HandleAsync(CancellationToken ct)
    {
        var subjects = await context.Subjects
            .Select(s => new CreateProjectViewModel.SubjectDropdownItem
            {
                Id = s.Id,
                Name = s.Name
            })
            .ToListAsync(ct);

        return new CreateProjectViewModel { Subjects = subjects };
    }
    
}