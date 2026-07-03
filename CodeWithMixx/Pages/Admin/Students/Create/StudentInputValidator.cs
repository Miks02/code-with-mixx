using FluentValidation;

namespace CodeWithMixx.Pages.Admin.Students.Create;

public class StudentInputValidator : AbstractValidator<StudentInput>
{
    public StudentInputValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email adresa je obavezna.")
            .EmailAddress().WithMessage("Email adresa nije validna.");
        
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ime je obavezno.")
            .MinimumLength(2).WithMessage("Ime mora imati najmanje 2 karaktera.")
            .MaximumLength(50).WithMessage("Ime ne sme imati više od 50 karaktera.");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Prezime je obavezno.")
            .MinimumLength(2).WithMessage("Prezime mora imati najmanje 2 karaktera.")
            .MaximumLength(50).WithMessage("Prezime ne sme imati više od 50 karaktera.");
        
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?\d{10,15}$").WithMessage("Broj telefona nije validan.");
        
        RuleFor(x => x.University)
            .MaximumLength(100).WithMessage("Univerzitet ne sme imati više od 100 karaktera.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Lozinka je obavezna.")
            .Matches("[a-z]") .WithMessage("Polje mora sadržati bar jedno malo slovo.")
            .MinimumLength(8).WithMessage("Lozinka mora imati najmanje 8 karaktera.");
        
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Potvrda lozinke je obavezna.")
            .Equal(x => x.Password).WithMessage("Lozinke se ne podudaraju.");
    }
}