using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Exceptions;
using Todo.Repositories;
using Xunit;

namespace Todo.Tests.Repositories
{
    public class TodoItemRepositoryTests
    {
        private TodoItemRepository _todoItemRepository;
        private ApplicationDbContext _context;

        public TodoItemRepositoryTests()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "todo.db")
                .Options;
            _context = new ApplicationDbContext(options);

            _todoItemRepository = new TodoItemRepository(_context);
        }

        [Fact]
        public async Task ModifyRankAsync_ItemFound_SuccessfullyUpdatesRank()
        {
            // Arrange
            var user = new IdentityUser("user_1")
            {
                Id = Guid.NewGuid().ToString(),
            };
            _context.Users.Add(user);
            var item = new TodoItem(42, user.Id, "Fresh item", Importance.High)
            {
                Rank = 1,
            };
            _context.TodoItems.Add(item);
            int newRank = item.Rank + 5;
            await _context.SaveChangesAsync();

            // Act
            await _todoItemRepository.ModifyRankAsync(item.TodoItemId, newRank);

            // Assert
            TodoItem afterUpdate = await _context.TodoItems.SingleAsync(i => i.TodoItemId == item.TodoItemId);
            Assert.Equal(newRank, afterUpdate.Rank);
        }

        [Fact]
        public void ModifyRankAsync_NoItemFound_ThrowsTodoItemNotFoundException()
        {
            // Act and assert
            Assert.ThrowsAsync<TodoItemNotFoundException>(async () => await _todoItemRepository.ModifyRankAsync(1, 2));
        }
    }
}
