using Dashboard.Core.Repositories;
using Dashboard.Api.Configurations;
using Dashboard.Core.Models;
using Dashboard.DataAccess.Repositories;
using Dashboard.Services.Container.TaskContainer;
using Dashboard.Services.Container.TokenContainer;
using Dashboard.Services.Container.UserContainer;
using Dashboard.Services.Mappings;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add logging providers
builder.Logging.ClearProviders();
var logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.AddSerilog(logger);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{

    options.SwaggerDoc("v1",
    new OpenApiInfo
    {
        Title = "Dashboard API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Add JWT configuration
builder.Services.AddJwtConfiguration(builder.Configuration);

// Configuration
builder.Services.AddMapConfiguration();

// Migration configuration
builder.Services.AddDBConfiguration(builder.Configuration);
// Service conatainer registration
builder.Services.AddScoped<ICrudRepository<TaskModel>, TaskRepository>();
builder.Services.AddScoped<ICrudRepository<UserModel>, UserRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<TokenService>();

// Add CORS configuration
builder.Services.AddCors(options =>
    options.AddPolicy("CorsPolicy", build =>
        build.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(builder.Configuration["DashboardWeb"])));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Database configuration
app.UseDBScopeConfiguration();

app.Run();
