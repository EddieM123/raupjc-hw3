using System;
using System.Collections.Generic;

namespace zad1
{
    public class TodoItemLabel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        /// <summary >
        /// All TodoItems that are associated with this label
        /// </ summary >
        public List<TodoItem> LabelTodoItems { get; set; }
        public TodoItemLabel(string value)
        {
            Id = Guid.NewGuid();
            Value = value;
            LabelTodoItems = new List<TodoItem>();
        }
    }
}