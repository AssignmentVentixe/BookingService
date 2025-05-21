using System.Linq.Expressions;
using API.Data.Entities;

namespace API.Interfaces
{
    public interface IBookingRepository
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<BookingEntity> AddAsync(BookingEntity entity);
        Task<bool> AlreadyExistsAsync(Expression<Func<BookingEntity, bool>> expression);
        Task<bool> DeleteAsync(Expression<Func<BookingEntity, bool>> expression);
        Task<IEnumerable<BookingEntity>> GetAllOnUserAsync(string userId);
        Task<BookingEntity> GetAsync(Expression<Func<BookingEntity, bool>> expression);
        Task<bool> SaveAsync();
    }
}