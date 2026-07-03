namespace CodeWithMixx.Pages.Admin.Students.Details
{
    public record StudentDetailsInputModel
    {
        public string Id { get; init; } = string.Empty;
        public string Initials { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string University { get; init; } = string.Empty;
        public string RegisteredAt { get; init; } = string.Empty;
    }
}
