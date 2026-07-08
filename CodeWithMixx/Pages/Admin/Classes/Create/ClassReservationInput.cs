using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Classes.Create
{
    public record ClassReservationInput
    {
        public int SubjectId { get; init; }
        public string StudentId { get; set; } = null!;

        [Precision(18, 2)] 
        public Decimal PricePerClass { get; init; } = 1300;
        [Precision(18,2)]
        public Decimal TotalPrice { get; init; }
        [Precision(18,2)]
        public Decimal PaidAmount { get; init; }

        public string? Notes { get; init; }
        public double AmountOfClasses { get; init; } = 1;
        public DateTime FirstClassStart { get; init; }
    }

    public class ClassReservationInputValidator : AbstractValidator<ClassReservationInput>
    {
        public ClassReservationInputValidator()
        {
            RuleFor(x => x.SubjectId)
                .GreaterThan(0)
                .WithMessage("Izaberite predmet rezervacije");

            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Izaberite studenta");

            RuleFor(x => x.AmountOfClasses)
                .GreaterThan(0).WithMessage("Dodajte barem jedan čas");

            RuleFor(x => x.PaidAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Uneti broj mora biti pozitivan");

            RuleFor(x => x.PricePerClass)
                .GreaterThanOrEqualTo(0).WithMessage("Uneti broj mora biti pozitivan");
    
            RuleFor(x => x.TotalPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Uneti broj mora biti pozitivan");
            
            RuleFor(x => x.FirstClassStart)
                .NotEmpty().WithMessage("Izaberite datum i vreme prvog časa");
            
        }
        
    }
    
}