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




        // GET: /todo/
        public async Task<IActionResult> Index()
        {
            var todo = _repository.GetActive(new Guid((await _userManager.GetUserAsync(HttpContext.User)).Id));
            var viewModel = new IndexViewModel();

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
                var todoItem = new TodoItem(model.Text, model.DateDue, new Guid((await _userManager.GetUserAsync(HttpContext.User)).Id));
                
                _repository.Add(todoItem);
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }




    }
}