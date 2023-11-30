using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TodoListAPI.Controllers;
using TodoListAPI.Models;
using Xunit;

public class TodoItemsControllerTests
{
    private readonly Mock<DbSet<TodoItem>> mockSet;
    private readonly Mock<TodoContext> mockContext;
    private readonly TodoItemsController controller;

    public TodoItemsControllerTests()
    {
        mockSet = new Mock<DbSet<TodoItem>>();
        mockContext = new Mock<TodoContext>();

        // Call SetupMockSet before accessing mockSet.Object
        SetupMockSet(new List<TodoItem>());

        // Setup the mock context to return the mock set
        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);

        // Create an instance of the controller with the mock context
        controller = new TodoItemsController(mockContext.Object);
    }

    /// <summary>
    /// Configures the mock set with a given list of TodoItems.
    /// </summary>
    /// <param name="items">The list of TodoItems to be set up in the mock set.</param>
    private void SetupMockSet(IEnumerable<TodoItem> items)
    {
        var queryableItems = items.AsQueryable();

        // Configure the mockSet to mimic IQueryable behavior
        mockSet.As<IAsyncEnumerable<TodoItem>>()
           .Setup(m => m.GetAsyncEnumerator(default))
           .Returns(new AsyncEnumerator<TodoItem>(items.GetEnumerator()));

        mockSet.As<IQueryable<TodoItem>>().Setup(m => m.Provider).Returns(queryableItems.Provider);
        mockSet.As<IQueryable<TodoItem>>().Setup(m => m.Expression).Returns(queryableItems.Expression);
        mockSet.As<IQueryable<TodoItem>>().Setup(m => m.ElementType).Returns(queryableItems.ElementType);
        mockSet.As<IQueryable<TodoItem>>().Setup(m => m.GetEnumerator()).Returns(() => queryableItems.GetEnumerator());
    }

    [Fact]
    public async Task GetTodoitems_ReturnsAllItems()
    {
        // Arrange
        var items = new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "Test Item 1" },
            new TodoItem { Id = 2, Title = "Test Item 2" }
        }.AsQueryable();

        // Configure the mockSet to return items
        SetupMockSet(items);

        // Act
        var result = await controller.GetTodoItems();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<TodoItem>>>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(actionResult.Value);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task GetTodoitem_ReturnsItemById()
    {
        // Arrange
        var item = new TodoItem { Id = 1, Title = "Test Item 1" };
        mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(item);

        // Act
        var result = await controller.GetTodoItem(1);

        // Assert
        var actionResult = Assert.IsType<ActionResult<TodoItem>>(result);
        var model = Assert.IsType<TodoItem>(actionResult.Value);
        Assert.Equal(1, model.Id);
    }

    [Fact]
    public async Task DeleteTodoItem_DeletesItemAndReturnsNoContent()
    {
        // Arrange
        var itemToDelete = new TodoItem { Id = 1, Title = "Delete Item" };
        mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(itemToDelete);
        mockSet.Setup(m => m.Remove(itemToDelete)).Verifiable();
        mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        var result = await controller.DeleteTodoItem(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        mockSet.Verify(m => m.Remove(itemToDelete), Times.Once());
        mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
    }

    [Fact]
    public async Task PostTodoItem_CreatesItemAndReturnsActionResult()
    {
        // Arrange
        var newItem = new TodoItem { Id = 1, Title = "New Test Item", Description = "New Item Description" };

        // Act
        var result = await controller.PostTodoItem(newItem);

        // Assert
        Assert.IsType<ActionResult<TodoItem>>(result);
    }
}

public class AsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> enumerator;

    public AsyncEnumerator(IEnumerator<T> enumerator) => this.enumerator = enumerator;

    public T Current => enumerator.Current;

    public ValueTask DisposeAsync()
    {
        enumerator.Dispose();
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(enumerator.MoveNext());
}