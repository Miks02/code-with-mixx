namespace CodeWithMixx.Pages.Admin.Classes
{
    public record TermDetails
    {
        public int ReservationId { get; init; }
        public string StudentName { get; init; } = string.Empty;
        public string SubjectName { get; init; } = string.Empty;
        public Decimal TotalPrice { get; init; }
        public IReadOnlyList<ClassItem> Classes { get; init; } = [];
        public int TotalClasses => Classes.Count;

        public record ClassItem
        {
            public int ClassId { get; init; }
            public string StartsAt { get; init; } = string.Empty;
            public string EndsAt { get; init; } = string.Empty;
            public Decimal Price { get; init; }
        }
    }
}
