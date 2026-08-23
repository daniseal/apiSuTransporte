using apiSuTransporte.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controladores API
builder.Services.AddControllers();

// Conexion con SQL Server
builder.Services.AddDbContext<BdSuTransporteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));

// CORS para permitir consumir la API desde el proyecto HTML
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReglasCors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("ReglasCors");

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();