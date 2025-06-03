using API.Data.Entities;
using API.Models;

namespace API.Factories;

public class BookingConfirmationFactory
{
    public static BookingConfirmationEmailDto CreateDto(BookingEntity entity, BookingRegisterDto registrationForm) => new()
    {
        BookingEmail = registrationForm.BookingEmail,
        EventId = registrationForm.EventId,
        EventName = registrationForm.EventName,
        EventLocation = registrationForm.EventLocation,
        BookedDate = entity.BookedDate,
    };
}
