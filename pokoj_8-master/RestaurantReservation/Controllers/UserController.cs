using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Data;
using RestaurantReservation.Models;
using System.Threading.Tasks;
using System.Linq;
using BCrypt.Net;
using RestaurantReservation.Context;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using RestaurantReservation.Repository;
using RestaurantReservation.Dto;
using RestaurantReservation.Contracts;

namespace RestaurantReservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository repo)
        {
            _userRepository = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForCreationDto user)
        {     
            try
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // Szyfrowanie hasła
                var createdUser = await _userRepository.CreateUser(user);
                //return CreatedAtRoute("register", new { id = createdUser.Id }, createdUser);
                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userRepository.GetUser(model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return Unauthorized();
            }
            return Ok(user);
           
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, UserForUpdateDto user)
        {
            try
            {
                var dbUser = await _userRepository.GetUser(userId);
                if (dbUser == null)
                    return NotFound();
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                await _userRepository.UpdateUser(userId, user);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var dbUser = await _userRepository.GetUser(userId);
                if (dbUser == null)
                    return NotFound();
                await _userRepository.DeleteUser(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
