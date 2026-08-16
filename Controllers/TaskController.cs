using Microsoft.AspNetCore.Mvc;

namespace MultiClientApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")] // Route will be: /api/tasks
public class TasksController : ControllerBase
{
    // In-memory list acting as a temporary mock database
    private static readonly List<TaskItemDto> Tasks = new()
    {
        new(1, "Install Linux", true),
        new(2, "Learn ASP.NET Core Web API", false)
    };

    // 1. GET: api/tasks (Retrieve all)
    [HttpGet]
    public ActionResult<IEnumerable<TaskItemDto>> GetAll()
    {
        return Ok(Tasks); // 200 OK
    }

    // 2. GET: api/tasks/1 (Retrieve single resource)
    [HttpGet("{id:int}")]
    public ActionResult<TaskItemDto> GetById(int id)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            return NotFound(new { message = $"Task with ID {id} not found." }); // 404 Not Found
        }

        return Ok(task); // 200 OK
    }

    // 3. POST: api/tasks (Create resource)
    [HttpPost]
    public ActionResult<TaskItemDto> Create([FromBody] CreateTaskDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest(new { message = "Title cannot be empty." }); // 400 Bad Request
        }

        var newTask = new TaskItemDto(
            Id: Tasks.Count + 1,
            Title: request.Title,
            IsCompleted: false
        );

        Tasks.Add(newTask);

        // 201 Created with Location header pointing to api/tasks/{id}
        return CreatedAtAction(nameof(GetById), new { id = newTask.Id }, newTask);
    }

    // 4. DELETE: api/tasks/1 (Remove resource)
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            return NotFound(); // 404 Not Found
        }

        Tasks.Remove(task);
        return NoContent(); // 204 No Content
    }
}

// Data Transfer Objects (DTOs)
public record TaskItemDto(int Id, string Title, bool IsCompleted);
public record CreateTaskDto(string Title);