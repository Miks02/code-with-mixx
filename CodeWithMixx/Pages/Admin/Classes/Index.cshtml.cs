using CodeWithMixx.Domain.Entities.Classes;
using CodeWithMixx.Infrastructure.Web;
using CodeWithMixx.Pages.Admin.Classes.Details;
using FluentValidation;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Classes;

public class IndexModel(
    GetClassesPageHandler getPageHandler,
    GetClassesHandler getClassesHandler,
    GetTermDetailsHandler getTermDetailsHandler,
    UpdateTermStatusHandler updateTermStatusHandler,
    UpdatePaymentStatusHandler updatePaymentStatusHandler,
    UpdateTotalPriceHandler updateTotalPriceHandler,
    UpdateNumberOfClassesHandler updateNumberOfClassesHandler,
    UpdateFirstClassStartHandler updateFirstClassStartHandler,
    IValidator<UpdateTermInput> validator) : PageModel
{
    private readonly int _pageSize = 14;

    public ClassPageViewModel ViewModel = new();

    public async Task<IActionResult> OnGet(
        [FromQuery] string? filter,
        [FromQuery] string? sort,
        [FromQuery] int? subjectId,
        [FromQuery] int page = 1,
        [FromQuery] int? reservationId = null,
        CancellationToken ct = default)
    {
        ViewModel = await getPageHandler.HandleAsync(filter, sort, subjectId, page, _pageSize, ct);

        if (reservationId.HasValue)
        {
            var termResult = await getTermDetailsHandler.HandleAsync(reservationId.Value, ct);
            if (termResult.IsSuccess)
                ViewModel = ViewModel with { CurrentTerm = termResult.Payload };
        }

        if (Request.IsHtmx())
            return Partial("_ClassPage", ViewModel);

        return Page();
    }

    public async Task<IActionResult> OnGetClasses(
        [FromQuery] string? filter,
        [FromQuery] string? sort,
        [FromQuery] int? subjectId,
        [FromQuery] int page = 1,
        CancellationToken ct = default)
    {
        var pagedClasses = await getClassesHandler.HandleAsync(filter, sort, subjectId, page, _pageSize, ct);

        if (Request.IsHtmx())
            return Partial("_ClassList", pagedClasses);

        return RedirectToPage("/admin/classes/index", new { filter, sort, subjectId, page });
    }

    public async Task<IActionResult> OnGetTermDetails(int reservationId, CancellationToken ct = default)
    {
        var result = await getTermDetailsHandler.HandleAsync(reservationId, ct);

        if (!result.IsSuccess)
        {
            Response.ShowToast("Detalji termina nisu pronađeni.", "error");
            return Partial("_ClassList", ViewModel);
        }

        if (Request.IsHtmx())
            return Partial("Admin/Classes/Details/_TermDetails", result.Payload);

        return RedirectToPage("/admin/classes/index", new { reservationId });
    }

    public async Task<IActionResult> OnPostUpdateTermStatus(
        [FromForm] UpdateTermInput input,
        CancellationToken ct = default)
    {
        var validation = await validator.ValidateAsync(input, opt => opt.IncludeRuleSets("ClassStatus"), ct);
        if (!validation.IsValid)
        {
            Response.ShowToast(validation.Errors[0].ErrorMessage, "error");
            return await ReloadPage(input.ReservationId, ct);
        }

        var result = await updateTermStatusHandler.HandleAsync(input.ReservationId, (ClassStatus)input.ClassStatus, ct);
        if (!result.IsSuccess)
        {
            Response.ShowToast("Greška pri ažuriranju statusa termina.", "error");
            return await ReloadPage(input.ReservationId, ct);
        }

        Response.ShowToast("Status termina je uspešno ažuriran.");
        return await ReloadPage(input.ReservationId, ct);
    }

    public async Task<IActionResult> OnPostUpdateTotalPrice(
        [FromForm] UpdateTermInput input,
        CancellationToken ct = default)
    {
        var validation = await validator.ValidateAsync(input, opt => opt.IncludeRuleSets("TotalPrice"), ct);
        if (!validation.IsValid)
        {
            Response.ShowToast(validation.Errors[0].ErrorMessage, "error");
            return await ReloadPage(input.ReservationId, ct);
        }

        var result = await updateTotalPriceHandler.HandleAsync(input.ReservationId, input.TotalPrice, ct);
        if (!result.IsSuccess)
        {
            Response.ShowToast("Greška pri ažuriranju ukupne cene.", "error");
            return await ReloadPage(input.ReservationId, ct);
        }

        Response.ShowToast("Ukupna cena je uspešno ažurirana.");
        return await ReloadPage(input.ReservationId, ct);
    }

    public async Task<IActionResult> OnPostUpdatePaymentStatus(
        [FromForm] UpdateTermInput input,
        CancellationToken ct = default)
    {
        var validation = await validator.ValidateAsync(input, opt => opt.IncludeRuleSets("PaidAmount"), ct);
        if (!validation.IsValid)
        {
            Response.ShowToast(validation.Errors[0].ErrorMessage, "error");
            return await ReloadPage(input.ReservationId, ct);
        }

        var result = await updatePaymentStatusHandler.HandleAsync(input.ReservationId, input.PaidAmount, ct);
        if (!result.IsSuccess)
        {
            Response.ShowToast("Greška pri ažuriranju plaćenog iznosa.", "error");
            return await ReloadPage(input.ReservationId, ct);
        }

        Response.ShowToast("Plaćeni iznos je uspešno ažuriran.");
        return await ReloadPage(input.ReservationId, ct);
    }

    public async Task<IActionResult> OnPostUpdateNumberOfClasses(
        [FromForm] UpdateTermInput input,
        CancellationToken ct = default)
    {
        var validation = await validator.ValidateAsync(input, opt => opt.IncludeRuleSets("AmountOfClasses"), ct);
        if (!validation.IsValid)
        {
            Response.ShowToast(validation.Errors[0].ErrorMessage, "error");
            return await ReloadPage(input.ReservationId, ct);
        }

        var result = await updateNumberOfClassesHandler.HandleAsync(input.ReservationId, input.AmountOfClasses, ct);
        if (!result.IsSuccess)
        {
            Response.ShowToast("Greška pri ažuriranju broja časova.", "error");
            return await ReloadPage(input.ReservationId, ct);
        }

        Response.ShowToast("Broj časova je uspešno ažuriran.");
        return await ReloadPage(input.ReservationId, ct);
    }

    public async Task<IActionResult> OnPostUpdateFirstClassStart(
        [FromForm] UpdateTermInput input,
        CancellationToken ct = default)
    {
        var validation = await validator.ValidateAsync(input, opt => opt.IncludeRuleSets("FirstClassStart"), ct);
        if (!validation.IsValid)
        {
            Response.ShowToast(validation.Errors[0].ErrorMessage, "error");
            return await ReloadPage(input.ReservationId, ct);
        }

        var result = await updateFirstClassStartHandler.HandleAsync(input.ReservationId, input.FirstClassStartsAt!.Value, ct);
        if (!result.IsSuccess)
        {
            Response.ShowToast("Greška pri ažuriranju početka prvog časa.", "error");
            return await ReloadPage(input.ReservationId, ct);
        }

        Response.ShowToast("Početak prvog časa je uspešno ažuriran.");
        return await ReloadPage(input.ReservationId, ct);
    }

    private async Task<IActionResult> ReloadPage(int reservationId, CancellationToken ct)
    {
        var filter = Request.Form["filter"].ToString();
        var sort = Request.Form["sort"].ToString();
        _ = int.TryParse(Request.Form["subjectId"], out var subjectId);
        _ = int.TryParse(Request.Form["page"], out var page);

        var viewModel = await getPageHandler.HandleAsync(filter, sort, subjectId == 0 ? null : subjectId, page == 0 ? 1 : page, _pageSize, ct);
        var termResult = await getTermDetailsHandler.HandleAsync(reservationId, ct);

        if (termResult.IsSuccess)
            viewModel = viewModel with { CurrentTerm = termResult.Payload };

        Response.Headers["HX-Replace-Url"] = $"/admin/classes?reservationId={reservationId}";

        if (Request.IsHtmx())
            return Partial("_ClassPage", viewModel);

        return RedirectToPage("/admin/classes/index", new { reservationId });
    }
}