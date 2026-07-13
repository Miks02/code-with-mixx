using CodeWithMixx.Common.Results;

namespace CodeWithMixx.Domain.Entities.Students;

public static class StudentError
{
    public static Error NotFound(string identifier = "")
    {
        string message = string.IsNullOrWhiteSpace(identifier)
            ? "Student not found"
            : $"Student with identifier '{identifier}' is not found";

        return new Error("Student.NotFound", message);
    }

    public static Error UniversityRequired()
        => new("Student.UniversityRequired", "University is required for this operation");

    public static Error AlreadyRegistered(string identifier = "")
    {
        string message = string.IsNullOrWhiteSpace(identifier)
            ? "Student is already registered"
            : $"Student with identifier '{identifier}' is already registered";

        return new Error("Student.AlreadyRegistered", message);
    }
}

