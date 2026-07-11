using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.Reservations;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Classes.Details;

public class UpdateFirstClassStartHandler(AppDbContext context) : IHandler
{
    public async Task<Result> HandleAsync(int reservationId, DateTime newFirstStart, CancellationToken ct = default)
    {
        var classes = await context.Classes
            .Where(c => c.ReservationId == reservationId)
            .OrderBy(c => c.StartsAt)
            .ToListAsync(ct);

        if (!classes.Any())
            return Result.Failure(ReservationError.NotFound(reservationId));

        var delta = newFirstStart - classes[0].StartsAt;

        foreach (var cls in classes)
        {
            cls.StartsAt += delta;
            cls.EndsAt += delta;
        }

        await context.SaveChangesAsync(ct);
        return Result.Success();
    }
}

