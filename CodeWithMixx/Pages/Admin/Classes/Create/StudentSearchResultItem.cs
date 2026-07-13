namespace CodeWithMixx.Pages.Admin.Classes.Create;

public record StudentSearchResultItem
{
    public string Id { get; init; } = null!;
    public string Initials { get; init; } = null!;
    public string FullName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string University { get; init; } = null!;
};