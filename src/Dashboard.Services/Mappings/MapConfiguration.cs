using Dashboard.Services.Mappings.Profiles.Task;
using Dashboard.Services.Mappings.Profiles.User;
using Microsoft.Extensions.DependencyInjection;

namespace Dashboard.Services.Mappings;

public static class MappersConfiguration
{
    public static void AddMapConfiguration(this IServiceCollection service)
    {
        service.AddAutoMapper(typeof(UserProfile).Assembly);
        service.AddAutoMapper(typeof(TaskProfile).Assembly);
    }
}