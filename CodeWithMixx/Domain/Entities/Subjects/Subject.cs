using CodeWithMixx.Domain.Entities.Admins;
using CodeWithMixx.Domain.Entities.Classes;
using CodeWithMixx.Domain.Entities.Students;

namespace CodeWithMixx.Domain.Entities.Subjects;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    public Admin? Admin { get; set; } = null!;
    public string? AdminId { get; set; }

    public ICollection<Class> Classes { get; set; } = [];
}