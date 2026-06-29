using System.Globalization;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Dashboard;

public class IndexModel : PageModel
{
    private DashboardViewModel _vm = new DashboardViewModel
    {
        MonthlyIncome = 30000.ToString("N0"),
        Lessons = 90,
        ActiveStudents = 50,
        ProjectsCount = 10,
        UpcomingLessons = new List<UpcomingLessonListItem>
        {
            new UpcomingLessonListItem
            {
                Initials = "AP",
                FullName = "Andrija Pantović",
                LessonName = "OOP - Nasledjivanje",
                LessonDate = new DateTime(2024, 6, 16, 14, 0, 0).ToString("yyyy-MM-dd hh:mm tt", new CultureInfo("sr-Latn-RS")),
                Email = "andrija@gmail.com",
                PhoneNumber = "123-456-7890"
            },
            new UpcomingLessonListItem()
            {
                Initials = "MJ",
                FullName = "Milica Jovanović",
                LessonName = "Web razvoj - HTML i CSS",
                LessonDate = new DateTime(2024, 6, 17, 10, 0, 0).ToString("yyyy-MM-dd hh:mm tt", new CultureInfo("sr-Latn-RS")),
                Email = "milica@gmail.com",
                PhoneNumber = "111-222-3333"
            },
            new UpcomingLessonListItem()
            {
                Initials = "NS",
                FullName = "Nikola Stanković",
                LessonName = "Baze podataka - SQL",
                LessonDate = new DateTime(2024, 6, 17, 12, 0, 0).ToString("yyyy-MM-dd hh:mm tt", new CultureInfo("sr-Latn-RS")),
                Email = "nikola@gmail.com",
                PhoneNumber = "444-555-6666"
            },
            new UpcomingLessonListItem()
            {
                Initials = "IP",
                FullName = "Isidora Petrović",
                LessonName = "Algoritmi i strukture podataka",
                LessonDate = new DateTime(2024, 6, 18, 9, 0, 0).ToString("yyyy-MM-dd hh:mm tt", new CultureInfo("sr-Latn-RS")),
                Email = "isidora@gmail.com",
                PhoneNumber = "777-888-9999"
            }
        }
    };

    public DashboardViewModel ViewModel { get; set; } = null!;
    
    public IActionResult OnGet()
    {
        ViewModel = _vm;
        
        if(Request.IsHtmx())
            return Partial("_Dashboard", _vm);
        
        return Page();
    }
}