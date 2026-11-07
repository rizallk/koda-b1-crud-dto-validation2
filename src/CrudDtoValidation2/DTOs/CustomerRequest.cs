using System.ComponentModel.DataAnnotations;

namespace CrudDtoValidation2.DTOs;

public sealed record CustomerRequest(
  [Required]
  int CustomerId,

  [Required, StringLength(80)]
  string Name,

  [Required, StringLength(80)]
  string Email,

  string Phone,
  string Address
);