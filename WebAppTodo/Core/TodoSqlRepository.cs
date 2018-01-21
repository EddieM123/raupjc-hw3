using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebAppTodo.Core
{
    class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }


        public TodoItemLabel LabelExists(TodoItemLabel tl)
        {
            TodoItemLabel curr = _context.TodoItemLabels.Where(t => t.Value == tl.Value).FirstOrDefault();
            if (curr == null)
            {
                _context.TodoItemLabels.Add(tl);
                _context.SaveChanges();
                return tl;
            }
            else
            {
                return curr;
            }
        }

        /// <summary >
        /// Gets TodoItem for a given id. Throw TodoAccessDeniedException with appropriate message if user is not the owner of the Todo item
        /// </ summary >
        /// <param name =" todoId " > Todo Id </ param >
        /// <param name =" userId " >Id of the user that is trying to fetch the  data</ param >
        /// <returns > TodoItem if found , null otherwise </ returns >
        public TodoItem Get(Guid todoId, Guid userId)
        {
            TodoItem todoItem = _context.TodoItems.Where(x => x.Id == todoId).First();
            if (todoItem is null) return null;
            if (todoItem.UserId != userId) throw new TodoAccessDeniedException("Access Denied!");

            return todoItem;
        }

        /// <summary >
        /// Adds new TodoItem object in database .
        /// If object with the same id already exists ,
        /// method should throw DuplicateTodoItemException with the message  " duplicate id: {id }".
        /// </ summary >
        public void Add(TodoItem todoItem)
        {
            TodoItem td = _context.TodoItems.Where(s => s.Id == todoItem.Id).FirstOrDefault();
            if(td is null)
            {
                _context.TodoItems.Add(todoItem);
                _context.SaveChanges();
            }
            else
            {
                throw new DulpicateItemException("Duplicate id: {0}", todoItem.Id);
            }

        }

        /// <summary >
        /// Tries to remove a TodoItem with given id from the database . Throw  TodoAccessDeniedException with appropriate message if user is not
        /// the owner of the Todo item
        /// </ summary >
        /// <param name =" todoId " > Todo Id </ param >
        /// <param name =" userId " >Id of the user that is trying to remove the
        /// data</ param >
        /// <returns > True if success , false otherwise </ returns >
        public bool Remove(Guid todoId, Guid userId)
        {
            TodoItem td = _context.TodoItems.Where(s => s.Id == todoId).FirstOrDefault();
            if (td is null) return false;
            if(td.UserId != userId)
            {
                throw new TodoAccessDeniedException("Ids not matching!");
            }
            else
            {
                _context.TodoItems.Remove(td);
                _context.SaveChangesAsync();
                return true;
            }
        }

        /// <summary >
        /// Updates given TodoItem in database .
        /// If TodoItem does not exist , method will add one . Throw TodoAccessDeniedException with appropriate message if user is not
        /// the owner of the Todo item
        /// </ summary >
        /// <param name =" todoItem " > Todo item </ param >
        /// <param name =" userId " >Id of the user that is trying to update the data</ param >
        public void Update(TodoItem todoItem, Guid userId)
        {
            TodoItem td = _context.TodoItems.Where(s => s.Id == todoItem.Id).FirstOrDefault();
            if(td is null || td.UserId == userId)
            {
                td.Text = todoItem.Text;
                td.Labels = todoItem.Labels;
                td.DateDue = todoItem.DateDue;
                td.DateCompleted = todoItem.DateCompleted;
                _context.SaveChanges();
            }
            else
            {
                throw new TodoAccessDeniedException("User is not the owner of the Todo item!");
            }
        }

        /// <summary >
        /// Tries to mark a TodoItem as completed in database . Throw TodoAccessDeniedException with appropriate message if user is not
        /// the owner of the Todo item
        /// </ summary >
        /// <param name =" todoId " > Todo Id </ param >
        /// <param name =" userId " >Id of the user that is trying to mark as completed</ param >
        /// <returns > True if success , false otherwise </ returns >
        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            TodoItem td = _context.TodoItems.Where(s => s.Id == todoId).FirstOrDefault();
            if (td is null) return false;
            if (td.UserId != userId) throw new TodoAccessDeniedException("User is not the owner of the Todo item!");

            bool ret = td.MarkAsCompleted();
            
            _context.SaveChanges();
            return ret;
        }

        /// <summary >
        /// Gets all TodoItem objects in database for user , sorted by date created(descending )
        /// </ summary >
        public async Task<List<TodoItem>> GetAll(Guid userId)
        {
            return  await _context.TodoItems.Where(s => s.UserId == userId).Include(s => s.Labels).OrderByDescending(s => s.DateCreated).ToListAsync();
        }

        /// <summary >
        /// Gets all incomplete TodoItem objects in database for user
        /// </ summary >
        public async Task<List<TodoItem>> GetActive(Guid userId)
        {
            return await _context.TodoItems.Where(s => s.UserId == userId && !s.DateCompleted.HasValue).Include(s => s.Labels).OrderByDescending(s => s.DateCreated).ToListAsync();
        }

        /// <summary >
        /// Gets all completed TodoItem objects in database for user
        /// </ summary >
        public async Task<List<TodoItem>> GetCompleted(Guid userId)
        {
            return await _context.TodoItems.Where(s => s.UserId == userId && s.DateCompleted.HasValue).Include(s => s.Labels).OrderByDescending(s => s.DateCreated).ToListAsync();
        }

        /// <summary >
        /// Gets all TodoItem objects in database for user that apply to the filter
        /// </ summary >
        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.TodoItems.Where(s => s.UserId == userId).Include(s => s.Labels).Where(filterFunction).OrderByDescending(s => s.DateCreated).ToList();
        }

        

        [Serializable]
        class DulpicateItemException : Exception
        {
            public DulpicateItemException(string v, Guid id) : base(String.Format("Duplicate id: {0}", id))
            {

            }
        }


        [Serializable]
        class TodoAccessDeniedException : Exception
        {
            public TodoAccessDeniedException(string v) : base(String.Format("The given ID does not match the id of the owner!"))
            {

            }
        }
    }

    
}
