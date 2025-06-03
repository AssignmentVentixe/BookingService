using System.Linq.Expressions;
using API.Data.Entities;
using API.Models;

namespace API.Interfaces
{
    public interface IBookingService
    {
        Task<bool> CreatebookingAsync(BookingRegisterDto registrationForm);
        Task<bool> DeleteBookingAsync(string id);
        Task<IEnumerable<BookingWithEvent>> GetAllBookingsOnUserAsync(string userEmail);
        Task<BookingWithEvent> GetByExpressionAsync(Expression<Func<BookingEntity, bool>> expression);
    }
}