Select name from sys.databases;

create database ecommerce;

select DB_NAME() as CurrentDatabase;

create table dbo.Product (
  ProductId INT IDENTITY(1,1) PRIMARY KEY,
  ProductCode NVARCHAR(20) NOT NULL UNIQUE,
  Name NVARCHAR(100) NOT NULL,
  Price DECIMAL(18,2) NOT NULL,
  Stock INT NOT NULL DEFAULT 0,
  Category NVARCHAR(50) NOT NULL,
  IsActive BIT NOT NULL DEFAULT 1,

  CONSTRAINT CK_Products_Price CHECK(Price >= 0),
  CONSTRAINT CK_Products_Stock CHECK(Stock >= 0)
)

select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE';
select name from sys.tables;

INSERT INTO dbo.Product (ProductCode, Name, Price, Stock, Category) VALUES
('PR001', 'Keyboard', 28000, 10, 'Elektronik'),
('PR002', 'Mouse', 10000, 48, 'Elektrik');

UPDATE dbo.Product SET Price = 5600 WHERE ProductCode = 'PR001';

select * from dbo.Product;

update dbo.Product set CustomerId = 1 WHERE ProductId = 2;
update dbo.Product set CustomerId = 2 WHERE ProductId = 3;
GO

-- ================ CUSTOMERS
create table dbo.Customers (
  CustomerId INT IDENTITY(1,1) PRIMARY KEY,
  Name NVARCHAR(80) NOT NULL,
  Email NVARCHAR(80) NOT NULL UNIQUE,
  Phone NVARCHAR(20) UNIQUE,
  Address NVARCHAR(255),
);

alter table dbo.Product 
add CustomerId INT NULL;

alter table dbo.Product 
ADD CONSTRAINT FK_product_customers
FOREIGN KEY (CustomerId)
REFERENCES dbo.Customers (CustomerId);

INSERT INTO dbo.Customers (Name, Email, Phone, Address) VALUES
('Andi', 'andi@gmail.com', '08123456789', 'Cibubur'),
('Budi', 'budi@gmail.com', '08321654987', 'Cileungsi');

select * from dbo.Customers;