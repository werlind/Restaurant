using Microsoft.Data.SqlClient;
using RestaurantReservation.Context;
using RestaurantReservation.Contracts;
using RestaurantReservation.Models;
using System.Data;
using Dapper;

namespace RestaurantReservation.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly DapperContext _context;

        public ReservationRepository(DapperContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Deletes a reservation of matching id from DB
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> DeleteReservations(int reservationId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", reservationId);
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync("dbo.DeleteReservation", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Returns a single reservation of matching id from DB
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        public async Task<Reservation> GetReservation(int reservationId)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", reservationId);
                Reservation output = await connection.QuerySingleAsync<Reservation>($"Select * from dbo.Reservations where Id = @Id", parameters);
                return output;
            }
        }
        public async Task<IReadOnlyCollection<Reservation>> GetReservationByUserId(int userId)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", userId);
                IEnumerable<Reservation> output = await connection.QueryAsync<Reservation>($"Select * from dbo.Reservations where UserId = @Id", parameters);
                return output.ToArray();
            }
        }
       
        /// <summary>
        /// Returns all reservations from DB
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Reservation>> GetReservations()
        {
            using (var connection = _context.CreateConnection())
            {
                var output = await connection.QueryAsync<Reservation>($"Select * from dbo.Reservations");
                return output.ToList();
            }
        }
        /// <summary>
        /// Inserts a new reservation into the DB
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> InsertReservation(Reservation reservation)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", reservation.UserId);
            parameters.Add("@TableId", reservation.TableId);
            parameters.Add("@ReservationDate", reservation.ReservationDate);
            parameters.Add("@NumberOfPeople", reservation.NumberOfPeople);
            parameters.Add("@Preferences", reservation.Preferences);
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync($"dbo.InsertReservation", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        /// <summary>
        /// Updates a reservation of matching id using another reservation object
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="reservation"></param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> UpdateReservation(int reservationId, Reservation reservation)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", reservationId);
            parameters.Add("@UserId", reservation.UserId);
            parameters.Add("@TableId", reservation.TableId);
            parameters.Add("@ReservationDate", reservation.ReservationDate);
            parameters.Add("@NumberOfPeople", reservation.NumberOfPeople);
            parameters.Add("@Preferences", reservation.Preferences);
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync($"dbo.UpdateReservation", parameters, commandType: CommandType.StoredProcedure);
            }     
        }
    }
}
