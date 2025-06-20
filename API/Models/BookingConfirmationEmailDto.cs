﻿namespace API.Models;

public class BookingConfirmationEmailDto
{
    public string BookingEmail { get; set; } = null!;
    public string EventId { get; set; } = null!;
    public string EventName { get; set; } = null!;
    public string EventLocation { get; set; } = null!;
    public string EventDate { get; set; } = null!;
    public DateTime BookedDate { get; set; }
}
