using CrudDtoValidation2.DTOs;
using CrudDtoValidation2.Models;
using CrudDtoValidation2.Services;
using Microsoft.AspNetCore.Mvc;

namespace CrudDtoValidation2.Controllers;

[ApiController]
[Route("api/v1/customers")]

public sealed class CustomerController(ICustomerService service) : ControllerBase
{
  private readonly ICustomerService _service = service;

  [HttpGet]
  [Route("products")]
  [ProducesResponseType(typeof(IReadOnlyList<CustomerResponse>), StatusCodes.Status200OK)]
  public async Task<ActionResult<IReadOnlyList<Customer>>> GetAllCustomerProducts(CancellationToken cancellationToken)
  {
    var customers = await _service.GetAllCustomerProductsAsync(cancellationToken);
    return Ok(customers);
  }

  [HttpGet]
  [ProducesResponseType(typeof(IReadOnlyList<CustomerResponse>), StatusCodes.Status200OK)]
  public async Task<ActionResult<IReadOnlyList<Customer>>> GetAll(CancellationToken cancellationToken)
  {
    var customers = await _service.GetAllAsync(cancellationToken);
    return Ok(customers);
  }

  [HttpGet("{customerId}")]
  [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  public async Task<ActionResult<IReadOnlyList<Customer>>> GetByCode([FromRoute] int customerId, CancellationToken cancellationToken)
  {
    var customer = await _service.GetByIdAsync(customerId, cancellationToken);

    return customer is null ? Problem(statusCode: StatusCodes.Status404NotFound, title: "Customer not found") : Ok(customer);
  }

  [HttpPost]
  [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  public async Task<ActionResult<CustomerResponse>> CreateCustomer([FromBody] CustomerRequest req, CancellationToken cancellationToken)
  {
    try
    {
      var created = await _service.CreateAsync(req, cancellationToken);

      if (created is null)
      {
        return Problem(statusCode: StatusCodes.Status409Conflict, title: "Customer already exists");
      }

      return CreatedAtAction(actionName: nameof(GetByCode), routeValues: new { customerId = created.CustomerId }, value: created);
    }
    catch (ArgumentException e)
    {
      return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Invalid customer data!", detail: e.Message);
    }

  }

  [HttpPut("{customerId}")]
  [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  public async Task<ActionResult<CustomerResponse>> UpdateCustomer([FromRoute] int customerId, [FromBody] UpdateCustomerRequest req, CancellationToken cancellationToken)
  {
    try
    {
      var updated = await _service.UpdateAsync(customerId, req, cancellationToken);

      return updated is null ? Problem(statusCode: StatusCodes.Status404NotFound, title: "Customer not found", detail: $"No customer was found for code {customerId}.") : Ok(updated);
    }
    catch (ArgumentException e)
    {
      return Problem(
             statusCode: StatusCodes.Status400BadRequest,
             title: "Invalid customer data!",
             detail: e.Message
           );
    }
  }

  [HttpDelete("{customerId}")]
  [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Delete([FromRoute] int customerId, CancellationToken cancellationToken)
  {
    var deleted = await _service.DeleteAsync(customerId, cancellationToken);

    return deleted ? NoContent() : Problem(statusCode: StatusCodes.Status404NotFound, title: "Customer not found", detail: $"No customer was found for code {customerId}.");
  }
}