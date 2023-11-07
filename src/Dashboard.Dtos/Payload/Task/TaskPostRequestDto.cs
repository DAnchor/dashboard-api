using Dashboard.Collections.Enums;
using System.Text.Json.Serialization;

namespace Dashboard.Dtos.Payload.Task;

public record class TaskPostRequestDto
(
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("description")]
    string? Description,
    [property: JsonPropertyName("duedate")]
    DateTime DueDate,
    [property: JsonPropertyName("priority")]
    PriorityEnum Priority,
    [property: JsonPropertyName("status")]
    StatusEnum Status
);