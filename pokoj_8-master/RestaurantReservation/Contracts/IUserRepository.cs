using RestaurantReservation.Dto;
using RestaurantReservation.Models;

namespace RestaurantReservation.Contracts
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetUsers();
        public Task<User> GetUser(int userId);
        public Task<User> GetUser(string email);
        public Task<int> DeleteUser(int userId);
        public Task<int> UpdateUser(int userId, UserForUpdateDto user);
        public Task<User> CreateUser(UserForCreationDto user);
    }
}
