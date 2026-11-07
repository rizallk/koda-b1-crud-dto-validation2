namespace CrudDtoValidation2.Models;

public sealed class Customer
{
  public int CustomerId { get; }
  public string Name { get; }
  public string Email { get; }
  public string? Phone { get; }
  public string? Address { get; }

  public Customer(int customerId, string name, string email, string? phone, string? address)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      throw new ArgumentException("Nama wajib diisi", nameof(name));
    }
    if (string.IsNullOrWhiteSpace(email))
    {
      throw new ArgumentException("Email wajib diisi", nameof(email));
    }

    CustomerId = customerId;
    Name = name;
    Email = email;
    Phone = phone;
    Address = address;
  }
}