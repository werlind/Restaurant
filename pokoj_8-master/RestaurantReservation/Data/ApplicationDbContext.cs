using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Models;
using System.Collections.Generic;

namespace RestaurantReservation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Table> Table { get; set; }
        
    }
}
