using System;
using System.Data.Common;
using System.Threading.Tasks;
using Dashboard.Collections.Enums;
using Dashboard.Core.Repositories;
using Dashboard.Core.Models;
using Dashboard.Dtos.Payload.Task;
using Dashboard.Services.Container.TaskContainer;
using Dashboard.Services.Mappings.Profiles.Task;
using Dashboard.TestHelpers;
using Microsoft.Extensions.Logging;

namespace Dashboard.Api.UnitTests.Services.TaskServiceTests;

public class TaskServiceCreate
{

    private readonly Mock<ICrudRepository<TaskModel>> _crudRepository;
    private readonly Mock<ILogger<TaskService>> _logger;
    private readonly TaskService _taskService;

    public TaskServiceCreate()
    {
        var taskProfile = new TaskProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(taskProfile));
        var mapper = new Mapper(configuration);

        _logger = new Mock<ILogger<TaskService>>();
        _crudRepository = new Mock<ICrudRepository<TaskModel>>();

        _taskService = new TaskService
        (
            mapper,
            _crudRepository.Object,
            _logger.Object
        );
    }

    [Fact]
    public async Task CreateTask_ShouldPass_WhenDateIsNull()
    {
        // arrange
        var dateTime = DateTimeOffset.Parse("Mon, 24 Jul 2023 16:46:35 GMT").UtcDateTime;
        var taskPostRequestDto = new TaskPostRequestDto
        (
                Name: "Task 1/1",
                Description: null,
                DueDate: dateTime,
                Priority: PriorityEnum.Low,
                Status: StatusEnum.Pending
        );
        TaskHelper.GetTask().DueDate = dateTime;
        _crudRepository.Setup(x => x.Create(TaskHelper.GetTask())).Returns(Task.CompletedTask);

        // act
        var createdTask = await _taskService.CreateTask(taskPostRequestDto);

        // assert
        createdTask.Name.Should().Be(TaskHelper.GetTask().Name);
        createdTask.Description.Should().BeNull();
        createdTask.DueDate.Should().Be(TaskHelper.GetTask().DueDate);
        createdTask.Priority.Should().Be(TaskHelper.GetTask().Priority);
        createdTask.Status.Should().Be(TaskHelper.GetTask().Status);

        _crudRepository.Verify(x => x
            .Create(It.Is<TaskModel>(y => y.Description == null)), Times.Once);
    }
}