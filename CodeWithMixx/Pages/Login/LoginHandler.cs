using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain;
using CodeWithMixx.Domain.Entities.AppUsers;
using Microsoft.AspNetCore.Identity;

namespace CodeWithMixx.Pages.Login;

public class LoginHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : IHandler
{
    public async Task<Result> HandleAsync(LoginInputModel request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        
        if (user is null)
            return Result.Failure(UserError.NotFound());

        var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, false);

        if (!result.Succeeded)
            return Result.Failure(AuthError.LoginFailed());
        
        return Result.Success();    

    }
}