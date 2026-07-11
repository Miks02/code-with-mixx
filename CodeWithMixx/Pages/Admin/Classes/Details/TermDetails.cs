using System.Globalization;
using CodeWithMixx.Domain.Entities.Classes;
using CodeWithMixx.Domain.Entities.Reservations;

namespace CodeWithMixx.Pages.Admin.Classes.Details
{
    public record TermDetails
    {
        public int ReservationId { get; init; }
        public int SubjectId { get; init; }
        public string SubjectName { get; init; } = string.Empty;
        public string StudentId { get; init; } = string.Empty;
        public string StudentName { get; init; } = string.Empty;
        public string Notes { get; init; } = string.Empty;
        public ClassStatus ClassStatus { get; init; }
        public PaymentStatus PaymentStatus { get; init; }
        public decimal PricePerClass => Classes.Any() ? Classes[0].Price : 0;
        public decimal TotalPrice { get; init; }
        public decimal PaidAmount { get; init; }
        public DateTime TermStartDate => Classes.First().StartsAt;
        public DateTime TermEndDate => Classes.Last().EndsAt;
        public int TotalClasses => Classes.Count;
        public IReadOnlyList<ClassItem> Classes { get; init; } = [];

        public record ClassItem
        {
            public int ClassId { get; init; }
            public DateTime StartsAt { get; init; }
            public DateTime EndsAt { get; init; }
            public decimal Price { get; init; }
        }
    }
}
