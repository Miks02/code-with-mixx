using System.Globalization;
using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.Projects;
using CodeWithMixx.Infrastructure.Persistence;

namespace CodeWithMixx.Pages.Admin.Projects;

public class GetProjectsHandler(AppDbContext context) : IHandler
{
    public Task<PagedResult<ProjectReservationListItem>> HandleAsync(
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

        if (subjectId is not null)
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

        return PagedResult<ProjectReservationListItem>.CreateAsync(projectsQuery, page, pageSize, ct);
    }
}