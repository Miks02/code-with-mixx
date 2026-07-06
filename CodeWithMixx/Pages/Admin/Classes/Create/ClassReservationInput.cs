using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Classes.Create
{
    public record ClassReservationInput
    {
        public int SubjectId { get; init; }
        public string StudentId { get; init; } = null!;

        [Precision(18,2)]
        public Decimal TotalPrice { get; init; }
        [Precision(18,2)]
        public Decimal PaidAmount { get; init; }

        public string? Notes { get; init; }
        public IReadOnlyList<ReservationClass> Classes { get; init; } = [];

        public record ReservationClass
        {
            [Precision(18, 2)]
            public Decimal Price { get; init; } = 1300;
            public DateTime StartsAt { get; init; }
            public DateTime EndsAt => StartsAt.AddMinutes(45);
        }
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

            RuleFor(x => x.Classes)
                .NotEmpty().WithMessage("Dodajte bar jedan čas");

            RuleFor(x => x.PaidAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Uneti broj mora biti pozitivan");

            RuleFor(x => x.TotalPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Uneti broj mora biti pozitivan");
        }
    }

    public class ReservationClassValidator : AbstractValidator<ClassReservationInput.ReservationClass>
    {
        public ReservationClassValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Uneti broj mora biti pozitivan");

            RuleFor(x => x.StartsAt)
                .NotEmpty().WithMessage("Unesite vreme početka časa");
        }
    }
}