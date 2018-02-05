using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using KanbanBoard.Models;

namespace KanbanBoard.Controllers
{
    //[Route("api/[controller]")]
    public class DashboardController : Controller
    {
        private BusinessLogic _businessLogic;

        public BusinessLogic BusinessLogic => _businessLogic ?? (_businessLogic = new BusinessLogic());

        //GET: Dashboard
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IEnumerable<Card> GetAll()
        {
            return BusinessLogic.GetAllCards();
        }

        // GET: Dashboard/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PostCard([FromBody]Card card)
        {
            BusinessLogic.AddCard(card);
            return Ok(card);
        }

        [HttpPost]
        public IActionResult UpdateCard([FromBody]Card uCard)
        {
            var card = BusinessLogic.UpdateCard(uCard);
            return card == null ? (IActionResult)NotFound() : Ok(card);
        }

        [HttpGet]
        public IActionResult ChangeStatusCard(Guid id)
        {
            var card = BusinessLogic.ChangeStatusCard(id);
            return card == null ? (IActionResult)NotFound() : Ok(card);
        }


        [HttpDelete]
        public IActionResult DeleteCard(Guid id)
        {
            var card = BusinessLogic.DeleteCard(id);
            return card == null ? (IActionResult)NotFound() : Ok(card);
        }
    }
}
