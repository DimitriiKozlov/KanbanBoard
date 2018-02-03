using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models
{
    public class State
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Card> Cards { get; set; }


        public State()
        {
            Cards = new List<Card>();
        }
    }
}
