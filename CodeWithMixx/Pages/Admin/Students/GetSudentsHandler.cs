using System.Globalization;
using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.AppUsers;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Students;

public class GetStudentsHandler(AppDbContext context) : IHandler
{
    public async Task<PagedResult<StudentListItem>> HandleAsync(int page, int pageSize, string sort, string filter, string search, CancellationToken ct = default)
    {
        var query = context.Students.AsQueryable();
        
        query = sort switch
        {
            "name" => query.OrderBy(s => s.AppUser.FirstName).ThenBy(s => s.AppUser.LastName),
            "registration" => query.OrderBy(s => s.AppUser.CreatedAt),
            "registration_desc" => query.OrderByDescending(s => s.AppUser.CreatedAt),
            "status" => query.OrderBy(s => s.AppUser.AccountStatus),
            _ => query.OrderBy(s => s.AppUser.FirstName).ThenBy(s => s.AppUser.LastName)
        };
        
        query = filter switch
        {
            "active" => query.Where(s => s.AppUser.AccountStatus == AccountStatus.Active),
            "deactivated" => query.Where(s => s.AppUser.AccountStatus == AccountStatus.Deactivated),
            "suspended" => query.Where(s => s.AppUser.AccountStatus == AccountStatus.Suspended),
            _ => query
        };
        
        if(!string.IsNullOrWhiteSpace(search))
        {
            var lowerSearch = search.ToLower();
            query = query.Where(s => s.AppUser.FirstName.ToLower().Contains(lowerSearch) 
                                     || s.AppUser.LastName.ToLower().Contains(lowerSearch) 
                                     || s.AppUser.Email!.ToLower().Contains(lowerSearch));
        }

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Select(s => new StudentListItem
            {
                Id = s.AppUserId,
                Initials = (s.AppUser.FirstName.Length > 0 ? s.AppUser.FirstName.Substring(0, 1) : "") + 
                           (s.AppUser.LastName.Length > 0 ? s.AppUser.LastName.Substring(0, 1) : ""),
                FullName = $"{s.AppUser.FirstName} {s.AppUser.LastName}",
                Email = s.AppUser.Email!,
                PhoneNumber = s.AppUser.PhoneNumber ?? "N/A",
                University = s.University ?? "N/A",
                RegisteredAt = s.AppUser.CreatedAt.ToString("MMMM dd, yyyy", new CultureInfo("sr-Latn-RS")),
                Status = s.AppUser.AccountStatus
            })
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
 
        
        return new PagedResult<StudentListItem>(items, page, pageSize, totalCount);
    }
}