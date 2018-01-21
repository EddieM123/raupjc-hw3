using System;
using System.Collections.Generic;
using System.Linq;
using WebAppTodo.Core;

namespace WebAppTodo.Models.Todo
{
    public class TodoViewModel
    {

        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime? DateDue { get; set; }
        public DateTime? Date_show { get; set; }
        public List<TodoItemLabel> TodoItemLabels { get; set; }
        public bool IsCompleted { get; set; }

        public TodoViewModel(TodoItem todoItem)
        {
            Id = todoItem.Id;
            Text = todoItem.Text;
            DateDue = todoItem.DateDue;

            if(todoItem.DateCompleted is null)
            {
                Date_show = todoItem.DateDue;
            }
            else
            {
                Date_show = todoItem.DateCompleted;
            }

            TodoItemLabels = todoItem.Labels;
            IsCompleted = todoItem.IsCompleted;
        }

        public TodoViewModel()
        {

        }
    }
}
