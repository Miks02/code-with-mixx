using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.Projects;
using CodeWithMixx.Infrastructure.Persistence;
using CodeWithMixx.Pages.Admin.Classes.Create;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Projects.Details;

public class GetProjectDetailsHandler(AppDbContext context, GetStudentHandler studentHandler) : IHandler
{
    public async Task<Result<(ProjectDetailsInput Input, StudentSearchResultItem Student)>> HandleAsync(int projectId, CancellationToken ct)
    {
        var project = await context.Projects
            .Select(p => new 
            {
                ProjectId = p.Id,
                Reservation = p.Reservation,
                ReservationId = p.ReservationId,
                SubjectId = p.SubjectId,
                StudentId = p.Reservation.StudentId,
                Notes = p.Reservation.Notes,
                PaymentStatus = p.Reservation.PaymentStatus,
                ProjectStatus = p.ProjectStatus,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Price = p.Reservation.TotalPrice,
                PaidAmount = p.Reservation.PaidAmount
            })
            .FirstOrDefaultAsync(p => p.ProjectId == projectId, ct);

        if (project is null)
            return Result<(ProjectDetailsInput, StudentSearchResultItem)>.Failure(ProjectError.NotFound(projectId));

        var studentResult = await studentHandler.HandleAsync(project.Reservation.StudentId, ct);
        if (!studentResult.IsSuccess)
            return Result<(ProjectDetailsInput, StudentSearchResultItem)>.Failure(studentResult.Errors[0]);

        var input = new ProjectDetailsInput
        {
            ProjectId = project.ProjectId,
            ReservationId = project.ReservationId,
            SubjectId = project.SubjectId,
            StudentId = project.StudentId,
            Notes = project.Notes,
            PaymentStatus = project.PaymentStatus,
            ProjectStatus = project.ProjectStatus,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Price = project.Price,
            PaidAmount = project.PaidAmount
        };

        return Result<(ProjectDetailsInput, StudentSearchResultItem)>.Success((input, studentResult.Payload));
    }
}
