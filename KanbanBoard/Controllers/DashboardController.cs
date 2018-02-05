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
        private readonly DashboardContext _context;

        public DashboardController(DashboardContext context)
        {
            _context = context;
            if (_context.States.ToList().Any())
            {
                return;
            }

            _context.States.Add(new State { Name = "ToDo" });
            _context.States.Add(new State { Name = "InProgress" });
            _context.States.Add(new State { Name = "Done" });
            _context.SaveChanges();
        }

        //GET: Dashboard
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IEnumerable<Card> GetAll()
        {
            var dashboardContext = _context.Cards.Include(c => c.State);
            return dashboardContext.ToList();
        }

        // GET: Dashboard/Create
        public IActionResult Create()
        {
            ViewData["StatePriority"] = new SelectList(_context.States, "Priority", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult PostCard([FromBody]Card card)
        {
            card.Id = Guid.NewGuid();
            card.State = _context.States.FirstOrDefault(s => s.Name == "ToDo");
            _context.Add(card);
            _context.SaveChanges();
            return Ok(card);
        }

        [HttpPost]
        public IActionResult UpdateCard([FromBody]Card uCard)
        {
            var card = _context.Cards.FirstOrDefault(c => c.Id == uCard.Id);
            if (card == null)
                return NotFound();
            try
            {
                card.Title = uCard.Title;
                card.Description = uCard.Description;
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            var dashboardContext = _context.Cards.Include(c => c.State);
            return Ok(dashboardContext.FirstOrDefault(c => c.Id == uCard.Id));
        }

        [HttpGet]
        public IActionResult ChangeStatusCard(Guid id)
        {
            var card = _context.Cards.Include(c => c.State).SingleOrDefault(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }
            card.State = _context.States.FirstOrDefault(s => s.Priority == card.State.Priority + 1);
            _context.SaveChanges();

            var dashboardContext = _context.Cards.Include(c => c.State);
            return Ok(dashboardContext.FirstOrDefault(c => c.Id == card.Id));
        }


        [HttpDelete]
        public IActionResult DeleteCard(string id)
        {
            var card = _context.Cards.SingleOrDefault(m => m.Id == new Guid(id));
            if (card == null)
            {
                return NotFound();
            }
            _context.Cards.Remove(card);
            _context.SaveChanges();
            return Ok(card);
        }
    }
}
