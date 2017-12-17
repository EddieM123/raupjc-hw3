using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using WebAppTodo.Core;
using WebAppTodo.Models;
using WebAppTodo.Models.Todo;

namespace WebAppTodo.Controllers
{

    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }




        // GET: /todo/index
        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel();
            var todo = _repository.GetActive(await GetCurrentUserId());

            foreach (var td in todo)
            {
               viewModel.TodoViewModels.Add(new TodoViewModel(td));
            }

            return View(viewModel);
        }

        // GET: /todo/Add
        public IActionResult Add()
        {
            return View(new AddTodoViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var todoItem = new TodoItem(model.Text, model.DateDue, await GetCurrentUserId());
                
                _repository.Add(todoItem);
                return RedirectToAction("Index");
            }
            
            return View(model);
        }

        public async Task<IActionResult> Completed()
        {
            var viewModel = new CompletedViewModel();
            var todos = _repository.GetCompleted(await GetCurrentUserId());

            foreach (var todo in todos)
            {
                viewModel.TodoViewModels.Add(new TodoViewModel(todo));
            }

            return View(viewModel);
        }

        public async Task<IActionResult> MarkAsCompleted(Guid todoId)
        {
            var userId = await GetCurrentUserId();
            _repository.MarkAsCompleted(todoId, userId);
            return RedirectToAction("Index");
        }

       

        /// <summary>
        /// Gets Guid of the currently logged user. Think about pulling this helper method outside,
        /// so other controllers can use it!
        /// </summary>
        private async Task<Guid> GetCurrentUserId()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return new Guid(user.Id);
        }
    }
}