using CleanArchitecture.OrderManagement.Application.Abstractions.Persistence;
using CleanArchitecture.OrderManagement.Application.Orders.Queries.GetOrders;
using CleanArchitecture.OrderManagement.Domain.Models;
using FluentAssertions;
using Moq;

namespace CleanArchitecture.OrderManagement.Test.Application.Orders.Queries;

public class GetOrdersQueryHandlerTests
{
    private readonly Mock<IOrderRepository> _repositoryMock;
    private readonly GetOrdersQueryHandler _handler;

    public GetOrdersQueryHandlerTests()
    {
        _repositoryMock = new Mock<IOrderRepository>();

        _handler = new GetOrdersQueryHandler(
            _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_All_Orders()
    {
        // Arrange
        var orders = new List<Order>
        {
            new(Guid.NewGuid()),
            new(Guid.NewGuid())
        };

        _repositoryMock
            .Setup(x => x.GetAllAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(orders);

        // Act
        var result = await _handler.Handle(
            new GetOrdersQuery(),
            CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_Orders_Exist()
    {
        // Arrange
        _repositoryMock
            .Setup(x => x.GetAllAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<Order>());

        // Act
        var result = await _handler.Handle(
            new GetOrdersQuery(),
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}