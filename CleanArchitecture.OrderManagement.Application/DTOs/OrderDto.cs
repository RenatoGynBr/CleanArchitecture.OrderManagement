using CleanArchitecture.OrderManagement.Domain.Enums;

namespace CleanArchitecture.OrderManagement.Application.DTOs;

public record OrderDto(
    Guid Id,
    Guid CustomerId,
    StatusType Status,
    DateTime CreatedAt,
    IReadOnlyCollection<OrderItemDto> Items,
    decimal Total);