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
        public DashboardContext(DbContextOptions<DashboardContext> options)
            : base(options)
        {
        }
    }
}
