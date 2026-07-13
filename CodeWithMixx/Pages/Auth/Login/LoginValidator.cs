using FluentValidation;

namespace CodeWithMixx.Pages.Auth.Login;

public class LoginValidator : AbstractValidator<LoginInputModel>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email adresa je obavezna.");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Lozinka je obavezna.");
    }
}