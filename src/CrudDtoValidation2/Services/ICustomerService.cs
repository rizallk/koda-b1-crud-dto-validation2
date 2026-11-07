using CrudDtoValidation2.DTOs;
using CrudDtoValidation2.Models;

namespace CrudDtoValidation2.Services;

public interface ICustomerService
{
  Task<IReadOnlyList<CustomerProductsResponse>> GetAllCustomerProductsAsync(CancellationToken cancellationToken);
  Task<IReadOnlyList<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken);
  Task<CustomerResponse?> GetByIdAsync(int customerId, CancellationToken cancellationToken);
  Task<CustomerResponse?> CreateAsync(CustomerRequest request, CancellationToken cancellationToken);
  Task<CustomerResponse?> UpdateAsync(int customerId, UpdateCustomerRequest request, CancellationToken cancellationToken);

  Task<bool> DeleteAsync(int customerId, CancellationToken cancellationToken);
}