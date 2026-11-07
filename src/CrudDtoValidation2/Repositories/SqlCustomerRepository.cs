using System.Data;
using CrudDtoValidation2.DTOs;
using CrudDtoValidation2.Models;
using Microsoft.Data.SqlClient;

namespace CrudDtoValidation2.Repositories;

public sealed class SqlCustomerRepository(SqlConnection connection) : ICustomerRepository
{
  public async Task<IReadOnlyList<CustomerProductsResponse>> GetAllCustomerProductsAsync(CancellationToken cancellationToken)
  {
    const string sql = """
      SELECT 
        c.CustomerId, 
        c.Name AS CustomerName, 
        c.Email, 
        p.ProductId, 
        p.ProductCode, 
        p.Name AS ProductName, 
        p.Price,
        p.Stock,               
        p.Category            
      FROM dbo.Customers c
      LEFT JOIN dbo.Product p ON c.CustomerId = p.CustomerId
      ORDER BY c.CustomerId DESC;
    """;

    var customers = new List<Customer>();

    if (connection.State != ConnectionState.Open) await connection.OpenAsync(cancellationToken);

    using var command = new SqlCommand(sql, connection);
    using var reader = await command.ExecuteReaderAsync(cancellationToken);

    var customerDictionary = new Dictionary<int, CustomerProductsResponse>();

    while (await reader.ReadAsync(cancellationToken))
    {
      int customerId = reader.GetInt32(0);

      if (!customerDictionary.TryGetValue(customerId, out var customerDto))
      {
        customerDto = new CustomerProductsResponse
        {
          CustomerId = customerId,
          Name = reader.GetString(1),
          Email = reader.GetString(2),
          Products = []
        };
        customerDictionary.Add(customerId, customerDto);
      }

      if (!reader.IsDBNull(3))
      {
        var productDto = new ProductResponse(
          ProductCode: reader.GetString(4),
          Name: reader.GetString(5),
          Price: reader.GetDecimal(6),
          Stock: reader.GetInt32(7),
          Category: reader.GetString(8)
        );

        customerDto.Products.Add(productDto);
      }
    }

    // Kembalikan hanya values-nya saja sebagai List
    return customerDictionary.Values.ToList();
  }

  public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken)
  {
    const string sql = """
      SELECT CustomerId, Name, Email, Phone, Address FROM dbo.Customers ORDER BY CustomerId DESC;
    """;

    var customers = new List<Customer>();

    if (connection.State != ConnectionState.Open) await connection.OpenAsync(cancellationToken);

    using var command = new SqlCommand(sql, connection);
    using var reader = await command.ExecuteReaderAsync(cancellationToken);

    while (await reader.ReadAsync(cancellationToken))
    {
      customers.Add(ReadCustomer(reader));
    }

    return customers;
  }

  public async Task<Customer?> GetByIdAsync(int customerId, CancellationToken cancellationToken)
  {
    const string sql = """
      SELECT CustomerId, Name, Email, Phone, Address FROM dbo.Customers WHERE CustomerId = @customerId;
    """;

    if (connection.State != ConnectionState.Open) await connection.OpenAsync(cancellationToken);

    await using var command = new SqlCommand(sql, connection);

    AddCustomerId(command, customerId);

    await using var reader = await command.ExecuteReaderAsync(cancellationToken);

    if (!await reader.ReadAsync(cancellationToken))
    {
      return null;
    }

    return ReadCustomer(reader);
  }

  public async Task<bool> TryAddAsync(Customer customer, CancellationToken cancellationToken)
  {
    const string sql = """
      INSERT INTO dbo.Customers(Name, Email, Phone, Address) VALUES (@customerId, @name, @email, @phone, @address);
    """;

    if (connection.State != ConnectionState.Open) await connection.OpenAsync(cancellationToken);

    await using var command = new SqlCommand(sql, connection);

    AddCustomerParameter(command, customer);

    try
    {
      var affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);

      return affectedRows == 1;
    }
    catch (SqlException e) when (e.Number is 2601 or 2627)
    {
      return false;
    }
    catch (SqlException e)
    {
      Console.WriteLine($"SQL Error tidak terduga: {e.Number} - {e.Message}");
      return false;
    }
  }

  public async Task<bool> TryDeleteAsync(int customerId, CancellationToken cancellationToken)
  {
    const string sql = """
      DELETE FROM dbo.Customers WHERE CustomerId = @customerId;
    """;

    if (connection.State != ConnectionState.Open) await connection.OpenAsync(cancellationToken);

    await using var command = new SqlCommand(sql, connection);

    AddCustomerId(command, customerId);

    var affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);

    return affectedRows == 1;
  }

  public async Task<bool> TryUpdateAsync(Customer customer, CancellationToken cancellationToken)
  {
    const string sql = """
      UPDATE dbo.Customers SET
        Name = @name,
        Email = @email,
        Phone = @phone,
        Address = @address
      WHERE CustomerId = @customerId;
    """;

    if (connection.State != ConnectionState.Open) await connection.OpenAsync(cancellationToken);

    await using var command = new SqlCommand(sql, connection);

    AddCustomerParameter(command, customer);

    var affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);

    return affectedRows == 1;
  }

  // ============= METHOD

  private static void AddCustomerParameter(SqlCommand command, Customer customer)
  {
    AddCustomerId(command, customer.CustomerId);

    command.Parameters.Add("@name", SqlDbType.NVarChar, 80).Value = customer.Name;

    command.Parameters.Add("@email", SqlDbType.NVarChar, 80).Value = customer.Email;

    command.Parameters.Add("@phone", SqlDbType.NVarChar, 20).Value = customer.Phone;

    command.Parameters.Add("@address", SqlDbType.NVarChar, 255).Value = customer.Address;
  }

  private static void AddCustomerId(SqlCommand command, int customerId)
  {
    command.Parameters.Add("@customerId", SqlDbType.Int).Value = customerId;
  }

  private static Customer ReadCustomer(SqlDataReader reader)
  {
    return new Customer(
      reader.GetInt32(0),
      reader.GetString(1),
      reader.GetString(2),
      reader.IsDBNull(3) ? null : reader.GetString(3),
      reader.IsDBNull(4) ? null : reader.GetString(4)
    );
  }
}