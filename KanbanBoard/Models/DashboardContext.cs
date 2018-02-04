using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Models
{
    public class DashboardContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<State> States { get; set; }
        public DashboardContext(DbContextOptions<DashboardContext> options) : base(options)
        {
            Database.EnsureDeleted();
            //Database.EnsureCreated();
            //States.Add(new State { Name = "ToDo" });
            //States.Add(new State { Name = "InProgress" });
            //States.Add(new State { Name = "Done" });

            //Cards.Add(new Card { Title = "BugFix", Description = "Fix som bug", StatePriority = 0 });

            //SaveChanges();

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=dashboarddb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Card>().Property(c => c.State).HasDefaultValue(States.Find(0));
        }
    }
}
