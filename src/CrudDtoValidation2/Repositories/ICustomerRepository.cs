using CrudDtoValidation2.DTOs;
using CrudDtoValidation2.Models;

namespace CrudDtoValidation2.Repositories;

public interface ICustomerRepository
{
  Task<IReadOnlyList<CustomerProductsResponse>> GetAllCustomerProductsAsync(CancellationToken cancellationToken);
  Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken);
  Task<Customer?> GetByIdAsync(int customerId, CancellationToken cancellationToken);
  Task<bool> TryAddAsync(Customer customer, CancellationToken cancellationToken);
  Task<bool> TryUpdateAsync(Customer customer, CancellationToken cancellationToken);
  Task<bool> TryDeleteAsync(int customerId, CancellationToken cancellationToken);
}