using CodeWithMixx.Domain.Entities.Admins;
using CodeWithMixx.Domain.Entities.Classes;
using CodeWithMixx.Domain.Entities.Projects;
using CodeWithMixx.Domain.Entities.Students;

namespace CodeWithMixx.Domain.Entities.Reservations;

public class Reservation
{
    public int Id { get; set; }

    public ServiceType ServiceType { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public Decimal TotalPrice { get; set; }
    public Decimal PaidAmount { get; set; }

    public Decimal DiscountRate { get; set; }

    public Decimal Bonus { get; set; }
    
    public string? Notes { get; set; }

    public Admin Admin { get; set; } = null!;
    public string AdminId { get; set; } = null!;  
    
    public Student Student { get; set; } = null!;
    public string StudentId { get; set; } = null!;
    
    public Project? Project { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Class> Classes { get; set; } = [];

}