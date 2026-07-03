namespace CodeWithMixx.Pages.Admin.Students.Create;

public record StudentInput
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string? PhoneNumber { get; init; } 
    public string Password { get; init; } = null!;
    public string ConfirmPassword { get; init; } = null!;
    public string? University { get; init; }
    
};