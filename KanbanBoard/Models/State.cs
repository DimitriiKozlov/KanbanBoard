using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models
{
    public class State
    {
        [Key]
        public int Priority { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
