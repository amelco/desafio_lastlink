IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' and xtype='U')
CREATE TABLE 'Products' (
    Id INT PRIMARY KEY IDENTITY(1, 1),
    [Name] NVARCHAR(256) NOT NULL,
    Category NVARCHAR(256) NOT NULL,
    UnitCost DECIMAL(12, 2) NOT NULL,
    CreatedAt DATETIME NOT NULL
);