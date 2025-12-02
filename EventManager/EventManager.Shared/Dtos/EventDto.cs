using EventManager.Shared.Database;
using System.ComponentModel.DataAnnotations;


namespace EventManager.Shared.Dtos
{
    public class EventDto : BaseDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [MaxLength(100)]
        [Required]
        public string Location { get; set; } = string.Empty;
        public string? Country { get; set; }

        [Range(0,999999999)]
        public int? Capacity { get; set; } 

        public EventDto(Event eventObject) : base(eventObject)
        {
            this.Name = eventObject.Name;
            this.Location = eventObject.Location;
            this.Country = eventObject.Country;
            this.Capacity = eventObject.Capacity;
        }

        public EventDto() { }
    }
}
