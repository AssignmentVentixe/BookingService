using System.Linq.Expressions;
using API.Data.Entities;
using API.Extensions;
using API.Interfaces;
using API.Models;

namespace API.Services;

public class BookingService(IBookingRepository bookingRepository) : IBookingService
{
    private readonly IBookingRepository _bookingRepository = bookingRepository;

    public async Task<BookingEntity> CreatebookingAsync(BookingDto registrationForm)
    {
        if (registrationForm == null)
            return null!;

        await _bookingRepository.BeginTransactionAsync();
        var entity = registrationForm.MapTo<BookingEntity>();
        await _bookingRepository.AddAsync(entity);
        var saved = await _bookingRepository.SaveAsync();
        if (!saved)
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
        var saved = await _bookingRepository.SaveAsync();
        if (!saved)
        {
            await _bookingRepository.RollbackTransactionAsync();
            return false;
        }

        await _bookingRepository.CommitTransactionAsync();
        return true;
    }

    public async Task<IEnumerable<Booking>> GetAllBookingsOnUserAsync(string userEmail)
    {
        var entities = await _bookingRepository.GetAllOnUserAsync(userEmail);
        if (entities == null)
            return null!;

        var bookingModels = entities.Select(x => x.MapTo<Booking>()).ToList();
        return bookingModels;
    }

    public async Task<Booking> GetByExpressionAsync(Expression<Func<BookingEntity, bool>> expression)
    {
        var entity = await _bookingRepository.GetAsync(expression);

        var bookingModel = entity.MapTo<Booking>();

        return bookingModel;
    }

}
