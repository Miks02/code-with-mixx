using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.Classes;
using CodeWithMixx.Domain.Entities.Reservations;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Classes.Details;

public class UpdateNumberOfClassesHandler(AppDbContext context) : IHandler
{
    public async Task<Result> HandleAsync(int reservationId, decimal amountOfClasses, CancellationToken ct = default)
    {
        if (!await context.Reservations.AnyAsync(r => r.Id == reservationId, ct))
            return Result.Failure(ReservationError.NotFound(reservationId));

        var classes = await context.Classes
            .Where(c => c.ReservationId == reservationId)
            .OrderBy(c => c.StartsAt)
            .ToListAsync(ct);

        if (!classes.Any())
            return Result.Failure(ReservationError.NotFound(reservationId));

        var ceiledAmount = (int)Math.Ceiling(amountOfClasses);
        var currentCount = classes.Count;

        if (ceiledAmount < currentCount)
        {
            context.Classes.RemoveRange(classes.Skip(ceiledAmount));
            classes = classes.Take(ceiledAmount).ToList();
        }
        else if (ceiledAmount > currentCount)
        {
            var lastClass = classes.Last();

            lastClass.EndsAt = DateTime.SpecifyKind(lastClass.StartsAt.AddMinutes(45), DateTimeKind.Utc);

            var nextStart = lastClass.EndsAt;

            for (int i = currentCount + 1; i <= ceiledAmount; i++)
            {
                var newClass = new Class
                {
                    ReservationId = reservationId,
                    SubjectId = lastClass.SubjectId,
                    Price = lastClass.Price,
                    ClassStatus = ClassStatus.Scheduled,
                    StartsAt = DateTime.SpecifyKind(nextStart, DateTimeKind.Utc),
                    EndsAt = DateTime.SpecifyKind(nextStart.AddMinutes(45), DateTimeKind.Utc)
                };
                context.Classes.Add(newClass);
                classes.Add(newClass);
                nextStart = nextStart.AddMinutes(45);
            }
        }

        classes.Last().EndsAt = GetLastClassEndTime(amountOfClasses, classes.Last().StartsAt);

        await context.SaveChangesAsync(ct);
        return Result.Success();
    }

    private DateTime GetLastClassEndTime(decimal classAmount, DateTime lastClassStartTime)
    {
        var timeModulus = classAmount - Math.Floor(classAmount);

        if (timeModulus == 0)
            return DateTime.SpecifyKind(lastClassStartTime.AddMinutes(45), DateTimeKind.Utc);

        var minutesToAdd = timeModulus * 45;
        var roundedMinutes = Math.Round(minutesToAdd, 0);

        return DateTime.SpecifyKind(lastClassStartTime.AddMinutes((double)roundedMinutes), DateTimeKind.Utc);
    }
}
