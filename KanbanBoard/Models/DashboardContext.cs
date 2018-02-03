using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Models
{
    public class DashboardContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<State> States { get; set; }
        public DashboardContext(DbContextOptions<DashboardContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
            //Database.EnsureCreated();
            States.Add(new State { Name = "ToDo", Priority = 0 });
            States.Add(new State { Name = "InProgress", Priority = 1 });
            States.Add(new State { Name = "Done", Priority = 2 });

            Cards.Add(new Card {Title = "BugFix", Description = "Fix som bug", StatePriority = 0});
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
