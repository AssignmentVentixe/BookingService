using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using API.Data.Entities;
using API.Interfaces;
using API.Data.Contexts;

namespace API.Data.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly DataContext _context;
    private readonly DbSet<BookingEntity> _dbSet;
    private IDbContextTransaction? _transaction;

    public BookingRepository(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<BookingEntity>();
    }

    #region Transaction Management

    public virtual async Task BeginTransactionAsync()
    {
        _transaction ??= await _context.Database.BeginTransactionAsync();
    }

    public virtual async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }

    public virtual async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }

    #endregion

    #region CRUD

    public virtual async Task<BookingEntity> AddAsync(BookingEntity entity)
    {
        if (entity == null)
            throw new Exception("Entity cannot be null");

        try
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating entity :: {ex.Message}");
            return null!;
        }
    }

    public virtual async Task<bool> SaveAsync()
    {
        try
        {
            var result = await _context.SaveChangesAsync();

            if (result == 0)
                throw new Exception("Failed saving to database");
            else
                return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error saving entity :: {ex.Message}");
            return false;
        }
    }

    public async Task<IEnumerable<BookingEntity>> GetAllOnUserAsync(string userEmail)
    {
        try
        {
            return await _dbSet
                .Where(x => x.BookingEmail == userEmail)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving bookings: {ex.Message}");
            return [];
        }
    }

    public virtual async Task<BookingEntity> GetAsync(Expression<Func<BookingEntity, bool>> expression)
    {
        if (expression == null)
            throw new Exception("Expression cannot be null");

        try
        {
            return await _dbSet.FirstOrDefaultAsync(expression) ?? null!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving entity :: {ex.Message}");
            return null!;
        }
    }

    public virtual async Task<bool> DeleteAsync(Expression<Func<BookingEntity, bool>> expression)
    {
        if (expression == null)
            throw new Exception("Expression cannot be null");

        try
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(expression) ?? throw new Exception("Cannot find existing entity");
            _dbSet.Remove(existingEntity);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting entity :: {ex.Message}");
            return false;
        }
    }

    public virtual async Task<bool> AlreadyExistsAsync(Expression<Func<BookingEntity, bool>> expression)
    {
        if (expression == null)
            throw new Exception("Expression cannot be null");

        try
        {
            return await _dbSet.AnyAsync(expression);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error checking if entity exists :: {ex.Message}");
            return false;
        }
    }

    #endregion
}
