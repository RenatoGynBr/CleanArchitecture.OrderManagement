namespace CleanArchitecture.OrderManagement.Application.DTOs;

public record OrderItemDto(
    Guid Id,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Total);