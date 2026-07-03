using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.AppUsers;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Students.Details;

public class UpdateAccountStatusHandler(AppDbContext context) : IHandler
{
    public async Task<Result> HandleAsync(string id, AccountStatus status, CancellationToken ct = default)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == id, ct);

        if (user is null)
            return Result.Failure(UserError.NotFound(id));

        user.AccountStatus = status;
        await context.SaveChangesAsync(ct);
        return Result.Success();   
    }
}