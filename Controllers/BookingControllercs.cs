using _27_victoria_esdproj.Data;
using _27_victoria_esdproj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using victoria_esdproj.Models;

namespace victoria_esdproj.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = $"{UserRoles.Admin}")]
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var bookings = _context.Bookings.ToList();

            if (bookings == null || !bookings.Any())
            {
                return Problem("No booking data available.", statusCode: 500);
            }

            return Ok(bookings);
        }


        [HttpGet("GetById")]
        public IActionResult GetById(int? id)
        {
            var bookings = _context.Bookings.FirstOrDefault(b => b.BookingId == id);
            if (bookings == null)
                return Problem(detail: "Bookings with Id" + id + "is not found.", statusCode: 404);

            return Ok(bookings);
        }


        [Authorize] // This attribute ensures that the user is authenticated
        [HttpPost("Create")]
        public IActionResult Post([FromBody] Booking booking)
        {
            if (booking == null)
            {
                return BadRequest("Invalid booking data.");
            }

            try
            {
                // Assuming _context is the database context for accessing the Bookings table
                _context.Bookings.Add(booking);
                _context.SaveChanges();

                // Return a response with the booking details and status 201 Created
                return CreatedAtAction(nameof(GetAll), new { id = booking.BookingId }, booking);
            }
            catch (Exception ex)
            {
                // Log the error and return a generic error message
                // You can log the exception using a logging framework like Serilog, NLog, etc.
                return StatusCode(500, "An error occurred while creating the booking.");
            }
        }

        [HttpPut("Put/{id}")]
        public IActionResult Put(int id, [FromBody] Booking booking)
        {
            if (booking == null)
            {
                return BadRequest("Booking data is required.");
            }

            try
            {
                var entity = _context.Bookings.FirstOrDefault(b => b.BookingId == id);
                if (entity == null)
                {
                    return NotFound($"Booking with id {id} not found.");
                }

                // Update the booking details
                entity.FacilityDescription = booking.FacilityDescription;
                entity.BookingDateFrom = booking.BookingDateFrom;
                entity.BookingDateTo = booking.BookingDateTo;
                entity.BookedBy = booking.BookedBy;
                entity.BookingStatus = booking.BookingStatus;

                _context.SaveChanges();

                return Ok(entity);
            }
            catch (Exception ex)
            {
                // Log the exception details
                // Example: _logger.LogError(ex, "Error occurred while updating booking.");
                return StatusCode(500, "An error occurred while updating the booking. Please try again later.");
            }
        }



        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id) // Changed from int? Bookingid to int id
        {
            var entity = _context.Bookings.FirstOrDefault(b => b.BookingId == id);
            if (entity == null)
                return Problem(detail: "Booking with id " + id + " is not found.", statusCode: 404);

            _context.Bookings.Remove(entity);
            _context.SaveChanges();

            return Ok(entity);
        }

    }
}
