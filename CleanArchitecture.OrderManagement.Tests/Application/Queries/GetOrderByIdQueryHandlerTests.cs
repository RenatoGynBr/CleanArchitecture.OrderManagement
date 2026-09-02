using CleanArchitecture.OrderManagement.Application.Abstractions.Persistence;
using CleanArchitecture.OrderManagement.Application.Orders.Queries.GetOrderById;
using CleanArchitecture.OrderManagement.Domain.Enums;
using CleanArchitecture.OrderManagement.Domain.Models;
using FluentAssertions;
using Moq;

namespace CleanArchitecture.OrderManagement.Test.Application.Orders.Queries;

public class GetOrderByIdQueryHandlerTests
{
    private readonly Mock<IOrderRepository> _repositoryMock;
    private readonly GetOrderByIdQueryHandler _handler;

    public GetOrderByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<IOrderRepository>();

        _handler = new GetOrderByIdQueryHandler(
            _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_NotFound_When_Order_Does_Not_Exist()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                orderId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order?)null);

        // Act
        var result = await _handler.Handle(
            new GetOrderByIdQuery(orderId),
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be("Order.NotFound");
    }
}