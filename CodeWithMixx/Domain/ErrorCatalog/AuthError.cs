using CodeWithMixx.Common.Results;

namespace CodeWithMixx.Domain.ErrorCatalog;

public static class AuthError
{
    public static Error RegistrationFailed(string message = "Unexpected error happened during registration")
        => new("Auth.RegistrationFailed", message);
        
    public static Error LoginFailed(string message = "Login has failed, invalid credentials")
        => new("Auth.LoginFailed", message);

    public static Error AccountDeactivated(string identifier = "")
    {
        return string.IsNullOrWhiteSpace(identifier)
            ? new("Auth.AccountDeactivated", "Account is deactivated")
            : new("Auth.AccountDeactivated", $"Account with identifier '{identifier}' is deactivated");
    }

    public static Error AccountSuspended(string identifier = "")
    {
        return string.IsNullOrWhiteSpace(identifier)
            ? new("Auth.AccountSuspended", "Account is suspended")
            : new("Auth.AccountSuspended", $"Account with identifier '{identifier}' is suspended");
    }

    public static Error AccountLocked(string identifier = "")
    {
        return string.IsNullOrWhiteSpace(identifier)
            ? new("Auth.AccountLocked", "Account is locked")
            : new("Auth.AccountLocked", $"Account with identifier '{identifier}' is locked");
    }
    
    public static Error PasswordError(string message = "Error occurred while trying to assign password to the user")
        => new("Auth.InvalidCredentials", message);

    public static Error InvalidCurrentPassword(
        string message = "Entered password does not match the current password")
        => new("Auth.InvalidCurrentPassword", message);

    public static Error PasswordTooShort(string message = "Password is too short")
        => new("Auth.PasswordTooShort", message);

    public static Error PasswordRequiresDigit(string message = "Password must contain at least one digit ('0'-'9')")
        => new("Auth.PasswordRequiresDigit", message);

    public static Error PasswordRequiresUpper(string message = "Password must contain at least one uppercase letter ('A'-'Z')")
        => new("Auth.PasswordRequiresUpper", message);

    public static Error PasswordRequiresNonAlphanumeric(string message = "Password must contain at least one special character")
        => new("Auth.PasswordRequiresNonAlphanumeric", message);
    
}