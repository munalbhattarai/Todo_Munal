using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    public class TodoController : Controller
    {
        private readonly TodoContext _db;
        public TodoController(TodoContext db) => _db = db;

        // GET: /Todo
        public async Task<IActionResult> Index()
        {
            var items = await _db.TodoItems.OrderByDescending(t => t.CreatedAt).ToListAsync();
            return View(items);
        }

        // POST: /Todo/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return BadRequest("Title required");

            var todo = new TodoItem { Title = title.Trim() };
            _db.TodoItems.Add(todo);
            await _db.SaveChangesAsync();

            // Return partial HTML for the new item so client can append it (AJAX)
            return PartialView("_TodoItemPartial", todo);
        }

        // POST: /Todo/Toggle/5
        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            var item = await _db.TodoItems.FindAsync(id);
            if (item == null) return NotFound();
            item.IsDone = !item.IsDone;
            await _db.SaveChangesAsync();
            return Ok(new { item.Id, item.IsDone });
        }

        // POST: /Todo/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.TodoItems.FindAsync(id);
            if (item == null) return NotFound();
            _db.TodoItems.Remove(item);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
