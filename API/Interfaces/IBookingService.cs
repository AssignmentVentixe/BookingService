using System.Linq.Expressions;
using API.Data.Entities;
using API.Models;

namespace API.Interfaces
{
    public interface IBookingService
    {
        Task<BookingEntity> CreatebookingAsync(BookingRegisterDto registrationForm);
        Task<bool> DeleteBookingAsync(string id);
        Task<IEnumerable<BookingWithEvent>> GetAllBookingsOnUserAsync(string userEmail);
        Task<Booking> GetByExpressionAsync(Expression<Func<BookingEntity, bool>> expression);
    }
}