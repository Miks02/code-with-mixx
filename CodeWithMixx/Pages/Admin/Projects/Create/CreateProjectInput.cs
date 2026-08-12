using CodeWithMixx.Domain.Entities.Projects;
using CodeWithMixx.Domain.Entities.Reservations;
using FluentValidation;

namespace CodeWithMixx.Pages.Admin.Projects.Create;

public record CreateProjectInput
{
    public int SubjectId { get; init; }
    public string StudentId { get; set; } = null!;
    public string? Notes { get; init; }
    public PaymentStatus PaymentStatus { get; init; } = PaymentStatus.Pending;
    public ProjectStatus ProjectStatus { get; init; } = ProjectStatus.Standby;
    public DateOnly StartDate { get; init; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public DateOnly EndDate { get; init; } = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7));
    public decimal Price { get; init; }
    public decimal PaidAmount { get; init; }
    
};

public class CreateProjectValidator : AbstractValidator<CreateProjectInput>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.SubjectId)
            .NotEmpty().WithMessage("Izaberite predmet projekta");
        
        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("Izaberite studenta.");
        
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Cena mora biti pozitivan broj");

        RuleFor(x => x.PaidAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Plaćeni iznos mora biti pozitivan broj");
        
        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Napomene ne mogu biti duže od 500 karaktera");
        
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Izaberite datum početka projekta");
        
        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("Izaberite datum završetka projekta")
            .GreaterThan(x => x.StartDate).WithMessage("Kraj projekta mora biti nakon početka projekta");
    }
}