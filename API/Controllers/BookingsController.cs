using System.Security.Claims;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers; 

[Route("api/[controller]")]
[ApiController]
public class BookingsController(IBookingService bookingService) : ControllerBase
{
    private readonly IBookingService _bookingService = bookingService;

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllBookingsOnUser()
    {
        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userEmail))
            return Unauthorized();

        var bookings = await _bookingService.GetAllBookingsOnUserAsync(userEmail);

        return (bookings != null)
            ? Ok(bookings)
            : NotFound();
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingById(string id)
    {
        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userEmail))
            return Unauthorized();

        var bookingModel = await _bookingService.GetByExpressionAsync(x => x.Id == id);

        return (bookingModel != null)
            ? Ok(bookingModel)
            : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Createbooking(BookingRegisterDto bookingDto)
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
