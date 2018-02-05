using System.Linq;
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
            if (States.ToList().Any())
                return;

            States.Add(new State { Name = "ToDo" });
            States.Add(new State { Name = "InProgress" });
            States.Add(new State { Name = "Done" });

            SaveChanges();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=dashboarddb;Trusted_Connection=True;");
        }
    }
}
