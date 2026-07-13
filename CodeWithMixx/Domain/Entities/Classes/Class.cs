using CodeWithMixx.Domain.Entities.Reservations;
using CodeWithMixx.Domain.Entities.Subjects;

namespace CodeWithMixx.Domain.Entities.Classes
{
    public class Class
    {
        public int Id { get; set; }
        public Decimal Price { get; set; } = 1300;

        public ClassStatus ClassStatus { get; set; } = ClassStatus.Scheduled;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }

        public Subject Subject { get; set; } = null!;
        public int SubjectId { get; set; }

        public Reservation Reservation { get; set; } = null!;
        public int ReservationId { get; set; }
    }
}
