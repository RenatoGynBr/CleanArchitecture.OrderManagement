using CleanArchitecture.OrderManagement.Application.Orders.Commands.ConfirmOrder;
using CleanArchitecture.OrderManagement.Application.Orders.Commands.CreateOrder;
using CleanArchitecture.OrderManagement.Application.Orders.Queries.GetOrderById;
using CleanArchitecture.OrderManagement.Application.Orders.Queries.GetOrders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.OrderManagement.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly ISender _sender;

    public OrdersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var orders =
            await _sender.Send(
                new GetOrdersQuery(),
                cancellationToken);

        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var order =
            await _sender.Send(
                new GetOrderByIdQuery(id),
                cancellationToken);

        return order is null
            ? NotFound()
            : Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            new { id });
    }

    [HttpPatch("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new ConfirmOrderCommand(id),
            cancellationToken);

        return result
            ? NoContent()
            : NotFound();
    }
}