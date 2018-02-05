using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KanbanBoard.Models;

namespace KanbanBoard.Controllers
{
    //[Route("api/[controller]")]
    public class DashboardController : Controller
    {
        //GET: Dashboard
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IEnumerable<Card> GetAll()
        {
            using (var context = new DashboardContext(new DbContextOptions<DashboardContext>()))
            {
                var dashboardContext = context.Cards.Include(c => c.State);
                return dashboardContext.ToList();
            }
        }

        // GET: Dashboard/Create
        public IActionResult Create()
        {
            using (var context = new DashboardContext(new DbContextOptions<DashboardContext>()))
            {
                ViewData["StatePriority"] = new SelectList(context.States, "Priority", "Name");
            }
            return View();
        }

        [HttpPost]
        public IActionResult PostCard([FromBody]Card card)
        {
            using (var context = new DashboardContext(new DbContextOptions<DashboardContext>()))
            {
                card.Id = Guid.NewGuid();
                card.State = context.States.FirstOrDefault(s => s.Name == "ToDo");
                context.Add(card);
                context.SaveChanges();
            }
            return Ok(card);
        }

        [HttpPost]
        public IActionResult UpdateCard([FromBody]Card uCard)
        {
            using (var context = new DashboardContext(new DbContextOptions<DashboardContext>()))
            {
                var card = context.Cards.FirstOrDefault(c => c.Id == uCard.Id);
                if (card == null)
                    return NotFound();
                try
                {
                    card.Title = uCard.Title;
                    card.Description = uCard.Description;
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }

                var dashboardContext = context.Cards.Include(c => c.State);
                return Ok(dashboardContext.FirstOrDefault(c => c.Id == uCard.Id));
            }
        }

        [HttpGet]
        public IActionResult ChangeStatusCard(Guid id)
        {
            using (var context = new DashboardContext(new DbContextOptions<DashboardContext>()))
            {
                var card = context.Cards.Include(c => c.State).SingleOrDefault(m => m.Id == id);
                if (card == null)
                {
                    return NotFound();
                }

                card.State = context.States.FirstOrDefault(s => s.Priority == card.State.Priority + 1);
                context.SaveChanges();

                var dashboardContext = context.Cards.Include(c => c.State);
                return Ok(dashboardContext.FirstOrDefault(c => c.Id == card.Id));
            }
        }


        [HttpDelete]
        public IActionResult DeleteCard(string id)
        {
            using (var context = new DashboardContext(new DbContextOptions<DashboardContext>()))
            {
                var card = context.Cards.SingleOrDefault(m => m.Id == new Guid(id));
                if (card == null)
                {
                    return NotFound();
                }

                context.Cards.Remove(card);
                context.SaveChanges();
                return Ok(card);
            }
        }
    }
}
