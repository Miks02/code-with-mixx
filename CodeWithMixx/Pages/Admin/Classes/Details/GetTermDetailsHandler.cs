using System.Globalization;
using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.Classes;
using CodeWithMixx.Domain.Entities.Reservations;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Classes.Details;

public class GetTermDetailsHandler(AppDbContext context) : IHandler
{
    public async Task<Result<TermDetails>> HandleAsync(int reservationId, CancellationToken ct = default)
    {
        var reservation = await context.Reservations
            .Where(r => r.Id == reservationId)
            .Select(r => new TermDetails
            {
                ReservationId = r.Id,
                StudentId = r.StudentId,
                StudentName = r.Student.AppUser.FirstName + " " + r.Student.AppUser.LastName,
                SubjectName = r.Classes.Select(c => c.Subject.Name).FirstOrDefault()!,
                TotalPrice = r.TotalPrice,
                PaidAmount = r.PaidAmount,
                Notes = r.Notes ?? "",
                DiscountRate = r.DiscountRate,
                BonusAmount = r.Bonus,
                PaymentStatus = r.PaymentStatus,
                ClassStatus = r.Classes.Any(c => c.ClassStatus == ClassStatus.Ongoing) ? ClassStatus.Ongoing :
                    r.Classes.All(c => c.ClassStatus == ClassStatus.Completed) ? ClassStatus.Completed :
                    r.Classes.All(c => c.ClassStatus == ClassStatus.Cancelled) ? ClassStatus.Cancelled :
                    ClassStatus.Scheduled,
                Classes = r.Classes.Select(c => new TermDetails.ClassItem
                {
                    ClassId = c.Id,
                    StartsAt = c.StartsAt,
                    EndsAt = c.EndsAt,
                    Price = c.Price
                }).ToList()
            })
            .FirstOrDefaultAsync(ct);

        if (reservation is null)
            return Result<TermDetails>.Failure(ReservationError.NotFound(reservationId));
        
        return Result<TermDetails>.Success(reservation);
    }
}