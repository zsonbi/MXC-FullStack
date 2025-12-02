using EventManager.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace EventManager.Shared.Database
{
    public abstract class BaseDbItem : IDbItem
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public abstract BaseDto ToDto();
    }
}
