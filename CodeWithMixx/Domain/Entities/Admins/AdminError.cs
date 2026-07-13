using CodeWithMixx.Common.Results;

namespace CodeWithMixx.Domain.Entities.Admins;

public static class AdminError
{
    public static Error NotFound(string identifier = "")
    {
        string message = string.IsNullOrWhiteSpace(identifier)
            ? "Admin not found"
            : $"Admin with identifier '{identifier}' is not found";

        return new Error("Admin.NotFound", message);
    }

    public static Error SubjectLimitExceeded(int maxSubjects)
        => new("Admin.SubjectLimitExceeded", $"Admin cannot manage more than '{maxSubjects}' subjects");

    public static Error UnauthorizedAction()
        => new("Admin.UnauthorizedAction", "Admin is not allowed to perform this action");
}

