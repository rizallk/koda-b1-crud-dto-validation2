using CrudDtoValidation2.DTOs;

namespace CrudDtoValidation2.DTOs;

public class CustomerProductsResponse
{
  public int CustomerId { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;

  public List<ProductResponse> Products { get; set; } = [];
}