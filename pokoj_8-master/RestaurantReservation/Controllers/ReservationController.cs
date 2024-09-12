using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Data;
using RestaurantReservation.Models;
using System.Threading.Tasks;
using System.Linq;
using RestaurantReservation.Contracts;
using RestaurantReservation.Repository;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly IReservationRepository reservationRepository;

    public ReservationController(IReservationRepository reservationRepository) // zzmiast aplicatin db context trzeba miec IReservationRep tak jak jest w user cotroller
    {
        this.reservationRepository = reservationRepository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReservations(int id)
    {
        var reservation = await reservationRepository.GetReservation(id);
        return Ok(reservation);
    }

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetReservationsByUserId(int id)
    {
        var reservation = await reservationRepository.GetReservationByUserId(id);
        return Ok(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation(Reservation reservation)
    {
        await reservationRepository.InsertReservation(reservation);

        return Ok(reservation);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReservation(int id, Reservation reservation)
    {
        if (id != reservation.Id)
        {
            return BadRequest();
        }
        await reservationRepository.UpdateReservation(id, reservation);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
       var reservation = await reservationRepository.GetReservation(id);
       

        if (reservation == null)
        {
            return NotFound();
        }

        await reservationRepository.DeleteReservations(id);

        return NoContent();
    }
}
