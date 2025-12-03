using Microsoft.EntityFrameworkCore;
using Talos.Server.Application.RealTime;
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

// SIGNALR
builder.Services.AddSignalR();

// Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Talos_";
});

// INYECCION
builder.Services.AddScoped<NotificationService>();

//debloquear conexion con cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true);
    });
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

app.MapHub<NotificationsHub>("/hubs/notifications");

//prueba de signalr con el html
app.UseStaticFiles();

//habilitar cors
app.UseCors();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();