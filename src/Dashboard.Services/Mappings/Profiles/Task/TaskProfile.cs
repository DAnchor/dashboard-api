using AutoMapper;
using Dashboard.Core.Models;
using Dashboard.Dtos.Payload.Task;

namespace Dashboard.Services.Mappings.Profiles.Task;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskPostRequestDto, TaskModel>();

        CreateMap<TaskUpdateRequestDto, TaskModel>();
    }
}