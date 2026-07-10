using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.Admins;
using CodeWithMixx.Domain.Entities.Classes;
using CodeWithMixx.Domain.Entities.Reservations;
using CodeWithMixx.Domain.Entities.Students;
using CodeWithMixx.Domain.Entities.Subjects;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Classes.Create;

public record CreateReservationDto
{
    public string AdminId { get; set; } = null!;
    public string StudentId { get; set; } = null!;
    public decimal? TotalPrice { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal PricePerClass { get; set; }
    public decimal AmountOfClasses { get; set; }
    public DateTime StartDate { get; set; }
    public string? Notes { get; set; }
}

public record CreateClassDto
{
    public Reservation Reservation { get; set; } = null!;
    public int SubjectId { get; set; }
    public decimal Price{ get; set; }
    public decimal AmountOfClasses { get; set; }
    public DateTime StartsAt { get; set; }
}

public class CreateReservationHandler(AppDbContext context) : IHandler
{
    public async Task<Result> HandleAsync(ClassReservationInput request, string adminId, CancellationToken ct)
    {
        var admin = await context.Admins.AnyAsync(a => a.AppUserId == adminId, ct);
        
        if(!admin)
            return Result.Failure(AdminError.NotFound(adminId));
        
        var student = await context.Students.AnyAsync(s => s.AppUserId == request.StudentId, ct);
        
        if (!student)
            return Result.Failure(StudentError.NotFound(request.StudentId));
        
        var subject = await context.Subjects.AnyAsync(s => s.Id == request.SubjectId, ct);
        
        if(!subject)
            return Result.Failure(SubjectError.NotFound(request.SubjectId));

        var createReservationDto = new CreateReservationDto
        {
            AdminId = adminId,
            StudentId = request.StudentId,
            AmountOfClasses = request.AmountOfClasses,
            PaidAmount = request.PaidAmount,
            PricePerClass = request.PricePerClass,
            StartDate = request.FirstClassStart,
            TotalPrice = request.TotalPrice,
            Notes = request.Notes
        };
        
        var reservation = CreateReservation(createReservationDto);
        
        context.Reservations.Add(reservation);
        
        Console.WriteLine("-------------------");
        Console.WriteLine("Reservation Details");
        Console.WriteLine("Admin: " + reservation.AdminId);
        Console.WriteLine("Student: " + reservation.StudentId);
        Console.WriteLine("Total Price: " + reservation.TotalPrice);
        Console.WriteLine("Paid Amount: " + reservation.PaidAmount);
        Console.WriteLine("Payment Status: " + reservation.PaymentStatus);
        Console.WriteLine("Discount rate: " + reservation.DiscountRate);
        Console.WriteLine("Bonus: " + reservation.Bonus);
        Console.WriteLine("Created at: " + reservation.CreatedAt);
        Console.WriteLine("-------------------");
        
        var createClassDto = new CreateClassDto
        {
            Reservation = reservation,
            SubjectId = request.SubjectId,
            Price = request.PricePerClass,
            AmountOfClasses = request.AmountOfClasses,
            StartsAt = request.FirstClassStart
        };
        
        var classes = CreateClasses(createClassDto);
        context.Classes.AddRange(classes);
        await context.SaveChangesAsync(ct);
        return Result.Success();
    }

    private Reservation CreateReservation(CreateReservationDto dto)
    {
        var reservation = new Reservation
        {
            AdminId = dto.AdminId,
            StudentId = dto.StudentId,
            ServiceType = ServiceType.Class,
            TotalPrice = CalculateTotalPriceBasedOnRequest(dto.TotalPrice, dto.PricePerClass, dto.AmountOfClasses),
            PaidAmount = dto.PaidAmount,
            Notes = dto.Notes
        };
        
        reservation.DiscountRate = CalculateDiscountRate(reservation.TotalPrice, dto.PricePerClass, dto.AmountOfClasses);
        reservation.Bonus = CalculateBonus(reservation.TotalPrice, reservation.PaidAmount);
        reservation.PaymentStatus = GetPaymentStatus(reservation.TotalPrice, reservation.PaidAmount, dto.StartDate);
        
        return reservation;
    }
    
    private decimal CalculateTotalPriceBasedOnRequest(decimal? requestedTotalPrice, decimal pricePerClass, decimal amountOfClasses)
    {
        if (requestedTotalPrice is not null)
            return requestedTotalPrice.Value;

        return pricePerClass * amountOfClasses;
    }

    private decimal CalculateDiscountRate(decimal requestedTotalPrice, decimal pricePerClass, decimal amountOfClasses)
    {
        var defaultTotal = amountOfClasses * pricePerClass;
        if (defaultTotal <= 0 || defaultTotal < requestedTotalPrice)
            return 0;
        
        var discountRate = (defaultTotal - requestedTotalPrice) / defaultTotal * 100;
        return Math.Round(discountRate, 2);
    }

    private decimal CalculateBonus(decimal totalPrice, decimal paidAmount)
    {
        if (totalPrice >= paidAmount)
            return 0;

        var bonus = paidAmount - totalPrice;
        
        return Math.Round(bonus, 2);
    }

    private PaymentStatus GetPaymentStatus(decimal totalPrice, decimal paidAmount, DateTime classDate)
    {
        return totalPrice switch
        {
            var price when price <= paidAmount => PaymentStatus.Paid,
            var price when price > paidAmount && DateTime.UtcNow.Date > classDate.Date => PaymentStatus.Overdue,
            var price when paidAmount > 0 && paidAmount < price => PaymentStatus.PartiallyPaid,
            _ => PaymentStatus.Pending
        };
    }

    private IReadOnlyList<Class> CreateClasses(CreateClassDto dto)
    {
        var currentStartTime = dto.StartsAt;
        var classes = new List<Class>();

        TimeZoneInfo belgradeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");

        DateTime belgradeTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, belgradeZone);

        var isSubsequent = dto.StartsAt.Date < belgradeTime.Date;

        var ceiledClassAmount = (int)Math.Ceiling(dto.AmountOfClasses);

        for (int i = 1; i <= ceiledClassAmount; i++)
        {
            var newClass = new Class
            {
                StartsAt = DateTime.SpecifyKind(currentStartTime, DateTimeKind.Utc),
                Reservation = dto.Reservation,
                ClassStatus = isSubsequent ? ClassStatus.Completed : ClassStatus.Scheduled,
                SubjectId = dto.SubjectId,
                Price = dto.Price
            };
            if (i == ceiledClassAmount)
            {
                newClass.EndsAt = GetLastClassEndTime(dto.AmountOfClasses, currentStartTime);
            }
            else
            {
                currentStartTime = currentStartTime.AddMinutes(45);
                newClass.EndsAt = DateTime.SpecifyKind(currentStartTime, DateTimeKind.Utc);
            }
            classes.Add(newClass);
        }

        foreach(var reservedClass in classes)
        {
            Console.WriteLine("-------------------");
            Console.WriteLine("Subject id: " + reservedClass.SubjectId);
            Console.WriteLine("Starts at: " + reservedClass.StartsAt);
            Console.WriteLine("Ends at: " + reservedClass.EndsAt);
            Console.WriteLine("------------------");
        }

        return classes;
    }

    private DateTime GetLastClassEndTime(decimal classAmount, DateTime previousClassEndTime)
    {
        var timeModulus = classAmount - Math.Floor(classAmount);

        if (timeModulus is 0)
            return DateTime.SpecifyKind(previousClassEndTime.AddMinutes(45), DateTimeKind.Utc);

        var minutesToAdd = timeModulus * 45;
        var roundedMinutes = Math.Round(minutesToAdd, 0);
        
        return DateTime.SpecifyKind(previousClassEndTime.AddMinutes((double)roundedMinutes), DateTimeKind.Utc);
    }
   
}