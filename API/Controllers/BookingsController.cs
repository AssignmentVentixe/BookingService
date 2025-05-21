using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers; 

[Route("api/[controller]")]
[ApiController]
public class BookingsController(IBookingService bookingService) : ControllerBase
{
    private readonly IBookingService _bookingService = bookingService;

    [HttpGet("user/{userEmail}")]
    public async Task<IActionResult> GetAllBookingsOnUser(string userEmail)
    {
        var bookings = await _bookingService.GetAllBookingsOnUserAsync(userEmail);

        return (bookings != null)
            ? Ok(bookings)
            : NotFound();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingById(string id)
    {
        var bookingModel = await _bookingService.GetByExpressionAsync(x => x.Id == id);

        return (bookingModel != null)
            ? Ok(bookingModel)
            : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Createbooking(BookingDto bookingDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdbooking = await _bookingService.CreatebookingAsync(bookingDto);

        return (createdbooking != null)
            ? Ok(createdbooking)
            : BadRequest("Failed to create booking");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletebooking(string id)
    {
        bool isDeleted = await _bookingService.DeleteBookingAsync(id);
        return (isDeleted)
            ? Ok()
            : BadRequest();
    }
}
