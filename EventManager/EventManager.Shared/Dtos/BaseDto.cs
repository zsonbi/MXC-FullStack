using EventManager.Shared.Database;

namespace EventManager.Shared.Dtos
{
    public class BaseDto 
    {
        public Guid Id { get; set; } = Guid.Empty;
        public DateTime CreatedAt { get; set; }
        public BaseDto(IDbItem dbItem)
        {
           this.Id = dbItem.Id;
            this.CreatedAt = dbItem.CreatedAt;
        }
        public BaseDto()
        {
            this.Id = Guid.Empty;
        }
    }
}
