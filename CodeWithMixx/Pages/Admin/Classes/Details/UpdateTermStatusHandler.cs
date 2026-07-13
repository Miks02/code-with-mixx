using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.Classes;
using CodeWithMixx.Domain.Entities.Reservations;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Classes.Details;

public class UpdateTermStatusHandler(AppDbContext context) : IHandler
{
    public async Task<Result> HandleAsync(int reservationId, ClassStatus status, CancellationToken ct = default)
    {
        if(!await context.Reservations.AnyAsync(r => r.Id == reservationId, ct))
            return Result.Failure(ReservationError.NotFound(reservationId));
        
        await context.Classes
           .Where(c => c.ReservationId == reservationId)
           .ExecuteUpdateAsync(s => s.SetProperty(c => c.ClassStatus, status), ct);
        
        return Result.Success();
    }
    
    
}