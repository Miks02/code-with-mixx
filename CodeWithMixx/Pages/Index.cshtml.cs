using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages;

public enum ContactFormType { Individual, ExamPreparation, Project, Cooperation }

public class ContactFormViewModel
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public ContactFormType Type { get; set; } = ContactFormType.Individual;
    public string Message { get; set; } = null!;
    
};

public class ContactFormValidator : AbstractValidator<ContactFormViewModel>
{
    public ContactFormValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("Unesite vaše ime")
            .MinimumLength(2)
            .WithMessage("Ime ne sme biti kraće od 2 karaktera")
            .MaximumLength(20)
            .WithMessage("Ime ne sme biti duže od 20 karaktera");
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Unesite vaše prezime")
            .MinimumLength(2)
            .WithMessage("Prezime ne sme biti kraće od 2 karaktera")
            .MaximumLength(20)
            .WithMessage("Prezime ne sme biti duže od 20 karaktera");
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Unesite vašu email adresu")
            .EmailAddress()
            .WithMessage("Neispravan format email adrese");
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Unesite vaš broj telefona")
            .Matches(@"^\+?\d{10,15}$")
            .WithMessage("Neispravan format broja telefona");
        
        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Unesite poruku")
            .MaximumLength(500)
            .WithMessage("Poruka ne sme biti duža od 500 karaktera");
    }
}

public class ContactHandler
{
    public async Task Handle(ContactFormViewModel request, CancellationToken ct = default)
    {
        
        Console.WriteLine("Contact form submitted");
        Console.WriteLine($"First name: {request.FirstName}");
        Console.WriteLine($"Last name: {request.LastName}");
        Console.WriteLine($"Email: {request.Email}");
        Console.WriteLine($"Phone number: {request.PhoneNumber}");
        Console.WriteLine($"Message: {request.Message}");
        Console.WriteLine($"Type: {request.Type}");
        Console.WriteLine("Sending the submitted data to the email....");
        await Task.Delay(3000, ct);
        Console.WriteLine("Email sent!");
        
    }
}


public class IndexModel(IValidator<ContactFormViewModel> contactValidator, ContactHandler handler) : PageModel
{
    [BindProperty] 
    public ContactFormViewModel ContactForm { get; set; } = null!;
    
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(CancellationToken ct = default)
    {
        var validationResult = await contactValidator.ValidateAsync(ContactForm, ct);

        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));
            return Partial("Shared/_Partials/Landing/_Contact", ContactForm);
        }
        
        await handler.Handle(ContactForm, ct);

        TempData["SuccessMessage"] = "Forma je poslata uspešno!";
        
        ModelState.Clear();
        
        return Partial("Shared/_Partials/Landing/_Contact", ContactForm);
    }
}