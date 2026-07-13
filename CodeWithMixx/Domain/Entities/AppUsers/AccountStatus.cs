using System.ComponentModel.DataAnnotations;

namespace CodeWithMixx.Domain.Entities.AppUsers;

public enum AccountStatus
{
    [Display(Name = "Aktivan")]
    Active,
    [Display(Name = "Suspendovan")]
    Suspended,
    [Display(Name = "Deaktiviran")]
    Deactivated
}