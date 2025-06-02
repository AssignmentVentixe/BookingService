using API.Interfaces;
using API.Models;

namespace API.Services;

public class EmailService(IHttpClientFactory httpClientFactory) : IEmailService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task SendBookingConfirmationAsync(BookingConfirmationEmailDto dto)
    {
        var client = _httpClientFactory.CreateClient("EmailVerificationProvider");

        var response = await client.PostAsJsonAsync("api/bookingconfirmation/send", dto);

        response.EnsureSuccessStatusCode();
    }
}
