using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using dicky_todolist.Data;
using dicky_todolist.Controllers;
using dicky_todolist.Models;
using dicky_todolist.DTOs.Todo;
using Xunit;

public class TodoControllerTests
{
    private AppDbContext GetDatabaseContext()
    {
        // Pake database di memori biar cepet dan nggak ngerusak DB asli
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var databaseContext = new AppDbContext(options);
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithListOfTodos()
    {
        // Arrange
        var context = GetDatabaseContext();
        context.Todos.Add(new Todo { Id = Guid.NewGuid(), Title = "Test 1", CreatedAt = DateTime.UtcNow });
        await context.SaveChangesAsync();
        var controller = new TodosController(context);

        // Act
        var result = await controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var items = Assert.IsAssignableFrom<IEnumerable<TodoResponseDto>>(okResult.Value);
        Assert.Single(items);
    }

    [Fact]
    public async Task GetTodo_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        // Arrange
        var context = GetDatabaseContext();
        var controller = new TodosController(context);

        // Act
        var result = await controller.GetTodo(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateTodo_ShouldReturnCreated_WhenInputIsValid()
    {
        // Arrange
        var context = GetDatabaseContext();
        var controller = new TodosController(context);
        var request = new TodoCreateRequestDto("Belajar Unit Test", "Tes Deskripsi");

        // Act
        var result = await controller.CreateTodo(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var model = Assert.IsType<TodoResponseDto>(createdResult.Value);
        Assert.Equal(request.Title, model.Title);
        Assert.Equal(201, createdResult.StatusCode);
    }

    [Fact]
    public async Task CompleteTodo_ShouldReturnNoContent_AndSetStatusToTrue()
    {
        // Arrange
        var context = GetDatabaseContext();
        var todoId = Guid.NewGuid();
        context.Todos.Add(new Todo { Id = todoId, Title = "Belum Selesai", IsCompleted = false });
        await context.SaveChangesAsync();
        var controller = new TodosController(context);

        // Act
        var result = await controller.CompleteTodo(todoId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var updatedTodo = await context.Todos.FindAsync(todoId);
        Assert.True(updatedTodo!.IsCompleted);
    }

    [Fact]
    public async Task DeleteTodo_ShouldReturnNotFound_WhenIdInvalid()
    {
        // Arrange
        var context = GetDatabaseContext();
        var controller = new TodosController(context);

        // Act
        var result = await controller.DeleteTodo(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}