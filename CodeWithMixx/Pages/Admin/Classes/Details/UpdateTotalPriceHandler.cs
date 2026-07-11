using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.Reservations;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Classes.Details;

public class UpdateTotalPriceHandler(AppDbContext context) : IHandler
{
    public async Task<Result> HandleAsync(int reservationId, decimal totalPrice, CancellationToken ct = default)
    {
        var reservation = await context.Reservations
            .Include(r => r.Classes)
            .FirstOrDefaultAsync(r => r.Id == reservationId, ct);

        if (reservation is null)
            return Result.Failure(ReservationError.NotFound(reservationId));

        var classes = reservation.Classes.OrderBy(c => c.StartsAt).ToList();
        var pricePerClass = classes.Any() ? classes.First().Price : 0;
        var totalMinutes = classes.Sum(c => (decimal)(c.EndsAt - c.StartsAt).TotalMinutes);
        var amountOfClasses = totalMinutes / 45m;
        var classDate = classes.Any() ? classes.First().StartsAt : DateTime.UtcNow;

        reservation.TotalPrice = totalPrice;
        reservation.DiscountRate = CalculateDiscountRate(totalPrice, pricePerClass, amountOfClasses);
        reservation.Bonus = CalculateBonus(totalPrice, reservation.PaidAmount);
        reservation.PaymentStatus = GetPaymentStatus(totalPrice, reservation.PaidAmount, classDate);

        await context.SaveChangesAsync(ct);
        return Result.Success();
    }

    private decimal CalculateDiscountRate(decimal totalPrice, decimal pricePerClass, decimal amountOfClasses)
    {
        var defaultTotal = amountOfClasses * pricePerClass;
        if (defaultTotal <= 0 || defaultTotal < totalPrice)
            return 0;

        var discountRate = (defaultTotal - totalPrice) / defaultTotal * 100;
        return Math.Round(discountRate, 2);
    }

    private decimal CalculateBonus(decimal totalPrice, decimal paidAmount)
    {
        if (totalPrice >= paidAmount)
            return 0;

        return Math.Round(paidAmount - totalPrice, 2);
    }

    private PaymentStatus GetPaymentStatus(decimal totalPrice, decimal paidAmount, DateTime classDate)
    {
        return totalPrice switch
        {
            var price when price <= paidAmount => PaymentStatus.Paid,
            var price when price > paidAmount && DateTime.UtcNow.Date > classDate.Date => PaymentStatus.Overdue,
            var price when paidAmount > 0 && paidAmount < price => PaymentStatus.PartiallyPaid,
            _ => PaymentStatus.Pending
        };
    }
}