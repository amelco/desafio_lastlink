using Api;
using Core.Interfaces;
using Infra;
using Infra.Consumers;
using Infra.EventHandler;
using Infra.Publishers;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductEventRepository, ProductEventRepository>();
builder.Services.AddScoped<IProductPublisher, ProductPublisher>();
builder.Services.AddScoped<IProductEventHandler, ProductEventHandler>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<ProductConsumerWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.CreateTables();
app.CreateQueues();


app.Run();
