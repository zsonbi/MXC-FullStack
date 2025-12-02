using EventManager.Shared.Database;
using EventManager.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EventManager.Database
{
    public class User : IdentityUser<Guid>, IDbItem
    {
        [Key]
        public override Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserDto ToDto()
        {
            return new Shared.Dtos.UserDto(this)
            {
                Username = this.UserName!,
                Email = this.Email!,
                
            };
        }

        BaseDto IDbItem.ToDto()
        {
            return ToDto();
        }
    }
}
