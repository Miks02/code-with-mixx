using System.ComponentModel.DataAnnotations;
using System.Threading.RateLimiting;
using CodeWithMixx.Common.Extensions;
using CodeWithMixx.Common.Results;
using Discord;
using Discord.WebSocket;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.RateLimiting;
using static CodeWithMixx.Domain.Discord.DiscordError;

namespace CodeWithMixx.Pages;

public enum ContactFormType
{
    [Display(Name = "Individualni časovi")]
    Individual, 
    [Display(Name = "Priprema za ispite")]
    ExamPreparation, 
    [Display(Name = "Projekat/Seminarski")]
    Project, 
    [Display(Name = "Saradnja")]
    Cooperation
}

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

public class ContactHandler(DiscordSocketClient client, IConfiguration config, ILogger<ContactHandler> logger)
{
    private readonly ulong _channelId = config.GetValue<ulong>("Discord:ReservationsChannelId");
    
    public async Task<Result> Handle(ContactFormViewModel request)
    {
        var channel = await client.GetChannelAsync(_channelId) as IMessageChannel;

        if (channel is null)
        {
            logger.LogError("Unable to get a channel with id {_channelId}", _channelId);
            return Result.Failure(ChannelNotFound(_channelId));
        }
        
        var embed = new EmbedBuilder()
            .WithTitle("Stigao je novi upit!")
            .WithColor(Color.Green)
            .AddField("Poslao: ", $"{request.FirstName} {request.LastName}")
            .AddField("Email: ", request.Email)
            .AddField("Broj telefona: ", request.PhoneNumber)
            .AddField("Tip upita: ", request.Type.GetDisplayName())
            .AddField("Poruka: ", request.Message)
            .WithCurrentTimestamp()
            .Build();
        
        await channel.SendMessageAsync(embed: embed);

        return Result.Success();
    }
}

public class IndexModel(
    IValidator<ContactFormViewModel> contactValidator,
    ContactHandler handler,
    PartitionedRateLimiter<HttpContext> limiter) : PageModel
{
    [BindProperty] 
    public ContactFormViewModel ContactForm { get; set; } = null!;

    [DisableRateLimiting]
    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPost(CancellationToken ct = default)
    {
        using var lease = await limiter.AcquireAsync(HttpContext);
        if (!lease.IsAcquired)
        {
            TempData["ErrorMessage"] = "Previše zahteva, pokušaj opet za 2 minuta.";
            return Partial("Shared/_Partials/Landing/_Contact");
        }
        
        var validationResult = await contactValidator.ValidateAsync(ContactForm, ct);

        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));
            return Partial("Shared/_Partials/Landing/_Contact");
        }
        
        var result = await handler.Handle(ContactForm);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = "Došlo je do greške prilikom slanja forme. Molim te pokušaj ponovo kasnije.";
            return Partial("Shared/_Partials/Landing/_Contact");
        }

        ModelState.Clear();
        ContactForm = new ContactFormViewModel();
        
        TempData["SuccessMessage"] = "Forma je poslata uspešno!";
        
        return Partial("Shared/_Partials/Landing/_Contact");
    }
}