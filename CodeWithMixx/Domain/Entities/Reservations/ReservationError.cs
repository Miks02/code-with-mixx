using CodeWithMixx.Common.Results;

namespace CodeWithMixx.Domain.Entities.Reservations;

public static class ReservationError
{
    public static Error NotFound(int? identifier = null)
    {
        string message = identifier is null
            ? "Reservation not found"
            : $"Reservation with identifier '{identifier}' is not found";

        return new Error("Reservation.NotFound", message);
    }

    public static Error AlreadyPaid(int? identifier = null)
    {
        string message = identifier is null
            ? "Reservation is already fully paid"
            : $"Reservation with identifier '{identifier}' is already fully paid";

        return new Error("Reservation.AlreadyPaid", message);
    }

    public static Error InvalidAmount(decimal amount)
        => new("Reservation.InvalidAmount", $"Amount '{amount}' must be greater than zero");
}

