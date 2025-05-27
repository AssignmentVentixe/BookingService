using System.Diagnostics;
using System.Linq.Expressions;
using API.Data.Entities;
using API.Extensions;
using API.Interfaces;
using API.Models;

namespace API.Services;

public class BookingService(IBookingRepository bookingRepository, IHttpClientFactory httpClientFactory) : IBookingService
{
    private readonly IBookingRepository _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("EventService");

    public async Task<IEnumerable<BookingWithEvent>> GetAllBookingsOnUserAsync(string userEmail)
    {
        var bookings = await _bookingRepository.GetAllOnUserAsync(userEmail) ?? [];

        var result = new List<BookingWithEvent>();

        foreach (var booking in bookings)
        {
            var response = await _httpClient.GetAsync($"api/events/{booking.EventId}");
            if (!response.IsSuccessStatusCode)
                continue;

            var eventDto = await response.Content.ReadFromJsonAsync<EventDto>();
            if (eventDto == null)
                continue;

            var mapped = booking.MapTo<BookingWithEvent>();
            mapped.Event = eventDto;
            result.Add(mapped);
        }

        return result;
    }

    public async Task<BookingEntity> CreatebookingAsync(BookingRegisterDto registrationForm)
    {
        ArgumentNullException.ThrowIfNull(registrationForm);

        await _bookingRepository.BeginTransactionAsync();
        var entity = registrationForm.MapTo<BookingEntity>();
        await _bookingRepository.AddAsync(entity);

        if (!await _bookingRepository.SaveAsync())
        {
            await _bookingRepository.RollbackTransactionAsync();
            return null!;
        }

        await _bookingRepository.CommitTransactionAsync();
        return entity;
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

    public async Task<Booking> GetByExpressionAsync(Expression<Func<BookingEntity, bool>> expression)
    {
        var entity = await _bookingRepository.GetAsync(expression);
        return entity.MapTo<Booking>();
    }
}
