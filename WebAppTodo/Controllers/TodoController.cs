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




        // GET: /<controller>/
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

        


    }
}