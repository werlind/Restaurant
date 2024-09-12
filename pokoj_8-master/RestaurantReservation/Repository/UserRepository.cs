using Dapper;
using Microsoft.Data.SqlClient;
using RestaurantReservation.Context;
using RestaurantReservation.Contracts;
using RestaurantReservation.Dto;
using RestaurantReservation.Models;
using System.Data;

namespace RestaurantReservation.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Deletes a user from DB
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> DeleteUser(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", userId);
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync($"dbo.DeleteUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        /// <summary>
        /// Returns a single user from DB.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>User of matching userId</returns>
        public async Task<User> GetUser(int userId)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", userId);
                var output = await connection.QuerySingleAsync<User>($"Select * from dbo.Users where Id = @Id", parameters);
                return output;
            }
        }
        /// <summary>
        /// Gets all users from db
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetUsers()
        {
            using (var connection = _context.CreateConnection())
            {
                var output =  await connection.QueryAsync<User>($"Select * from dbo.Users");
                return output.ToList();
            }
        }
        /// <summary>
        /// Inserts a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Number of rows affected</returns>
        public async Task<User> CreateUser(UserForCreationDto user)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", user.FirstName);
            parameters.Add("@LastName", user.LastName);
            parameters.Add("@Email", user.Email);
            parameters.Add("@Password", user.Password);
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>("dbo.InsertUser", parameters, commandType: CommandType.StoredProcedure);

                var createdUser = new User
                {
                    Id = id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password
                };
                return createdUser;
            }
        }
        /// <summary>
        /// Updates a user of specified id using another user object
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> UpdateUser(int userId, UserForUpdateDto user)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", userId);
            parameters.Add("@FirstName", user.FirstName);
            parameters.Add("@LastName", user.LastName);
            parameters.Add("@Email", user.Email);
            parameters.Add("@Password", user.Password);
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync($"dbo.UpdateUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<User> GetUser(string email)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Email", email);
                var output = await connection.QuerySingleAsync<User>($"Select * from dbo.Users where Email = @Email", parameters);
                return output;
            }
        }
    }
}
