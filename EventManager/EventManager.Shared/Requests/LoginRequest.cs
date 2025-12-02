
using System.ComponentModel.DataAnnotations;

namespace EventManager.Shared.Requests
{
    public class LoginRequest
    {
        [MaxLength(100)]
        public required string UserName { get; set; }
        [MaxLength(80)]
        public required string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
