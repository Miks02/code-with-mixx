using System.Globalization;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.AppUsers;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Students;

public class IndexModel : PageModel
{
    public StudentsViewModel ViewModel { get; set; } = null!;
    private const int PageSize = 9;
    
    public IActionResult OnGet([FromQuery]int page = 1)
    {
        ViewModel = new StudentsViewModel
        {
            StudentsPage = CreatePagedStudents(page)
        };
        
        if (Request.IsHtmx())
            return Partial("_Students", ViewModel);
        
        return Page();
    }

    public IActionResult OnGetStudents([FromQuery]int page = 1)
    {
        ViewModel = new StudentsViewModel
        {
            StudentsPage = CreatePagedStudents(page)
        };
        
        if(Request.IsHtmx())
            return Partial("_StudentsList", ViewModel.StudentsPage);
        return Page();
    }


    private List<StudentListItem> StudentsFactory()
    {
        var students = new List<StudentListItem>()
        {
            new StudentListItem()
            {
                Initials = "MN",
                FullName = "Marko Nikolić",
                Email = "marko.nikolic@gmail.com",
                PhoneNumber = "061-234-5678",
                University = "Računarski fakultet (RAF)",
                LessonCount = 14,
                ActiveProject = true,
                RegisteredAt = DateTime.Now.AddDays(-120).ToString("dd.MM.yyyy", new CultureInfo("sr-Latn-RS")),
                Status = AccountStatus.Active
            },
            new StudentListItem()
            {
                Initials = "AK",
                FullName = "Ana Kovačević",
                Email = "ana.kovacevic@gmail.com",
                PhoneNumber = "062-315-4876",
                University = "Fakultet organizacionih nauka (FON)",
                LessonCount = 9,
                ActiveProject = false,
                RegisteredAt = DateTime.Now.AddDays(-87).ToString("dd.MM.yyyy", new CultureInfo("sr-Latn-RS")),
                Status = AccountStatus.Active
            },
            new StudentListItem()
            {
                Initials = "LP",
                FullName = "Luka Petrović",
                Email = "luka.petrovic@gmail.com",
                PhoneNumber = "063-442-1987",
                University = "Univerzitet Singidunum",
                LessonCount = 18,
                ActiveProject = true,
                RegisteredAt = DateTime.Now.AddDays(-240).ToString("dd.MM.yyyy", new CultureInfo("sr-Latn-RS")),
                Status = AccountStatus.Deactivated
            },
            new StudentListItem()
            {
                Initials = "MS",
                FullName = "Milica Stojanović",
                Email = "milica.stojanovic@gmail.com",
                PhoneNumber = "064-557-3201",
                University = "Elektrotehnički fakultet (ETF)",
                LessonCount = 6,
                ActiveProject = false,
                RegisteredAt = DateTime.Now.AddDays(-35).ToString("dd.MM.yyyy", new CultureInfo("sr-Latn-RS")),
                Status = AccountStatus.Active
            },
            new StudentListItem()
            {
                Initials = "DV",
                FullName = "David Vasić",
                Email = "david.vasic@gmail.com",
                PhoneNumber = "065-681-4590",
                University = "Visoka škola strukovnih studija ITS",
                LessonCount = 11,
                ActiveProject = true,
                RegisteredAt = DateTime.Now.AddDays(-170).ToString("dd.MM.yyyy", new CultureInfo("sr-Latn-RS")),
                Status = AccountStatus.Active
            },
            new StudentListItem()
            {
                Initials = "NJ",
                FullName = "Nikola Jovanović",
                Email = "nikola.jovanovic@gmail.com",
                PhoneNumber = "066-734-5566",
                University = "Metropolitan Univerzitet",
                LessonCount = 7,
                ActiveProject = false,
                RegisteredAt = DateTime.Now.AddDays(-59).ToString("dd.MM.yyyy", new CultureInfo("sr-Latn-RS")),
                Status = AccountStatus.Suspended
            },
            new StudentListItem()
            {
                Initials = "TS",
                FullName = "Tamara Simić",
                Email = "tamara.simic@gmail.com",
                PhoneNumber = "069-845-1122",
                University = "Fakultet tehničkih nauka (FTN)",
                LessonCount = 21,
                ActiveProject = true,
                RegisteredAt = DateTime.Now.AddDays(-315).ToString("dd.MM.yyyy", new CultureInfo("sr-Latn-RS")),
                Status = AccountStatus.Deactivated
            },
            new StudentListItem()
            {
                Initials = "AV",
                FullName = "Aleksa Vuković",
                Email = "aleksa.vukovic@gmail.com",
                PhoneNumber = "060-928-3344",
                University = "Univerzitet Union Nikola Tesla",
                LessonCount = 4,
                ActiveProject = false,
                RegisteredAt = DateTime.Now.AddDays(-12).ToString("dd.MM.yyyy", new CultureInfo("sr-Latn-RS")),
                Status = AccountStatus.Active
            },
            new StudentListItem()
            {
                Initials = "IM",
                FullName = "Ivana Marković",
                Email = "ivana.markovic@gmail.com",
                PhoneNumber = "061-555-7812",
                University = "Visoka ICT škola",
                LessonCount = 13,
                ActiveProject = true,
                RegisteredAt = DateTime.Now.AddDays(-143).ToString("dd.MM.yyyy", new CultureInfo("sr-Latn-RS")),
                Status = AccountStatus.Active
            },
            new StudentListItem()
            {
                Initials = "SF",
                FullName = "Stefan Filipović",
                Email = "stefan.filipovic@gmail.com",
                PhoneNumber = "062-998-4411",
                University = "Prirodno-matematički fakultet (PMF)",
                LessonCount = 16,
                ActiveProject = false,
                RegisteredAt = DateTime.Now.AddDays(-201).ToString("dd.MM.yyyy", new CultureInfo("sr-Latn-RS")),
                Status = AccountStatus.Active
            },
        };

        return students;
    }

    private PagedResult<StudentListItem> CreatePagedStudents(int page)
    {
        var students = StudentsFactory();
        var paginatedStudents = students.Skip((page - 1) * PageSize).Take(PageSize).ToList();
        
        return new PagedResult<StudentListItem>(paginatedStudents, page, PageSize, students.Count);
    }
    
}