using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Models.TodoItems;

namespace Todo.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly ApplicationDbContext dbContext;

        public TodoItemRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> CreateAsync(TodoItemCreateFields create)
        {
            var item = new TodoItem(create.TodoListId, create.ResponsiblePartyId, create.Title, create.Importance);
            EntityEntry<TodoItem> entity = await dbContext.AddAsync(item);
            await dbContext.SaveChangesAsync();
            return entity.Entity.TodoItemId;
        }
    }
}
