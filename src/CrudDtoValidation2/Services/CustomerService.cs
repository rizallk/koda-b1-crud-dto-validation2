using CrudDtoValidation2.DTOs;
using CrudDtoValidation2.Models;
using CrudDtoValidation2.Repositories;

namespace CrudDtoValidation2.Services;

public sealed class CustomerService(ICustomerRepository repository) : ICustomerService
{
  private readonly ICustomerRepository _repository = repository;

  public async Task<CustomerResponse?> CreateAsync(CustomerRequest req, CancellationToken cancellationToken)
  {
    var customer = new Customer(req.CustomerId, req.Name, req.Email, req.Phone, req.Address);

    var created = await _repository.TryAddAsync(customer, cancellationToken);

    return created ? CustomerResponse.FromModel(customer) : null;
  }

  public Task<bool> DeleteAsync(int customerId, CancellationToken cancellationToken)
  {
    if (customerId <= 0)
    {
      return Task.FromResult(false);
    }
    return _repository.TryDeleteAsync(customerId, cancellationToken);
  }

  public async Task<IReadOnlyList<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken)
  {
    var customers = await _repository.GetAllAsync(cancellationToken);

    return customers.Select(CustomerResponse.FromModel).ToArray();
  }

  public async Task<IReadOnlyList<CustomerProductsResponse>> GetAllCustomerProductsAsync(CancellationToken cancellationToken)
  {
    var customers = await _repository.GetAllCustomerProductsAsync(cancellationToken);

    return customers;
  }

  public async Task<CustomerResponse?> GetByIdAsync(int customerId, CancellationToken cancellationToken)
  {
    if (customerId <= 0)
    {
      return null;
    }

    var customer = await _repository.GetByIdAsync(customerId, cancellationToken);

    return customer is null ? null : CustomerResponse.FromModel(customer);
  }

  public async Task<CustomerResponse?> UpdateAsync(int customerId, UpdateCustomerRequest req, CancellationToken cancellationToken)
  {
    var customer = new Customer(customerId, req.Name, req.Email, req.Phone, req.Address);

    var updated = await _repository.TryUpdateAsync(customer, cancellationToken);

    return updated ? CustomerResponse.FromModel(customer) : null;
  }
}