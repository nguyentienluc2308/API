using CourseCatalog.Api.Data;
using CourseCatalog.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure CORS to connect easily to VueJS frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure PostgreSQL Database Context
builder.Services.AddDbContext<CourseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register DI services
builder.Services.AddScoped<IEventPublisher, EventPublisher>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Course Catalog Core API",
        Version = "v1",
        Description = "Core API for course, class, and schedule management with conflict detection."
    });
});

var app = builder.Build();

// Auto migrate and seed database on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CourseDbContext>();
        await DbInitializer.InitializeAsync(context);
        app.Logger.LogInformation("Database initialized and seeded successfully.");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while initializing the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || true) // Enable Swagger in production/testing as well to make it easy to inspect
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Course Catalog Core API v1");
        c.RoutePrefix = string.Empty; // Serve swagger at the root (http://localhost:port/)
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
