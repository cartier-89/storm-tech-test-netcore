using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Todo.Controllers;
using Todo.Data;
using Todo.Exceptions;
using Todo.Repositories;
using Xunit;

namespace Todo.Tests.Controllers
{
    public class TodoitemControllerTests
    {
        private TodoItemController _controller;
        private Mock<ITodoItemRepository> _toDoitemRepositoryMock;

        public TodoitemControllerTests()
        {
            _toDoitemRepositoryMock = new Mock<ITodoItemRepository>();

            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "todo.db")
                .Options;
            var context = new ApplicationDbContext(options);

            _controller = new TodoItemController(context, _toDoitemRepositoryMock.Object);
        }

        [Fact]
        public async Task Put_ModifyRank_ReturnsNoContent()
        {
            // Act
            IActionResult response = await _controller.ModifyRank(42, 42);

            // Assert
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public async Task Put_ModifyRankWithMissingItem_ReturnsNotFound()
        {
            // Arrange
            var exception = new TodoItemNotFoundException(42);
            _toDoitemRepositoryMock
                .Setup(r => r.ModifyRankAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(exception);

            // Act
            IActionResult response = await _controller.ModifyRank(42, 42);

            //Assert
            Assert.IsType<NotFoundResult>(response);
        }
    }
}
