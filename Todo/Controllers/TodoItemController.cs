using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Exceptions;
using Todo.Models.TodoItems;
using Todo.Repositories;
using Todo.Services;

namespace Todo.Controllers
{
    [Authorize]
    public class TodoItemController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ITodoItemRepository _todoItemRepository;

        public TodoItemController(ApplicationDbContext dbContext, ITodoItemRepository todoItemRepository)
        {
            this.dbContext = dbContext;
            _todoItemRepository = todoItemRepository;
        }

        [HttpGet]
        public IActionResult Create(int todoListId)
        {
            var todoList = dbContext.SingleTodoList(todoListId);
            var fields = TodoItemCreateFieldsFactory.Create(todoList, User.Id());
            return View(fields);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoItemCreateFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            await _todoItemRepository.CreateAsync(fields);

            return RedirectToListDetail(fields.TodoListId);
        }

        [HttpPost("unused")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItem(TodoItemCreateFields fields)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            int id = await _todoItemRepository.CreateAsync(fields);

            return Ok(id);
        }

        [HttpGet]
        public IActionResult Edit(int todoItemId)
        {
            var todoItem = dbContext.SingleTodoItem(todoItemId);
            var fields = TodoItemEditFieldsFactory.Create(todoItem);
            return View(fields);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TodoItemEditFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            var todoItem = dbContext.SingleTodoItem(fields.TodoItemId);

            TodoItemEditFieldsFactory.Update(fields, todoItem);

            dbContext.Update(todoItem);
            await dbContext.SaveChangesAsync();

            return RedirectToListDetail(todoItem.TodoListId);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ModifyRank([FromRoute] int id, int rank)
        {
            try
            {
                await _todoItemRepository.ModifyRankAsync(id, rank);
                return NoContent();
            }
            catch (TodoItemNotFoundException e)
            {
                return NotFound();
            }
        }

        private RedirectToActionResult RedirectToListDetail(int fieldsTodoListId)
        {
            return RedirectToAction("Detail", "TodoList", new {todoListId = fieldsTodoListId});
        }
    }
}
