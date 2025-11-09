using Core.Interfaces;
using Infra;
using Microsoft.EntityFrameworkCore;

namespace Api
{
    public static class AppExtensions
    {
        public static async void CreateTables(this WebApplication app)
        {
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
        }

        public static async void CreateQueues(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var rabbit = scope.ServiceProvider.GetRequiredService<IProductPublisher>();
                await rabbit.Publish("");
            }
        }
    }
}
