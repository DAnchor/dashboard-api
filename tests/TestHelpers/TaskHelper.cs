using Dashboard.Collections.Enums;
using Dashboard.Core.Models;
using System;

namespace Dashboard.TestHelpers;

public static class TaskHelper
{
    public static TaskModel GetTask()
    {
        return new TaskModel
        {
            Id = 1,
            Name = "Task 1/1",
            Description = "Some task description",
            DueDate = DateTimeOffset.Parse("Mon, 24 Jul 2023 16:46:35 GMT").UtcDateTime,
            Priority = PriorityEnum.Low,
            Status = StatusEnum.Pending
        };
    }
}