using System.Globalization;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.AppUsers;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Students;

public class IndexModel(GetStudentsHandler handler) : PageModel
{
    public StudentsViewModel ViewModel { get; set; } = null!;
    private const int PageSize = 9;
    
    public async Task<IActionResult> OnGet([FromQuery]int page = 1, string search = "", string filter = "", string sort = "name")
    {
        ViewModel = new StudentsViewModel
        {
            StudentsPage = await handler.HandleAsync(page, PageSize, sort, filter, search)
        };
        
        if (Request.IsHtmx())
            return Partial("_Students", ViewModel);
        
        return Page();
    }

    public async Task<IActionResult> OnGetStudents([FromQuery]int page = 1, string search = "", string filter = "", string sort = "name")
    {
        ViewModel = new StudentsViewModel
        {
            StudentsPage = await handler.HandleAsync(page, PageSize, sort, filter, search)
        };
        
        if(Request.IsHtmx())
            return Partial("_StudentsList", ViewModel.StudentsPage);
        return Page();
    }

    public IActionResult OnGetStudentDetails([FromQuery] string id)
    {
        return RedirectToPage("/Admin/Students/Details", new { id });
    }
    
    
}