using Microsoft.EntityFrameworkCore;
using GymManagement.Models;

namespace GymManagement.Data
{
    public class GymContext : DbContext
    {
        public DbSet<Member> Members { get; set; }

        // Kết nối với cơ sở dữ liệu SQLite
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=gymmanagement.db");
        }
    }
}