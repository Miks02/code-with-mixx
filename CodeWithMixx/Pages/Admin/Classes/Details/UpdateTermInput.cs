using FluentValidation;

namespace CodeWithMixx.Pages.Admin.Classes.Details;

public record UpdateTermInput
{
    public int ReservationId { get; init; }
    public int ClassStatus { get; init; }
    public decimal TotalPrice { get; init; }
    public decimal AmountOfClasses { get; init; }
    public decimal PaidAmount { get; init; }
    public DateTime? FirstClassStartsAt { get; init; }
}

public class UpdateTermInputValidator : AbstractValidator<UpdateTermInput>
{
    public UpdateTermInputValidator()
    {
        RuleSet("ClassStatus", () =>
        {
            RuleFor(x => x.ReservationId)
                .GreaterThan(0)
                .WithMessage("Nevažeći identifikator rezervacije");

            RuleFor(x => x.ClassStatus)
                .InclusiveBetween(0, 3)
                .WithMessage("Nevažeći status termina");
        });

        RuleSet("TotalPrice", () =>
        {
            RuleFor(x => x.ReservationId)
                .GreaterThan(0)
                .WithMessage("Nevažeći identifikator rezervacije");

            RuleFor(x => x.TotalPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Ukupna cena mora biti pozitivna vrednost");
        });

        RuleSet("PaidAmount", () =>
        {
            RuleFor(x => x.ReservationId)
                .GreaterThan(0)
                .WithMessage("Nevažeći identifikator rezervacije");

            RuleFor(x => x.PaidAmount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Plaćeni iznos mora biti pozitivna vrednost");
        });

        RuleSet("AmountOfClasses", () =>
        {
            RuleFor(x => x.ReservationId)
                .GreaterThan(0)
                .WithMessage("Nevažeći identifikator rezervacije");

            RuleFor(x => x.AmountOfClasses)
                .GreaterThan(0)
                .WithMessage("Broj časova mora biti veći od nule");
        });

        RuleSet("FirstClassStart", () =>
        {
            RuleFor(x => x.ReservationId)
                .GreaterThan(0)
                .WithMessage("Nevažeći identifikator rezervacije");

            RuleFor(x => x.FirstClassStartsAt)
                .NotNull()
                .WithMessage("Datum i vreme prvog časa su obavezni");
        });
    }
}

