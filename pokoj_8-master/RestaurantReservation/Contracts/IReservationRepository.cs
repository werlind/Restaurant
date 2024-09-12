using RestaurantReservation.Models;

namespace RestaurantReservation.Contracts
{
    public interface IReservationRepository
    {
        public Task<IEnumerable<Reservation>> GetReservations();
        public Task<Reservation> GetReservation(int reservationId);
        public Task<int> DeleteReservations(int reservationId);
        public Task<int> UpdateReservation(int reservationId, Reservation reservation);
        public Task<int> InsertReservation(Reservation reservation);
        public Task<IReadOnlyCollection<Reservation>> GetReservationByUserId(int userId);
    }
}
