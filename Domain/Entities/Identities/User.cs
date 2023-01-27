using Domain.Common;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identities
{
    public class User : IdentityUser<Guid>, IAuditableEntity
    {
        public Guid? InstanceId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Country { get; set; }
        public string LastName { get; set; }


        public string ProfileImage { get; set; }
        public string Password { get; set; }
        public string Status { get; set; } = EUserStatus.Pending.ToString();
        public bool Verified { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTimeOffset LastLogin { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}

