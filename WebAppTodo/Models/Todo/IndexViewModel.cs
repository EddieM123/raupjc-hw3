using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTodo.Models.Todo
{
    public class IndexViewModel
    {

        public List<TodoViewModel> TodoViewModels { get; set; }

        public IndexViewModel()
        {
            TodoViewModels = new List<TodoViewModel>();
        }
    }
}
