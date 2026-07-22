using CodeWithMixx.Common.Results;

namespace CodeWithMixx.Domain.Entities.Projects;

public static class ProjectError
{
    public static Error NotFound(int? identifier = null)
    {
        string message = identifier is null
            ? "Project not found"
            : $"Project with identifier '{identifier}' is not found";

        return new Error("Project.NotFound", message);
    }
    
    public static Error AlreadyCompleted(int? identifier = null)
    {
        string message = identifier is null
            ? "Project is already completed"
            : $"Project with identifier '{identifier}' is already completed";

        return new Error("Project.AlreadyCompleted", message);
    }
}