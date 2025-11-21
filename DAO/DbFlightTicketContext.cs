using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;     // BẮT BUỘC
using DTO.Ticket;

namespace DAO
{
    public class DbFlightTicketContext : DbContext
    {
        public DbSet<TicketHistory> TicketsHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Server=localhost;Database=flightticketmanagement;User ID=root;Password=;";
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TicketHistory>()
                .HasNoKey()
                .ToTable("ticket_history"); // Tên bảng thật trong DB
        }
    }
}
