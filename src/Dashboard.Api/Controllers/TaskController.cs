using Dashboard.Api.Validations.Task;
using Dashboard.Core.Models;
using Dashboard.Dtos.Payload.Task;
using Dashboard.Services.Container.TaskContainer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ITaskService _taskService;

    public TaskController
    (
        ILogger<TaskController> logger,
        ITaskService taskService
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _taskService = taskService ?? throw new ArgumentNullException(nameof(logger));
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost]
    [Route("CreateTask")]
    public async Task<ActionResult> CreateTask([FromForm] TaskPostRequestDto request)
    {
        try
        {
            var taskRequestDtoValidator = new TaskPostRequestValidator();
            var validationResult = taskRequestDtoValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.First());
            }

            var response = await _taskService.CreateTask(request);

            if (response != null)
            {
                return CreatedAtAction(nameof(CreateTask), response);
            }

            return BadRequest(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("GetAllTasks")]
    public async Task<ActionResult<IEnumerable<TaskModel>>> GetAllTasks()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            var tasks = await _taskService.ReadAllTasks();

            _logger.LogInformation("executing ReadAllTasks()");

            return Ok(tasks);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetTaskById/{id}")]
    public async Task<ActionResult<TaskModel>> GetTaskById(string id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Invalid task id.");
            }

            var user = await _taskService.ReadTaskById(id);

            if (user == null)
            {
                return NotFound("Task does not exist.");
            }

            return Ok(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateTask")]
    public async Task<ActionResult> UpdateTask([FromForm] TaskUpdateRequestDto request)
    {
        try
        {
            var taskRequestDtoValidator = new TaskUpdateRequestValidator();
            var validationResult = taskRequestDtoValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.First());
            }

            var task = await _taskService.ReadTaskById(request.Id.ToString());

            if (task == null)
            {
                return NotFound("Task does not exist.");
            }

            var updatedTask = _taskService.UpdateTask(task, request);

            return Ok(updatedTask);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteTask/{id}")]
    public async Task<ActionResult> DeleteTask(string id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Invalid task id.");
            }

            var task = await _taskService.ReadTaskById(id);

            if (task == null)
            {
                return NotFound("Task does not exist.");
            }

            await _taskService.DeleteTask(task);

            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}