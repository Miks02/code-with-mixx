using System.Globalization;
using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.Projects;
using CodeWithMixx.Infrastructure.Persistence;
using CodeWithMixx.Pages.Admin.Classes;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Projects;

public class GetProjectsPageHandler(AppDbContext context) : IHandler
{
    public async Task<ProjectsPageViewModel> HandleAsync(
        string? filter, 
        string? sort, 
        int? subjectId,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var query = context.Projects.AsQueryable();

        query = sort switch
        {
            "reservation_asc" => query.OrderBy(p => p.ReservedAt),
            "reservation_desc" => query.OrderByDescending(p => p.ReservedAt),
            _ => query
        };
        
        query = filter switch
        {
            "ongoing" => query.Where(p => p.ProjectStatus == ProjectStatus.Ongoing),
            "completed" => query.Where(p => p.ProjectStatus == ProjectStatus.Completed),
            "cancelled" => query.Where(p => p.ProjectStatus == ProjectStatus.Cancelled),
            "standby" => query.Where(p => p.ProjectStatus == ProjectStatus.Standby),
            _ => query
        };
        
        if(subjectId is not null)
            query = query.Where(r => r.SubjectId == subjectId);

        var projectsQuery = query
            .Select(p => new ProjectReservationListItem
            {
                Id = p.Id,
                StudentName = p.Reservation.Student.AppUser.FirstName + " " + p.Reservation.Student.AppUser.LastName,
                TotalPrice = p.Reservation.TotalPrice,
                PaymentStatus = p.Reservation.PaymentStatus,
                ProjectStatus = p.ProjectStatus,
                SubjectName = p.Subject.Name,
                StartDate = p.StartDate.ToString("yy.M.dd HH:mm", CultureInfo.InvariantCulture),
                EndDate = p.EndDate.ToString("yy.M.dd HH:mm", CultureInfo.InvariantCulture),
                ReservedAt = p.ReservedAt.ToString("yy.M.dd HH:mm", CultureInfo.InvariantCulture),
            });

        var stats = await context.Projects
            .GroupBy(_ => 1)
            .Select(g => new
            {
                Total = g.Count(),
                Completed = g.Count(p => p.ProjectStatus == ProjectStatus.Completed),
                Cancelled = g.Count(p => p.ProjectStatus == ProjectStatus.Cancelled),
                Standby = g.Count(p => p.ProjectStatus == ProjectStatus.Standby),
                Ongoing = g.Count(p => p.ProjectStatus == ProjectStatus.Ongoing)
            })
            .FirstOrDefaultAsync(ct);

        var subjects = await context.Subjects
            .Select(s => new ProjectsPageViewModel.SubjectDropdownItem
            {
                Id = s.Id,
                SubjectName = s.Name
            })
            .ToListAsync(ct);

        var pagedResult = PagedResult<ProjectReservationListItem>.CreateAsync(projectsQuery, page, pageSize, ct);
        
        return new ProjectsPageViewModel
        {
            TotalProjects = stats?.Total ?? 0,
            CompletedProjects = stats?.Completed ?? 0,
            OngoingProjects = stats?.Ongoing ?? 0,
            CancelledProjects = stats?.Cancelled ?? 0,
            StandbyProjects = stats?.Standby ?? 0,
            Subjects = subjects,
            ProjectsPage = pagedResult.Result
        };
    }
}