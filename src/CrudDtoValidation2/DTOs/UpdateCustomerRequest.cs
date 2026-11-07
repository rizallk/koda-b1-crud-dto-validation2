namespace CrudDtoValidation2.DTOs;

public record UpdateCustomerRequest(int CustomerId, string Name, string Email, string Phone, string Address);