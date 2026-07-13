using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.AppUsers;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Students.Details;

public class DeleteAccountHandler(AppDbContext context) : IHandler
{
    public async Task<Result> HandleAsync(string id, CancellationToken ct = default)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == id, ct);

        if (user is null)
            return Result.Failure(UserError.NotFound(id));
        
        context.Users.Remove(user);
        await context.SaveChangesAsync(ct);
        return Result.Success();
    }
}