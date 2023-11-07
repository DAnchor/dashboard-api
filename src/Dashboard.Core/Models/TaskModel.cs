using Dashboard.Collections.Enums;

namespace Dashboard.Core.Models;

public class TaskModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public PriorityEnum Priority { get; set; }
    public StatusEnum Status { get; set; }

    public TaskModel() { }

    public TaskModel
    (
        int id,
        string name,
        string description,
        DateTime dueDate,
        PriorityEnum priority,
        StatusEnum status
    )
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        DueDate = dueDate;
        Priority = priority;
        Status = status;
    }
}