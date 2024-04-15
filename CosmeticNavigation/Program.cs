using ComponentsStore.Application.Service;
using ComponentsStore.DataAccess;
using ComponentsStore.DataAccess.Repositories;
using CosmeticsStore.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ComponentsStoreDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(ComponentsStoreDbContext)));
    }
    );
builder.Services.AddScoped<IComponentsService, ComponentsService>();
builder.Services.AddScoped<IComponentsRepository, ComponentsRepository>();
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
