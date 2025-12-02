using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Shared.Database
{
    public interface IDeletable
    {
        DateTime? DeletedAtUtc { get; set; }
        [NotMapped]
        bool IsDeleted => DeletedAtUtc.HasValue;
    }
}
