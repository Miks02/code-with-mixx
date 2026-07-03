using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.AppUsers;
using CodeWithMixx.Domain.Entities.Students;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace CodeWithMixx.Pages.Admin.Students.Create;

public class CreateStudentHandler(UserManager<AppUser> userManager, AppDbContext context, ILogger<CreateStudentHandler> logger) : IHandler
{
    public async Task<Result> HandleAsync(StudentInput request, CancellationToken ct = default)
{
    await using var transaction = await context.Database.BeginTransactionAsync(ct);
    
    try
    {
        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
        };

        var createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            logger.LogError("Failed to create user: {Errors}", string.Join(", ", createResult.Errors.Select(e => e.Description)));
            return Result.Failure(createResult.Errors.ToArray());
        }
        
        var roleResult = await userManager.AddToRoleAsync(user, "Student");
        if (!roleResult.Succeeded)
        {
            logger.LogError("Failed to assign role to user: {Errors}", string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            return Result.Failure(roleResult.Errors.ToArray());     
        }  

        var student = new Student
        {
            AppUserId = user.Id,
            University = request.University
        };
        
        context.Students.Add(student);
        await context.SaveChangesAsync(ct);

        await transaction.CommitAsync(ct);
        
        return Result.Success();
    }
    catch (Exception ex)
    {
        logger.LogError("An unexpected error occurred during student registration: {Error}", ex.Message);
        throw;
    }
}
}