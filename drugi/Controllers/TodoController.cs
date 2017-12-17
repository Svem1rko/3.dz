using System;
using System.Threading.Tasks;
using drugi.Entities;
using drugi.Models;
using drugi.Models.TodoModels;
using drugi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace drugi.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _user;

        public TodoController(ITodoRepository repository, UserManager<ApplicationUser> user)
        {
            _repository = repository;
            _user = user;
        }
        
        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await _user.GetUserAsync(HttpContext.User);
            var userId = currentUser.Id;

            var todo = new TodoItem();
            var a = _repository.GetActive(userId);

            IndexViewModel ivm = new IndexViewModel(a);

            return View(ivm);
        }      
        

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel vm)
        {
            ApplicationUser currentUser = await _user.GetUserAsync(HttpContext.User);
            var userId = currentUser.Id;

            if (ModelState.IsValid)
            {
                vm.UserId = userId;
                vm.DetermineLabels();
                _repository.Add(new TodoViewModel(vm));
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        public async Task<IActionResult> Completed()
        {
            ApplicationUser currentUser = await _user.GetUserAsync(HttpContext.User);
            var userId = currentUser.Id;

            var a = _repository.GetCompleted(userId);

            CompletedViewModel cvm = new CompletedViewModel(a);

            return View(cvm);
        }

        public async Task<IActionResult> Mark(Guid id)
        {
            ApplicationUser currentUser = await _user.GetUserAsync(HttpContext.User);
            var userId = currentUser.Id;

            _repository.MarkAsCompleted(id, userId);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(Guid id)
        {
            ApplicationUser currentUser = await _user.GetUserAsync(HttpContext.User);
            var userId = currentUser.Id;

            _repository.Remove(id, userId);

            return RedirectToAction("Completed");
        }
    }
}