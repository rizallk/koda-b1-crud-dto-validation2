using CrudDtoValidation2.DTOs;

namespace CrudDtoValidation2.Services;

public interface IProductService
{
  Task<IReadOnlyList<ProductResponse>> GetAllAsync(CancellationToken cancellationToken);
  Task<ProductResponse?> GetByCodeAsync(string productCode, CancellationToken cancellationToken);
  Task<ProductResponse?> CreateAsync(ProductRequest request, CancellationToken cancellationToken);
  Task<ProductResponse?> UpdateAsync(string productCode, UpdateProductRequest request, CancellationToken cancellationToken);

  Task<bool> DeleteAsync(string productCode, CancellationToken cancellationToken);
}