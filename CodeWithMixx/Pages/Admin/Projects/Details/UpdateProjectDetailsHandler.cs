using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.Admins;
using CodeWithMixx.Domain.Entities.Projects;
using CodeWithMixx.Domain.Entities.Reservations;
using CodeWithMixx.Domain.Entities.Subjects;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Projects.Details;

public class UpdateProjectDetailsHandler(AppDbContext context) : IHandler
{
    public async Task<Result> HandleAsync(ProjectDetailsInput request, string adminId, CancellationToken ct)
    {
        var adminExists = await context.Admins.AnyAsync(a => a.AppUserId == adminId, ct);
        if (!adminExists)
            return Result.Failure(AdminError.NotFound(adminId));

        var project = await context.Projects
            .Include(p => p.Reservation)
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, ct);

        if (project is null)
            return Result.Failure(ProjectError.NotFound(request.ProjectId));

        var subjectExists = await context.Subjects.AnyAsync(s => s.Id == request.SubjectId, ct);
        if (!subjectExists)
            return Result.Failure(SubjectError.NotFound(request.SubjectId));

        project.Reservation.TotalPrice = request.Price;
        project.Reservation.PaidAmount = request.PaidAmount;
        project.Reservation.Notes = request.Notes;
        project.Reservation.Bonus = CalculateBonus(project.Reservation.TotalPrice, project.Reservation.PaidAmount);
        project.Reservation.PaymentStatus = GetPaymentStatus(project.Reservation.TotalPrice, project.Reservation.PaidAmount, request.EndDate);

        project.SubjectId = request.SubjectId;
        project.ProjectStatus = request.ProjectStatus;
        project.StartDate = request.StartDate;
        project.EndDate = request.EndDate;

        await context.SaveChangesAsync(ct);

        return Result.Success();
    }

    private decimal CalculateBonus(decimal totalPrice, decimal paidAmount)
    {
        if (totalPrice >= paidAmount)
            return 0;

        var bonus = paidAmount - totalPrice;

        return Math.Round(bonus, 2);
    }

    private PaymentStatus GetPaymentStatus(decimal totalPrice, decimal paidAmount, DateOnly endDate)
    {
        return totalPrice switch
        {
            var price when price <= paidAmount => PaymentStatus.Paid,
            var price when price > paidAmount && DateOnly.FromDateTime(DateTime.UtcNow) > endDate => PaymentStatus.Overdue,
            var price when paidAmount > 0 && paidAmount < price => PaymentStatus.PartiallyPaid,
            _ => PaymentStatus.Pending
        };
    }
}
