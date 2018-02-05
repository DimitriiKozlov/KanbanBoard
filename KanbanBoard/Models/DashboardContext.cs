using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Models
{
    public sealed class DashboardContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<State> States { get; set; }
        public DashboardContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=dashboarddb;Trusted_Connection=True;");
        }
    }
}
