using System;
using System.ComponentModel.DataAnnotations;

namespace KanbanBoard.Models
{
    public class Card
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        public int StatePriority { get; set; }
        public virtual State State { get; set; }
    }
}
