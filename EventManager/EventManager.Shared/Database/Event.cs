using EventManager.Shared.Dtos;


namespace EventManager.Shared.Database
{
    public class Event : BaseDbItem
    {
        public required string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int Capacity { get; set; }

        public override EventDto ToDto()
        {
            return new EventDto(this);
        }
    }
}
