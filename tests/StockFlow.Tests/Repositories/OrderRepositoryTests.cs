using StockFlow.Models;
using StockFlow.Repositories;
using StockFlow.Tests.Helpers;

namespace StockFlow.Tests.Repositories;

public class OrderRepositoryTests
{
    [Fact]
    public void AddOrder_ValidOrder_ReturnsGenerateOrderId()
    {
        //Arrange
        using TestDatabase testDatabase = new TestDatabase();

        OrderRepository repository = new OrderRepository(testDatabase.DatabaseConnectionService);

        Order order = new Order()
        {
            OrderNumber = "TEST-ORD-001",
            OrderDate = DateTime.Now,
            TotalAmount = 1500.00m,
            OrderStatus = "Pending Payment",
            PaymentStatus = "Unpaid"
        };

        //Act
        int orderId = repository.AddOrder(order);

        //Assert
        Assert.True(orderId > 0);
    }

    [Fact]
    public void AddOrder_ValidOrder_PersistsOrder()
    {
        // Arrange
        using TestDatabase testDatabase =
            new TestDatabase();

        OrderRepository repository =
            new OrderRepository(
                testDatabase.DatabaseConnectionService
            );

        Order order = new Order
        {
            OrderNumber = "TEST-ORD-002",
            OrderDate = DateTime.Now,
            TotalAmount = 2250.00m,
            OrderStatus = "Pending Payment",
            PaymentStatus = "Unpaid"
        };

        // Act
        int orderId = repository.AddOrder(order);

        Order? savedOrder =
            repository.FindOrderByNumber(
                "TEST-ORD-002"
            );

        // Assert
        Assert.NotNull(savedOrder);

        Assert.Equal(
            orderId,
            savedOrder.OrderId
        );

        Assert.Equal(
            "TEST-ORD-002",
            savedOrder.OrderNumber
        );

        Assert.Equal(
            2250.00m,
            savedOrder.TotalAmount
        );

        Assert.Equal(
            "Pending Payment",
            savedOrder.OrderStatus
        );

        Assert.Equal(
            "Unpaid",
            savedOrder.PaymentStatus
        );
    }

    [Fact]
    public void FindOrderByNumber_UnknownOrder_ReturnsNull()
    {
        // Arrange
        using TestDatabase testDatabase =
            new TestDatabase();

        OrderRepository repository =
            new OrderRepository(
                testDatabase.DatabaseConnectionService
            );

        // Act
        Order? order =
            repository.FindOrderByNumber(
                "DOES-NOT-EXIST"
            );

        // Assert
        Assert.Null(order);
    }

    [Fact]
    public void GetAllOrders_AddedOrder_IncludesOrder()
    {
        // Arrange
        using TestDatabase testDatabase =
            new TestDatabase();

        OrderRepository repository =
            new OrderRepository(
                testDatabase.DatabaseConnectionService
            );

        Order order = new Order
        {
            OrderNumber = "TEST-ORD-003",
            OrderDate = DateTime.Now,
            TotalAmount = 500.00m,
            OrderStatus = "Pending Payment",
            PaymentStatus = "Unpaid"
        };

        repository.AddOrder(order);

        // Act
        List<Order> orders =
            repository.GetAllOrders();

        // Assert
        Assert.Contains(
            orders,
            order =>
                order.OrderNumber == "TEST-ORD-003"
        );
    }

    [Fact]
    public void UpdateOrderStatus_ExistingOrder_PersistsStatus()
    {
        // Arrange
        using TestDatabase testDatabase =
            new TestDatabase();

        OrderRepository repository =
            new OrderRepository(
                testDatabase.DatabaseConnectionService
            );

        Order order = new Order
        {
            OrderNumber = "TEST-ORD-004",
            OrderDate = DateTime.Now,
            TotalAmount = 900.00m,
            OrderStatus = "Pending Payment",
            PaymentStatus = "Unpaid"
        };

        repository.AddOrder(order);

        // Act
        repository.UpdateOrderStatus(
            "TEST-ORD-004",
            "Completed"
        );

        Order? updatedOrder =
            repository.FindOrderByNumber(
                "TEST-ORD-004"
            );

        // Assert
        Assert.NotNull(updatedOrder);

        Assert.Equal(
            "Completed",
            updatedOrder.OrderStatus
        );
    }

    [Fact]
    public void UpdatePaymentStatus_ExistingOrder_PersistsPaymentStatus()
    {
        // Arrange
        using TestDatabase testDatabase =
            new TestDatabase();

        OrderRepository repository =
            new OrderRepository(
                testDatabase.DatabaseConnectionService
            );

        Order order = new Order
        {
            OrderNumber = "TEST-ORD-005",
            OrderDate = DateTime.Now,
            TotalAmount = 1250.00m,
            OrderStatus = "Pending Payment",
            PaymentStatus = "Unpaid"
        };

        repository.AddOrder(order);

        // Act
        repository.UpdatePaymentStatus(
            "TEST-ORD-005",
            "Paid"
        );

        Order? updatedOrder =
            repository.FindOrderByNumber(
                "TEST-ORD-005"
            );

        // Assert
        Assert.NotNull(updatedOrder);

        Assert.Equal(
            "Paid",
            updatedOrder.PaymentStatus
        );
    }

}