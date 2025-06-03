using System.Diagnostics;
using System.Linq.Expressions;
using API.Data.Entities;
using API.Extensions;
using API.Factories;
using API.Interfaces;
using API.Models;

namespace API.Services;

public class BookingService(IBookingRepository bookingRepository, IEventApiService eventService, IEmailService emailService) : IBookingService
{
    private readonly IBookingRepository _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
    private readonly IEventApiService _eventService = eventService;
    private readonly IEmailService _emailService = emailService;

    public async Task<IEnumerable<BookingWithEvent>> GetAllBookingsOnUserAsync(string userEmail)
    {
        var bookings = await _bookingRepository.GetAllOnUserAsync(userEmail) ?? [];

        var result = new List<BookingWithEvent>();

        foreach (var booking in bookings)
        {
            var eventDto = await _eventService.GetEventByIdAsync(booking.EventId);
            if (eventDto == null)
                continue;

            var mapped = booking.MapTo<BookingWithEvent>();
            mapped.Event = eventDto;
            result.Add(mapped);
        }

        return result;
    }

    public async Task<bool> CreatebookingAsync(BookingRegisterDto registrationForm)
    {
        ArgumentNullException.ThrowIfNull(registrationForm);

        await _bookingRepository.BeginTransactionAsync();
        var entityToCreate = registrationForm.MapTo<BookingEntity>();
        var entity = await _bookingRepository.AddAsync(entityToCreate);

        if (!await _bookingRepository.SaveAsync())
        {
            await _bookingRepository.RollbackTransactionAsync();
            return false;
        }

        await _bookingRepository.CommitTransactionAsync();

        try
        {
            BookingConfirmationEmailDto dto = BookingConfirmationFactory.CreateDto(entity, registrationForm);

            await _emailService.SendBookingConfirmationAsync(dto);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to send booking confirmation email: {ex.Message}");
        }

        return true;
    }

    public async Task<bool> DeleteBookingAsync(string id)
    {
        var entity = await _bookingRepository.GetAsync(x => x.Id == id);
        if (entity == null)
            return false;

        await _bookingRepository.BeginTransactionAsync();
        await _bookingRepository.DeleteAsync(x => x.Id == id);

        if (!await _bookingRepository.SaveAsync())
        {
            await _bookingRepository.RollbackTransactionAsync();
            return false;
        }

        await _bookingRepository.CommitTransactionAsync();
        return true;
    }

    public async Task<BookingWithEvent> GetByExpressionAsync(Expression<Func<BookingEntity, bool>> expression)
    {
        var entity = await _bookingRepository.GetAsync(expression);
        return entity.MapTo<BookingWithEvent>();
    }
}
