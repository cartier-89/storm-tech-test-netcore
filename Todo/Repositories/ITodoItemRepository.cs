using System.Threading.Tasks;
using Todo.Models.TodoItems;

namespace Todo.Repositories
{
    /// <summary>
    /// Repository for manipulating TodoItem entities.
    /// </summary>
    public interface ITodoItemRepository
    {
        /// <summary>
        /// Creates asynchronously TodoItem entity.
        /// </summary>
        /// <param name="create">Object holding all parameters of the TodoItem entity.</param>
        Task<int> CreateAsync(TodoItemCreateFields create);
    }
}
