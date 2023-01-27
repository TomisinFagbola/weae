using Domain.Entities;
using Domain.Entities.Identities;
using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects
{
    public record TokenDto(string AccessToken, string RefreshToken, DateTime ExpiresIn);

    public record UserCreateDTO
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
       
        
    }

    public record UserUpdateDTO : UserCreateDTO
    {
    
    }
    public record UserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
    public record RefreshTokenDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
    public record ResetPasswordDTO
    {
        public string Email { get; set; }

    }
    public record SetPasswordDTO
    {
        public string Password { get; set; }
        public string Token { get; set; }
    }
    public record AuthDto
    {
        public string AccessToken { get; set; }
        public DateTime? ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
    }

    public record UserDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public bool Verified { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}