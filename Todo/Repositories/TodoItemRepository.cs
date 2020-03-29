using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Exceptions;
using Todo.Models.TodoItems;

namespace Todo.Repositories
{
    /// <summary>
    /// EntityFramework implementation of ITodoItemRepository
    /// </summary>
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Constructs ToDoItemRepository object.
        /// </summary>
        /// <param name="dbContext">Entity framework dbcontext to inject.</param>
        public TodoItemRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<int> CreateAsync(TodoItemCreateFields create)
        {
            var item = new TodoItem(create.TodoListId, create.ResponsiblePartyId, create.Title, create.Importance);
            EntityEntry<TodoItem> entity = await _dbContext.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return entity.Entity.TodoItemId;
        }

        /// <inheritdoc/>
        public async Task ModifyRankAsync(int id, int rank)
        {
            TodoItem item = await _dbContext.TodoItems.FindAsync(id);
            if (item == null)
            {
                throw new TodoItemNotFoundException(id);
            }
            item.Rank = rank;
            _dbContext.Update(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}
