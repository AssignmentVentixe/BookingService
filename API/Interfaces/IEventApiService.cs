using API.Models;

namespace API.Interfaces;

public interface IEventApiService
{
    Task<EventDto?> GetEventByIdAsync(string eventId);
}