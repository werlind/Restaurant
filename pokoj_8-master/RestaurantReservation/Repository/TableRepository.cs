using Dapper;
using Microsoft.Data.SqlClient;
using RestaurantReservation.Context;
using RestaurantReservation.Contracts;
using RestaurantReservation.Models;
using System.Data;

namespace RestaurantReservation.Repository
{
    public class TableRepository : ITableRepository
    {
        private readonly DapperContext _context;

        public TableRepository(DapperContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Deletes a table from DB
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> DeleteTable(int tableId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", tableId);
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync($"dbo.DeleteTable", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        /// <summary>
        /// Returns a single table of matching tableId from DB
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public async Task<Table> GetTable(int tableId)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", tableId);
                var output = await connection.QuerySingleAsync<Table>($"Select * from dbo.Tables where Id = @Id", parameters);
                return output;
            }
        }
        /// <summary>
        /// Returns all tables from DB
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Table>> GetTables()
        {
            using (var connection = _context.CreateConnection())
            {
                var output = await connection.QueryAsync<Table>($"Select * from dbo.Tables");
                return output.ToList();
            }
        }
        /// <summary>
        /// Inserts a table into DB
        /// </summary>
        /// <param name="table"></param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> InsertTable(Table table)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Capacity", table.Capacity);
            parameters.Add("@IsAvailable", table.IsAvailable);
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync($"dbo.InsertTable", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        /// <summary>
        /// Updates a table of matching tableId with another table object
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="table"></param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> UpdateTable(int tableId, Table table)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", tableId);
            parameters.Add("@Capacity", table.Capacity);
            parameters.Add("@IsAvailable", table.IsAvailable);
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync($"dbo.UpdateTable", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
