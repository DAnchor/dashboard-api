using AutoMapper;
using Dashboard.Core.Repositories;
using Dashboard.Core.Models;
using Dashboard.Dtos.Payload.Task;
using Microsoft.Extensions.Logging;

namespace Dashboard.Services.Container.TaskContainer;

public interface ITaskService
{
    Task<TaskModel> CreateTask(TaskPostRequestDto request);
    Task<IEnumerable<TaskModel>> ReadAllTasks();
    Task<TaskModel> ReadTaskById(string id);
    TaskModel UpdateTask(TaskModel taskModel, TaskUpdateRequestDto request);
    Task DeleteTask(TaskModel taskModel);
}

public class TaskService : ITaskService
{
    private readonly IMapper _mapper;
    private readonly ICrudRepository<TaskModel> _taskRepository;
    private readonly ILogger _logger;

    public TaskService
    (
        IMapper mapper,
        ICrudRepository<TaskModel> taskRepository,
        ILogger<TaskService> logger
    )
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TaskModel> CreateTask(TaskPostRequestDto request)
    {
        var taskMap = _mapper.Map<TaskPostRequestDto, TaskModel>(request);

        LogHighPriority((int)request.Priority, nameof(CreateTask));

        await _taskRepository.Create(taskMap);

        return taskMap;
    }

    public Task DeleteTask(TaskModel taskModel)
    {
        return _taskRepository.Delete(taskModel);
    }

    public async Task<IEnumerable<TaskModel>> ReadAllTasks()
    {
        return await _taskRepository.ReadAll();
    }

    public async Task<TaskModel> ReadTaskById(string id)
    {
        return await _taskRepository.ReadById(id);
    }

    public TaskModel UpdateTask(TaskModel taskModel, TaskUpdateRequestDto request)
    {
        var taskMap = _mapper.Map<TaskUpdateRequestDto, TaskModel>(request);

        LogHighPriority((int)request.Priority, nameof(UpdateTask));

        _taskRepository.Update(taskMap);

        return taskMap;
    }

    private void LogHighPriority(int priority, string nameofMethod)
    {
        if (priority == 3)
        {
            _logger.LogCritical(priority, $"WARNING: '{nameofMethod}' method has executed HIGH priority!!!");
        }
    }
}