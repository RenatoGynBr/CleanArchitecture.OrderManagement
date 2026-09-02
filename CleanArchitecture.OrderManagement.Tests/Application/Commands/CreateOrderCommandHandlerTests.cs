using CleanArchitecture.OrderManagement.Application.Abstractions.Persistence;
using CleanArchitecture.OrderManagement.Application.Orders.Commands.CreateOrder;
using CleanArchitecture.OrderManagement.Domain.Enums;
using CleanArchitecture.OrderManagement.Domain.Models;
using FluentAssertions;
using Moq;

namespace CleanArchitecture.OrderManagement.Test.Application.Commands;

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
    public async Task Handle_Should_Create_Order()
    {
        // Arrange

        var customerId = Guid.NewGuid();

        var command = new CreateOrderCommand(
            customerId,
            new List<CreateOrderItemRequest>
            {
                new(
                    "Laptop",
                    1,
                    1500m),

                new(
                    "Mouse",
                    2,
                    30m)
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

        result.Should().NotBeNull();

        capturedOrder.Should().NotBeNull();

        capturedOrder!.CustomerId
            .Should()
            .Be(customerId);

        capturedOrder.Status
            .Should()
            .Be(StatusType.Pending);

        capturedOrder.OrderItems
            .Should()
            .HaveCount(2);

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
}