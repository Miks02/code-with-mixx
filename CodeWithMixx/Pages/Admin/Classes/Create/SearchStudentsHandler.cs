using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Classes.Create;

public record StudentSearchResultItem
{
    public string Id { get; init; } = null!;
    public string Initials { get; init; } = null!;
    public string FullName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string University { get; init; } = null!;
}

public class SearchStudentsHandler(AppDbContext context) : IHandler
{
    public async Task<IReadOnlyList<StudentSearchResultItem>> HandleAsync(string search, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(search))
            return [];

        var lower = search.ToLower();

        return await context.Students
            .Where(s => s.AppUser.FirstName.ToLower().Contains(lower)
                     || s.AppUser.LastName.ToLower().Contains(lower)
                     || s.AppUser.Email!.ToLower().Contains(lower))
            .Take(8)
            .Select(s => new StudentSearchResultItem
            {
                Id = s.AppUserId,
                Initials = s.AppUser.FirstName.Substring(0, 1) + s.AppUser.LastName.Substring(0, 1),
                FullName = s.AppUser.FirstName + " " + s.AppUser.LastName,
                Email = s.AppUser.Email!,
                University = s.University ?? "N/A"
            })
            .ToListAsync(ct);
    }
}
