using Microsoft.EntityFrameworkCore;
using TodoListAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<TodoContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

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
