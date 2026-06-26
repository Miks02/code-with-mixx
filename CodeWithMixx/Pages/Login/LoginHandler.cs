using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.AppUsers;
using CodeWithMixx.Domain.ErrorCatalog;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Login;

public class LoginHandler(
    SignInManager<AppUser> signInManager,
    ILogger<LoginHandler> logger,
    AppDbContext context) : IHandler
{
    public async Task<Result> HandleAsync(LoginInputModel request, CancellationToken ct = default)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == request.Email, ct);
        
        if (user is null)
            return Result.Failure(AuthError.LoginFailed("Login has failed, user not found"));
        
        var canSignInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (canSignInResult.IsLockedOut)
            return Result.Failure(AuthError.AccountLocked(user.Id));
        
        if(!canSignInResult.Succeeded)
            return Result.Failure(AuthError.LoginFailed());       
        
        if (user.AccountStatus == AccountStatus.Deactivated)
            return Result.Failure(AuthError.AccountDeactivated(user.Id));
        
        if (user.AccountStatus == AccountStatus.Suspended) 
            return Result.Failure(AuthError.AccountSuspended(user.Id));

        await signInManager.SignInAsync(user, isPersistent: request.RememberMe);
        
        user.LastLoginAt = DateTime.UtcNow;

        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning(ex, "Failed to save 'LastLoginAt' to database for user with id {UserId}", user.Id);
        }
        
        return Result.Success();    

    }
}