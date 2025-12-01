using Microsoft.EntityFrameworkCore;
using Talos.Shared.Data;

var builder = WebApplication.CreateBuilder(args);


// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
// AUTOMAPPER
builder.Services.AddAutoMapper(typeof(Program));


// Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Talos_";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Talos API V1");
    c.RoutePrefix = string.Empty; 
});


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();