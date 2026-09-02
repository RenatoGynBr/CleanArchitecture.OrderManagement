using CleanArchitecture.OrderManagement.Application.Abstractions.Persistence;
using CleanArchitecture.OrderManagement.Domain.Enums;
using FluentAssertions;
using Moq;

namespace CleanArchitecture.OrderManagement.Test.Application.Orders.Commands;

public class ConfirmOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _repositoryMock;
    private readonly ConfirmOrderCommandHandler _handler;

    public ConfirmOrderCommandHandlerTests()
    {
        _repositoryMock = new Mock<IOrderRepository>();

        _handler = new ConfirmOrderCommandHandler(
            _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Confirm_Order_When_Order_Is_Pending()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                order.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        // Act
        var result = await _handler.Handle(
            new ConfirmOrderCommand(order.Id),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        order.Status.Should().Be(StatusType.Confirmed);

        _repositoryMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
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
            new ConfirmOrderCommand(orderId),
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be("Order.NotFound");

        _repositoryMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_Order_Is_Not_Pending()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());

        order.Confirm();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                order.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        // Act
        var result = await _handler.Handle(
            new ConfirmOrderCommand(order.Id),
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be("Order.InvalidStatus");

        _repositoryMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}