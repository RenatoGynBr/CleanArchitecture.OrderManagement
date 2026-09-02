namespace CleanArchitecture.OrderManagement.Application.Common;

public static class OrderErrors
{
    public const string NotFoundCode = "Order.NotFound";

    public const string InvalidStatusCode = "Order.InvalidStatus";

    public const string EmptyItemsCode = "Order.EmptyItems";

    public const string InvalidQuantityCode = "Order.InvalidQuantity";


    public static Error NotFound(Guid id) =>
        new(
            "Order.NotFound",
            $"Order '{id}' was not found.");

    public static readonly Error InvalidStatus =
        new(
            "Order.InvalidStatus",
            "Only pending orders can be confirmed.");
    public static readonly Error EmptyItems =
        new(
            "Order.EmptyItems",
            "An order must contain at least one item.");
}