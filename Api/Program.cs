using Core.Interfaces;
using Infra;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

// TODO: configure dependency injection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
  
    // Ensure database is created
    dbContext.Database.EnsureCreated();
  
    // Or use raw SQL for more control
    await dbContext.Database.ExecuteSqlRawAsync(@"
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' and xtype='U')
        CREATE TABLE Products (
            Id INT PRIMARY KEY IDENTITY(1, 1),
            [Name] NVARCHAR(256) NOT NULL,
            Category NVARCHAR(256) NOT NULL,
            UnitCost DECIMAL(12, 2) NOT NULL,
            CreatedAt DATETIME NOT NULL,
            IsDeleted BIT NOT NULL DEFAULT 0
        )");
}

app.Run();
