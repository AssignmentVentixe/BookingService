using System.Linq.Expressions;
using API.Data.Entities;
using API.Models;

namespace API.Interfaces
{
    public interface IBookingService
    {
        Task<BookingEntity> CreatebookingAsync(BookingDto registrationForm);
        Task<bool> DeleteBookingAsync(string id);
        Task<IEnumerable<Booking>> GetAllBookingsOnUserAsync(string userId);
        Task<Booking> GetByExpressionAsync(Expression<Func<BookingEntity, bool>> expression);
    }
}