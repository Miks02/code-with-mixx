using CodeWithMixx.Common.Results;
using FluentValidation;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Students.Create;

public class IndexModel(IValidator<StudentInput> validator, CreateStudentHandler handler) : PageModel
{
    
    [BindProperty]
    public StudentInput Input { get; set; } = new StudentInput();
    
    public IActionResult OnGet()
    {
        if(Request.IsHtmx())
            return Partial("_Create", Input);

        return Page();
    }

    public async Task<IActionResult> OnPost(CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(Input, ct);
        
        if(!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                Console.WriteLine(error.PropertyName);
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            
            return Partial("_Create", Input);
        }

        var result = await handler.HandleAsync(Input, ct);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = GetErrorMessage(result.Errors[0]);
            return Partial("_Create", Input);
        }

        TempData["SuccessMessage"] = "Student je uspešno kreiran!";
        return RedirectToPage("/Admin/Students/Index");
    }

    private string GetErrorMessage(Error error)
    {
        return error.Code switch
        {
            "DuplicateUserName" => "Email adresa je zauzeta.",
            "UserAlreadyInRole" => "Korisnik je već u toj ulozi.",
            "InvalidRoleName" => "Uloga ne postoji.",
            "ConcurrencyFailure" => "Došlo je do greške, student je ažuriran u toku modifikacije",
            _ => "Došlo je do nepoznate greške. Pogledaj logove za detalje"
        };
    }
}