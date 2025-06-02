using API.Models;

namespace API.Interfaces;

public interface IEmailService
{
    Task SendBookingConfirmationAsync(BookingConfirmationEmailDto dto);
}