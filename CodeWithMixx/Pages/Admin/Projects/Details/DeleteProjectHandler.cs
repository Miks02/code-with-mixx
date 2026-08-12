using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.Projects;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Projects.Details;

public class DeleteProjectHandler(AppDbContext context) : IHandler
{
    public async Task<Result> HandleAsync(int projectId, CancellationToken ct)
    {
        var projectToDelete = await context.Reservations
            .FirstOrDefaultAsync(r => r.Project.Id == projectId, ct);
        
        if (projectToDelete is null)
            return Result.Failure(ProjectError.NotFound(projectId));
        
        context.Reservations.Remove(projectToDelete);
        await context.SaveChangesAsync(ct);
        
        return Result.Success();
    }
}
