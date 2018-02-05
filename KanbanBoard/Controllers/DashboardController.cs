using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            _context.States.Add(new State {Name = "ToDo"});
            _context.States.Add(new State {Name = "InProgress"});
            _context.States.Add(new State {Name = "Done"});
            _context.SaveChanges();
        }

        //GET: Dashboard
        public /*async Task<*/IActionResult Index()
        {
            //var dashboardContext = _context.Cards.Include(c => c.State);
            //return View(await dashboardContext.ToListAsync());
            return View();
        }

        [HttpGet]
        public IEnumerable<Card> GetAll()
        {
            var dashboardContext = _context.Cards.Include(c => c.State);
            return dashboardContext.ToList();
        }


        // GET: Dashboard/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .Include(c => c.State)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // GET: Dashboard/Create
        public IActionResult Create()
        {
            ViewData["StatePriority"] = new SelectList(_context.States, "Priority", "Name");
            return View();
        }

        //// POST: Dashboard/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Title,Description,StatePriority")] Card card)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        card.Id = Guid.NewGuid();
        //        _context.Add(card);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["StatePriority"] = new SelectList(_context.States, "Priority", "Name", card.StatePriority);
        //    return View(card);
        //}

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
                //_context.Cards.Update(card);

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

        // GET: Dashboard/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards.SingleOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }
            ViewData["StatePriority"] = new SelectList(_context.States, "Priority", "Name", card.StatePriority);
            return View(card);
        }

        //// POST: Dashboard/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Description,StatePriority")] Card card)
        //{
        //    if (id != card.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(card);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CardExists(card.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["StatePriority"] = new SelectList(_context.States, "Priority", "Name", card.StatePriority);
        //    return View(card);
        //}

        //// GET: Dashboard/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var card = await _context.Cards
        //        .Include(c => c.State)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (card == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(card);
        //}

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

        //// POST: Dashboard/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    var card = await _context.Cards.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Cards.Remove(card);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool CardExists(Guid id)
        {
            return _context.Cards.Any(e => e.Id == id);
        }
    }
}
