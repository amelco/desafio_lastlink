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

// TODO: maybe put in a separate file
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
  
    dbContext.Database.EnsureCreated();
  
    await dbContext.Database.ExecuteSqlRawAsync(@"
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' and xtype='U')
        CREATE TABLE Products (
            Id          INT             PRIMARY KEY IDENTITY(1, 1),
            [Name]      NVARCHAR(256)   NOT NULL,
            Category    NVARCHAR(256)   NOT NULL,
            UnitCost    DECIMAL(12, 2)  NOT NULL,
            CreatedAt   DATETIME        NOT NULL,
            IsDeleted   BIT             NOT NULL DEFAULT 0
        )");

    await dbContext.Database.ExecuteSqlRawAsync(@"
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ProductEvents' and xtype='U')
        CREATE TABLE ProductEvents (
            Id        INT           PRIMARY KEY IDENTITY(1, 1),
            Type      NVARCHAR(256) NOT NULL,
            ProductId INT           NOT NULL,
            CreatedAt DATETIME      NOT NULL,
        FOREIGN KEY (ProductId) REFERENCES Products(Id)
        );");
}

// TODO: maybe put in a separate file
using (var scope = app.Services.CreateScope())
{
    var rabbit = scope.ServiceProvider.GetRequiredService<IProductPublisher>();
    await rabbit.Publish("");
}

app.Run();
