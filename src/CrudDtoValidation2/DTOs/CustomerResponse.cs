using CrudDtoValidation2.Models;

namespace CrudDtoValidation2.DTOs;

public sealed record CustomerResponse(int CustomerId, string Name, string Email, string? Phone, string? Address)
{
  public static CustomerResponse FromModel(Customer customer) => new(customer.CustomerId, customer.Name, customer.Email, customer.Phone, customer.Address);
}