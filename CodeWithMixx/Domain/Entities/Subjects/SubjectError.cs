using CodeWithMixx.Common.Results;

namespace CodeWithMixx.Domain.Entities.Subjects;

public class SubjectError
{
    public static Error NotFound(int? identifier = null)
    {
        string message = identifier == null
            ? "Subject not found"
            : $"Subject with identifier '{identifier}' is not found";

        return new Error("Subject.NotFound", message);
    }
    
    public static Error AlreadyExists(string identifier = "")
    {
        string message = string.IsNullOrWhiteSpace(identifier)
            ? "Subject already exists"
            : $"Subject with identifier '{identifier}' already exists";

        return new Error("Subject.AlreadyExists", message);
    }
}