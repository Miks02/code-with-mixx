using System.Globalization;
using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.Classes;
using CodeWithMixx.Domain.Entities.Reservations;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Classes;

public class GetClassesPageHandler(AppDbContext context) : IHandler
{
    public async Task<ClassPageViewModel> HandleAsync(string filter, string sort, int page, int pageSize, CancellationToken ct)
    {
        var classes = BuildMockClasses();

        var subjects = await context.Subjects
            .Select(s => new ClassPageViewModel.SubjectDropdownItem()
            {
                Id = s.Id,
                SubjectName = s.Name
            })  
            .ToListAsync(ct);


        var pagedResult = new PagedResult<ClassReservationListItem>(classes, page, pageSize, classes.Count);


        return new ClassPageViewModel
        {
            TotalClasses = classes.Count,
            UpcomingClasses = classes.Count(c => c.ClassStatus == ClassStatus.Scheduled),
            FinishedClasses = classes.Count(c => c.ClassStatus == ClassStatus.Completed),
            CancelledClasses = classes.Count(c => c.ClassStatus == ClassStatus.Cancelled),
            Subjects = subjects,
            Classes = pagedResult
        };
    }

    private static List<ClassReservationListItem> BuildMockClasses()
    {
        var culture = new CultureInfo("sr-Latn-RS");

        return
        [
            new ClassReservationListItem
            {
                Id = 1,
                SubjectName = "Web programiranje",
                StudentName = "John Doe",
                StartsAt = new DateTime(2024, 6, 01, 10, 00, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                EndsAt = new DateTime(2024, 6, 01, 12, 30, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                AmountOfClasses = 4,
                TotalPrice = 5200,
                PaymentStatus = PaymentStatus.Overdue,
                ClassStatus = ClassStatus.Ongoing
            },
            new ClassReservationListItem
            {
                Id = 2,
                SubjectName = "Programerski alati",
                StudentName = "Jane Smith",
                StartsAt = new DateTime(2024, 6, 02, 14, 00, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                EndsAt = new DateTime(2024, 6, 02, 14, 45, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                AmountOfClasses = 1,
                TotalPrice = 1300,
                PaymentStatus = PaymentStatus.Pending,
                ClassStatus = ClassStatus.Scheduled
            },
            new ClassReservationListItem
            {
                Id = 3,
                SubjectName = "Praktikum primenjenog programiranja",
                StudentName = "Milan Jovanovic",
                StartsAt = new DateTime(2024, 6, 03, 09, 30, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                EndsAt = new DateTime(2024, 6, 03, 11, 00, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                AmountOfClasses = 2,
                TotalPrice = 2600,
                PaymentStatus = PaymentStatus.Paid,
                ClassStatus = ClassStatus.Completed
            },
            new ClassReservationListItem
            {
                Id = 4,
                SubjectName = "Baza podataka",
                StudentName = "Ana Markovic",
                StartsAt = new DateTime(2024, 6, 04, 16, 00, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                EndsAt = new DateTime(2024, 6, 04, 17, 30, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                AmountOfClasses = 3,
                TotalPrice = 3900,
                PaymentStatus = PaymentStatus.Overdue,
                ClassStatus = ClassStatus.Ongoing
            },
            new ClassReservationListItem
            {
                Id = 5,
                SubjectName = "Osnove C programiranja",
                StudentName = "Stefan Petrovic",
                StartsAt = new DateTime(2024, 6, 05, 12, 00, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                EndsAt = new DateTime(2024, 6, 05, 13, 15, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                AmountOfClasses = 1,
                TotalPrice = 1500,
                PaymentStatus = PaymentStatus.Pending,
                ClassStatus = ClassStatus.Scheduled
            },
            new ClassReservationListItem
            {
                Id = 6,
                SubjectName = "Objektno orijentisano programiranje",
                StudentName = "Petar Nikolic",
                StartsAt = new DateTime(2024, 6, 06, 18, 00, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                EndsAt = new DateTime(2024, 6, 06, 19, 30, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                AmountOfClasses = 2,
                TotalPrice = 2800,
                PaymentStatus = PaymentStatus.Paid,
                ClassStatus = ClassStatus.Completed
            },
            new ClassReservationListItem
            {
                Id = 7,
                SubjectName = "Internet programerski alati",
                StudentName = "Nikola Matic",
                StartsAt = new DateTime(2024, 6, 07, 11, 00, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                EndsAt = new DateTime(2024, 6, 07, 12, 30, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                AmountOfClasses = 2,
                TotalPrice = 3200,
                PaymentStatus = PaymentStatus.Overdue,
                ClassStatus = ClassStatus.Cancelled
            },
            new ClassReservationListItem
            {
                Id = 8,
                SubjectName = "Programerski alati",
                StudentName = "Jelena Simic",
                StartsAt = new DateTime(2024, 6, 08, 15, 30, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                EndsAt = new DateTime(2024, 6, 08, 17, 00, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                AmountOfClasses = 4,
                TotalPrice = 6000,
                PaymentStatus = PaymentStatus.Paid,
                ClassStatus = ClassStatus.Ongoing
            },
            new ClassReservationListItem
            {
                Id = 9,
                SubjectName = "Praktikum primenjenog programiranja",
                StudentName = "Milica Vasic",
                StartsAt = new DateTime(2024, 6, 09, 10, 15, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                EndsAt = new DateTime(2024, 6, 09, 11, 45, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                AmountOfClasses = 1,
                TotalPrice = 1400,
                PaymentStatus = PaymentStatus.Pending,
                ClassStatus = ClassStatus.Scheduled
            },
            new ClassReservationListItem
            {
                Id = 10,
                SubjectName = "Web programiranje",
                StudentName = "Luka Kovacevic",
                StartsAt = new DateTime(2024, 6, 10, 13, 00, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                EndsAt = new DateTime(2024, 6, 10, 14, 30, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                AmountOfClasses = 3,
                TotalPrice = 4500,
                PaymentStatus = PaymentStatus.Paid,
                ClassStatus = ClassStatus.Completed
            },
            new ClassReservationListItem
            {
                Id = 11,
                SubjectName = "Osnove C programiranja",
                StudentName = "Sofija Ilic",
                StartsAt = new DateTime(2024, 6, 11, 09, 00, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                EndsAt = new DateTime(2024, 6, 11, 10, 00, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                AmountOfClasses = 1,
                TotalPrice = 1200,
                PaymentStatus = PaymentStatus.Pending,
                ClassStatus = ClassStatus.Scheduled
            },
            new ClassReservationListItem
            {
                Id = 12,
                SubjectName = "Objektno orijentisano programiranje",
                StudentName = "Nenad Milic",
                StartsAt = new DateTime(2024, 6, 12, 17, 00, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                EndsAt = new DateTime(2024, 6, 12, 18, 30, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                AmountOfClasses = 2,
                TotalPrice = 2600,
                PaymentStatus = PaymentStatus.Overdue,
                ClassStatus = ClassStatus.Ongoing
            },
            new ClassReservationListItem
            {
                Id = 13,
                SubjectName = "Internet programerski alati",
                StudentName = "Kristina Ristic",
                StartsAt = new DateTime(2024, 6, 13, 08, 30, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                EndsAt = new DateTime(2024, 6, 13, 10, 00, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                AmountOfClasses = 5,
                TotalPrice = 7500,
                PaymentStatus = PaymentStatus.Paid,
                ClassStatus = ClassStatus.Completed
            },
            new ClassReservationListItem
            {
                Id = 14,
                SubjectName = "Baza podataka",
                StudentName = "Marko Savic",
                StartsAt = new DateTime(2024, 6, 14, 19, 00, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                EndsAt = new DateTime(2024, 6, 14, 20, 00, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                AmountOfClasses = 2,
                TotalPrice = 3000,
                PaymentStatus = PaymentStatus.Pending,
                ClassStatus = ClassStatus.Cancelled
            },
            new ClassReservationListItem
            {
                Id = 15,
                SubjectName = "Programerski alati",
                StudentName = "Teodora Pavlovic",
                StartsAt = new DateTime(2024, 6, 15, 14, 30, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                EndsAt = new DateTime(2024, 6, 15, 16, 00, 00).ToString("yy.MM.dd HH:mm:ss", culture),
                AmountOfClasses = 3,
                TotalPrice = 4200,
                PaymentStatus = PaymentStatus.Overdue,
                ClassStatus = ClassStatus.Scheduled
            }
        ];
    }
}