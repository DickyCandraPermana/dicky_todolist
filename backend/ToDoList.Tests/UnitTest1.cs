using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using dicky_todolist.Data;
using dicky_todolist.Controllers;
using dicky_todolist.Models;
using dicky_todolist.DTOs.Todo;
using dicky_todolist.Exceptions;
using dicky_todolist.Validators;
using System.Security.Claims;

public class TodoControllerTests
{
    private AppDbContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var databaseContext = new AppDbContext(options);
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }

    private TodosController CreateController(AppDbContext context, Guid userId)
    {
        var controller = new TodosController(
            context,
            new TodoCreateRequestValidator(),
            new TodoUpdateRequestValidator());

        var identity = new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ],
        "TestAuth");

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(identity)
            }
        };

        return controller;
    }

    private async Task<User> SeedUser(AppDbContext context, Guid userId)
    {
        var user = new User
        {
            Id = userId,
            Username = "tester",
            Email = "tester@example.com",
            Password = "hash"
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithListOfTodos()
    {
        var context = GetDatabaseContext();
        var userId = Guid.NewGuid();
        await SeedUser(context, userId);
        context.Todos.Add(new Todo { Id = Guid.NewGuid(), Title = "Test 1", CreatedAt = DateTime.UtcNow, UserId = userId });
        await context.SaveChangesAsync();
        var controller = CreateController(context, userId);

        var result = await controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var items = Assert.IsAssignableFrom<IEnumerable<TodoResponseDto>>(okResult.Value);
        Assert.Single(items);
    }

    [Fact]
    public async Task GetTodo_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var context = GetDatabaseContext();
        var userId = Guid.NewGuid();
        await SeedUser(context, userId);
        var controller = CreateController(context, userId);

        await Assert.ThrowsAsync<TodoNotFoundException>(() => controller.GetTodo(Guid.NewGuid()));

    }

    [Fact]
    public async Task CreateTodo_ShouldReturnCreated_WhenInputIsValid()
    {
        var context = GetDatabaseContext();
        var userId = Guid.NewGuid();
        await SeedUser(context, userId);
        var controller = CreateController(context, userId);
        var request = new TodoCreateRequestDto("Belajar Unit Test", "Tes Deskripsi");

        var result = await controller.CreateTodo(request);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var model = Assert.IsType<TodoResponseDto>(createdResult.Value);
        Assert.Equal(request.Title, model.Title);
        Assert.Equal(201, createdResult.StatusCode);
    }

    [Fact]
    public async Task CompleteTodo_ShouldReturnNoContent_AndSetStatusToTrue()
    {
        var context = GetDatabaseContext();
        var userId = Guid.NewGuid();
        await SeedUser(context, userId);
        var todoId = Guid.NewGuid();
        context.Todos.Add(new Todo { Id = todoId, Title = "Belum Selesai", IsCompleted = false, UserId = userId });
        await context.SaveChangesAsync();
        var controller = CreateController(context, userId);

        var result = await controller.CompleteTodo(todoId);

        Assert.IsType<OkObjectResult>(result);
        var updatedTodo = await context.Todos.FindAsync(todoId);
        Assert.True(updatedTodo!.IsCompleted);
    }

    [Fact]
    public async Task DeleteTodo_ShouldReturnNotFound_WhenIdInvalid()
    {
        var context = GetDatabaseContext();
        var userId = Guid.NewGuid();
        await SeedUser(context, userId);
        var controller = CreateController(context, userId);

        await Assert.ThrowsAsync<TodoNotFoundException>(() => controller.DeleteTodo(Guid.NewGuid()));

    }
}