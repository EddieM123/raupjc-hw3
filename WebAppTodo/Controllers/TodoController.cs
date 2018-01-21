using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppTodo.Models.Todo;
using WebAppTodo.Core;
using WebAppTodo.Models;
using System.Diagnostics;

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
            var todoItems = await _repository.GetActive(await GetUserId());
            IndexViewModel indexViewModel = new IndexViewModel();

            foreach (var td in todoItems)
            {
                TodoViewModel tdv = new TodoViewModel(td);
                indexViewModel.TodoViewModels.Add(tdv);
            }

            return View(indexViewModel);
        }

        public async Task<IActionResult> Completed()
        {
            var todoItems = await _repository.GetCompleted(await GetUserId());
            CompletedViewModel completedViewModel = new CompletedViewModel();

            foreach (var td in todoItems)
            {
                TodoViewModel tdv = new TodoViewModel(td);
                completedViewModel.TodoViewModels.Add(tdv);
            }

            return View(completedViewModel);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel model)
        {
            if (ModelState.IsValid)
            {
                TodoItem newItem = new TodoItem(model.Text, await GetUserId());
                newItem.DateDue = model.DateDue;

                if (!string.IsNullOrEmpty(model.Labels))
                {
                    var lbls = model.Labels.Split(',').Select(tl => tl.Trim().ToLower());
                    foreach (var label in lbls)
                    {
                        if (label.Equals(""))
                        {
                            continue;
                        }

                        var newlbl = new TodoItemLabel(label);
                        var lbl = _repository.LabelExists(newlbl);
                        if (!newItem.Labels.Contains(lbl))
                        {
                            newItem.Labels.Add(lbl);
                        }
                    }
                }
                _repository.Add(newItem);
                return RedirectToAction("Index");
            }
            return View("Add");
        }

        
        public async Task<IActionResult> MarkAsCompleted(Guid guid)
        {
            _repository.MarkAsCompleted(guid, await GetUserId());
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> RemoveFromCompleted(Guid guid)
        {
            var todo = _repository.Get(guid, await GetUserId());

            todo.DateCompleted = null;  
            _repository.Update(todo, await GetUserId());

            return RedirectToAction("Completed");
        }

        public async Task<Guid> GetUserId()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            return new Guid(user.Id);
        }

    }
}