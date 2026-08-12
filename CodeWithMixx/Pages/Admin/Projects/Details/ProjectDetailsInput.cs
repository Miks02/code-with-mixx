using CodeWithMixx.Domain.Entities.Projects;
using CodeWithMixx.Domain.Entities.Reservations;
using CodeWithMixx.Pages.Admin.Projects.Create;
using FluentValidation;

namespace CodeWithMixx.Pages.Admin.Projects.Details;

public record ProjectDetailsInput
{
    public int ProjectId { get; init; }
    public int ReservationId { get; init; }
    public int SubjectId { get; init; }
    public string StudentId { get; init; } = null!;
    public string? Notes { get; init; }
    public PaymentStatus PaymentStatus { get; init; }
    public ProjectStatus ProjectStatus { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public decimal Price { get; init; }
    public decimal PaidAmount { get; init; }
}

public class ProjectDetailsValidator : AbstractValidator<ProjectDetailsInput>
{
    public ProjectDetailsValidator()
    {
        RuleFor(x => x.SubjectId)
            .NotEmpty().WithMessage("Izaberite predmet projekta");
        
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
