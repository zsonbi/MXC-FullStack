
using System.ComponentModel.DataAnnotations;

namespace EventManager.Shared.Requests
{
    public class RegisterRequest
    {
        [MaxLength(100)]
        public required string Username { get; set; }
        [MaxLength(120)]
        public required string Email { get; set; }
        [MaxLength(80)]
        public required string Password { get; set; }
        [MaxLength(80)]
        public required string ConfirmPassword { get; set; }

    }
}
