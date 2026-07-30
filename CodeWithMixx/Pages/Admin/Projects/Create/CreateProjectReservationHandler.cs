using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.Admins;
using CodeWithMixx.Domain.Entities.Projects;
using CodeWithMixx.Domain.Entities.Reservations;
using CodeWithMixx.Domain.Entities.Students;
using CodeWithMixx.Domain.Entities.Subjects;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Projects.Create;

public class CreateProjectReservationHandler(AppDbContext context) : IHandler
{
    public async Task<Result> HandleAsync(CreateProjectInput request, string adminId, CancellationToken ct)
    {
        var adminExists = await context.Admins.AnyAsync(a => a.AppUserId == adminId, ct);
        if (!adminExists)
            return Result.Failure(AdminError.NotFound(adminId));

        var studentExists = await context.Students.AnyAsync(s => s.AppUserId == request.StudentId, ct);
        if (!studentExists)
            return Result.Failure(StudentError.NotFound(request.StudentId));

        var subjectExists = await context.Subjects.AnyAsync(s => s.Id == request.SubjectId, ct);
        if (!subjectExists)
            return Result.Failure(SubjectError.NotFound(request.SubjectId));

        var reservation = new Reservation
        {
            AdminId = adminId,
            StudentId = request.StudentId,
            ServiceType = ServiceType.Project,
            TotalPrice = request.Price,
            PaidAmount = request.PaidAmount,
            Notes = request.Notes
        };

        reservation.Bonus = CalculateBonus(reservation.TotalPrice, reservation.PaidAmount);
        reservation.PaymentStatus = GetPaymentStatus(reservation.TotalPrice, reservation.PaidAmount, request.EndDate);

        var project = new Project
        {
            Reservation = reservation,
            SubjectId = request.SubjectId,
            ProjectStatus = request.ProjectStatus,
            StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc),
            EndDate = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc),
            ReservedAt = DateTime.UtcNow
        };

        context.Reservations.Add(reservation);
        context.Projects.Add(project);

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

    private PaymentStatus GetPaymentStatus(decimal totalPrice, decimal paidAmount, DateTime endDate)
    {
        return totalPrice switch
        {
            var price when price <= paidAmount => PaymentStatus.Paid,
            var price when price > paidAmount && DateTime.UtcNow.Date > endDate.Date => PaymentStatus.Overdue,
            var price when paidAmount > 0 && paidAmount < price => PaymentStatus.PartiallyPaid,
            _ => PaymentStatus.Pending
        };
    }
}
