using CleanArchitecture.OrderManagement.Application.Abstractions.Persistence;
using CleanArchitecture.OrderManagement.Application.Orders.Commands.CreateOrder;
using CleanArchitecture.OrderManagement.Domain.Models;
using FluentAssertions;
using Moq;

namespace CleanArchitecture.OrderManagement.Test.Application.Orders.Commands;

public class CreateOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _repositoryMock;
    private readonly CreateOrderCommandHandler _handler;

    public CreateOrderCommandHandlerTests()
    {
        _repositoryMock = new Mock<IOrderRepository>();

        _handler = new CreateOrderCommandHandler(
            _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Order_When_Command_Is_Valid()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var command = new CreateOrderCommand(
            customerId,
            new List<CreateOrderItemRequest>
            {
                new("Laptop", 1, 1500m),
                new("Mouse", 2, 50m)
            });

        Order? capturedOrder = null;

        _repositoryMock
            .Setup(x => x.AddAsync(
                It.IsAny<Order>(),
                It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>(
                (order, _) => capturedOrder = order);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        capturedOrder.Should().NotBeNull();
        capturedOrder!.CustomerId.Should().Be(customerId);
        capturedOrder.OrderItems.Should().HaveCount(2);

        _repositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Order>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _repositoryMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_Items_Are_Empty()
    {
        // Arrange
        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            Array.Empty<CreateOrderItemRequest>());

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be("Order.EmptyItems");

        _repositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Order>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _repositoryMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_Item_Quantity_Is_Invalid()
    {
        // Arrange
        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            new List<CreateOrderItemRequest>
            {
                new("Laptop", 0, 1500m)
            });

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be("Order.InvalidItem");

        _repositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Order>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}