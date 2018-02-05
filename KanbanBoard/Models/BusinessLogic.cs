using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Models
{
    public class BusinessLogic
    {
        public IEnumerable<Card> GetAllCards()
        {
            using (var context = new DashboardContext(new DbContextOptions<DashboardContext>()))
            {
                var dashboardContext = context.Cards.Include(c => c.State);
                return dashboardContext.ToList();
            }
        }

        public void AddCard(Card card)
        {
            using (var context = new DashboardContext(new DbContextOptions<DashboardContext>()))
            {
                card.Id = Guid.NewGuid();
                card.State = context.States.FirstOrDefault(s => s.Name == "ToDo");
                context.Add(card);
                context.SaveChanges();
            }
        }

        public Card UpdateCard(Card uCard)
        {
            using (var context = new DashboardContext(new DbContextOptions<DashboardContext>()))
            {
                var card = context.Cards.FirstOrDefault(c => c.Id == uCard.Id);
                if (card == null)
                    return null;

                card.Title = uCard.Title;
                card.Description = uCard.Description;
                context.SaveChanges();

                var dashboardContext = context.Cards.Include(c => c.State);
                return dashboardContext.FirstOrDefault(c => c.Id == uCard.Id);
            }
        }

        public Card ChangeStatusCard(Guid id)
        {
            using (var context = new DashboardContext(new DbContextOptions<DashboardContext>()))
            {
                var card = context.Cards.Include(c => c.State).SingleOrDefault(m => m.Id == id);
                if (card == null)
                    return null;

                card.State = context.States.FirstOrDefault(s => s.Priority == card.State.Priority + 1);
                context.SaveChanges();

                var dashboardContext = context.Cards.Include(c => c.State);
                return dashboardContext.FirstOrDefault(c => c.Id == card.Id);
            }
        }

        public Card DeleteCard(Guid id)
        {
            using (var context = new DashboardContext(new DbContextOptions<DashboardContext>()))
            {
                var card = context.Cards.SingleOrDefault(m => m.Id == id);
                if (card == null)
                    return null;

                context.Cards.Remove(card);
                context.SaveChanges();
                return card;
            }
        }
    }
}
