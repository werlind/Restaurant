using RestaurantReservation.Models;

namespace RestaurantReservation.Contracts
{
    public interface ITableRepository
    {
        public Task<IEnumerable<Table>> GetTables();
        public Task<Table> GetTable(int tableId);
        public Task<int> DeleteTable(int tableId);
        public Task<int> UpdateTable(int tableId, Table table);
        public Task<int> InsertTable(Table table);
    }
}
