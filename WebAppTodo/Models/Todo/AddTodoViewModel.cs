using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTodo.Models.Todo
{
    public class AddTodoViewModel
    {
        [Required]
        public string Text { get; set; }

        [Display(Name = "Date due")]
        public DateTime? DateDue { get; set; }

        [Display(Name = "Labels - seperated by comma!")]
        public string Labels { get; set; }
    }
}
