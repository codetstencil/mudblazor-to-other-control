using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class ApplicationUser : IdentityUser<string>
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
