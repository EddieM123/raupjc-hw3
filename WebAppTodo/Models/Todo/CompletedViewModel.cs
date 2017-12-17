using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTodo.Models.Todo
{
    public class CompletedViewModel
    {

        public List<TodoViewModel> TodoViewModels { get; set; }

        public CompletedViewModel()
        {
            TodoViewModels = new List<TodoViewModel>();
        }
    }
}
