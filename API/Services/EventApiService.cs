using API.Interfaces;
using API.Models;

namespace API.Services;

public class EventApiService(IHttpClientFactory httpClientFactory) : IEventApiService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("EventService");

    public async Task<EventDto?> GetEventByIdAsync(string eventId)
    {
        if (string.IsNullOrWhiteSpace(eventId))
            return null;

        var response = await _httpClient.GetAsync($"api/events/{eventId}");
        if (!response.IsSuccessStatusCode)
            return null;

        var eventDto = await response.Content.ReadFromJsonAsync<EventDto>();
        return eventDto;
    }
}
