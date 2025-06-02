using System.ComponentModel.DataAnnotations;

namespace API.Data.Entities;

public class BookingEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string BookingEmail { get; set; } = null!;
    public string EventId { get; set; } = null!;
    public DateTime BookedDate { get; set; } = DateTime.UtcNow;
}