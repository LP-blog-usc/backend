using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Cargar las variables del archivo .env
DotNetEnv.Env.Load();

// Construir la cadena de conexión
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
connectionString = connectionString
            .Replace("{DB_SERVER}", Environment.GetEnvironmentVariable("DB_SERVER"))
            .Replace("{DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT"))
            .Replace("{DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME"))
            .Replace("{DB_USER}", Environment.GetEnvironmentVariable("DB_USER"))
            .Replace("{DB_PASS}", Environment.GetEnvironmentVariable("DB_PASS"));

// Configurar la conexión con la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString,
                     new MySqlServerVersion(new Version(8, 0, 30)),
                      mysqlOptions => mysqlOptions.EnableRetryOnFailure()));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
