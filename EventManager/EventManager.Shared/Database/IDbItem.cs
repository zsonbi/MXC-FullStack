using EventManager.Shared.Dtos;

namespace EventManager.Shared.Database
{
    public interface IDbItem
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public BaseDto ToDto();
    }
}
