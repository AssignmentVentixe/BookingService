namespace API.Models;

public class BookingRegisterDto
{
    public string BookingEmail { get; set; } = null!;
    public string EventId { get; set; } = null!;
    public string EventName { get; set; } = null!;
    public string EventLocation { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public decimal EventPrice { get; set; }
}