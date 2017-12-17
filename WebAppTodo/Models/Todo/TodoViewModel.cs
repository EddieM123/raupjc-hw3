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
        public List<string> TodoItemLabels { get; set; }
        public bool IsCompleted { get; set; }

        public TodoViewModel(TodoItem todoItem)
        {
            Id = todoItem.Id;
            Text = todoItem.Text;
            DateDue = todoItem.DateDue;
            TodoItemLabels = todoItem.Labels.Select(s => s.Value).ToList();
            IsCompleted = todoItem.IsCompleted;
        }

        public TodoViewModel()
        {

        }
    }
}
