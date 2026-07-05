using CodeWithMixx.Domain.Entities.Classes;
using CodeWithMixx.Domain.Entities.Reservations;

namespace CodeWithMixx.Pages.Admin.Classes;

public record ClassListItem
{
    public int Id { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string StartsAt { get; set; } = string.Empty;
    public string EndsAt { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
    public ClassStatus ClassStatus { get; set; }
};