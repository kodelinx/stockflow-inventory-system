using StockFlow.Models;
using StockFlow.Repositories;
using StockFlow.Tests.Helpers;

namespace StockFlow.Tests.Repositories;

public class OrderItemRepositoryTests
{
    [Fact]
    public void AddOrderItem_ValidItem_PersistOrderItem()
    {
        //Arrange
        using TestDatabase testDatabase = new TestDatabase();

        ProductRepository productRepository = new ProductRepository(
            testDatabase.DatabaseConnectionService
        );

        OrderRepository orderRepository = new OrderRepository(
            testDatabase.DatabaseConnectionService
        );

        OrderItemRepository orderItemRepository = new OrderItemRepository(
            testDatabase.DatabaseConnectionService
        );

        Product product = new Product
        {
            ProductCode = "TEST-P-ORDER-001",
            Name = "Test Mouse",
            Category = "Accessories",
            UnitPrice = 500.00m,
            QuantityInStock = 20,
            ReorderLevel = 5,
            IsActive = true
        };

        productRepository.AddProduct(product);

        Product? savedProduct = productRepository.FindProductByCode("TEST-P-ORDER-001");

        Assert.NotNull(savedProduct);

        Order order = new Order
        {
            OrderNumber = "TEST-ORD-ITEM-001",
            OrderDate = DateTime.Now,
            TotalAmount = 1000.00m,
            OrderStatus = "Pending Payment",
            PaymentStatus = "Unpaid"
        };
        
        int orderId = orderRepository.AddOrder(order);

        OrderItem orderItem = new OrderItem
        {
            OrderId = orderId,
            ProductId = savedProduct.ProductId,
            ProductCode = savedProduct.ProductCode,
            ProductName = savedProduct.Name,
            Quantity = 2,
            UnitPrice = 500.00m,
            LineTotal = 1000.00m
        };

        // Act
        orderItemRepository.AddOrderItem(
            orderItem
        );

        List<OrderItem> savedItems =
            orderItemRepository
                .GetOrderItemsByOrderId(
                    orderId
                );
        
        //Assert
        Assert.Single(savedItems);

        OrderItem savedItem =
        savedItems[0];

        Assert.Equal(
            orderId,
            savedItem.OrderId
        );

        Assert.Equal(
            savedProduct.ProductId,
            savedItem.ProductId
        );

        Assert.Equal(
            "TEST-P-ORDER-001",
            savedItem.ProductCode
        );

        Assert.Equal(
            "Test Mouse",
            savedItem.ProductName
        );

        Assert.Equal(
            2,
            savedItem.Quantity
        );

        Assert.Equal(
            500.00m,
            savedItem.UnitPrice
        );

        Assert.Equal(
            1000.00m,
            savedItem.LineTotal
        );
    }

    [Fact]
    public void GetOrderItemsByOrderId_MultipleItems_ReturnsItemsForOrder()
    {
        // Arrange
        using TestDatabase testDatabase =
            new TestDatabase();

        ProductRepository productRepository =
            new ProductRepository(
                testDatabase.DatabaseConnectionService
            );

        OrderRepository orderRepository =
            new OrderRepository(
                testDatabase.DatabaseConnectionService
            );

        OrderItemRepository orderItemRepository =
            new OrderItemRepository(
                testDatabase.DatabaseConnectionService
            );

        Product mouse = new Product
        {
            ProductCode = "TEST-MOUSE-001",
            Name = "Mouse",
            Category = "Accessories",
            UnitPrice = 500.00m,
            QuantityInStock = 20,
            ReorderLevel = 5,
            IsActive = true
        };

        Product keyboard = new Product
        {
            ProductCode = "TEST-KEYBOARD-001",
            Name = "Keyboard",
            Category = "Accessories",
            UnitPrice = 1000.00m,
            QuantityInStock = 10,
            ReorderLevel = 3,
            IsActive = true
        };

        productRepository.AddProduct(mouse);
        productRepository.AddProduct(keyboard);

        Product? savedMouse =
            productRepository.FindProductByCode(
                "TEST-MOUSE-001"
            );

        Product? savedKeyboard =
            productRepository.FindProductByCode(
                "TEST-KEYBOARD-001"
            );

        Assert.NotNull(savedMouse);
        Assert.NotNull(savedKeyboard);

        Order order = new Order
        {
            OrderNumber = "TEST-ORD-ITEM-002",
            OrderDate = DateTime.Now,
            TotalAmount = 2000.00m,
            OrderStatus = "Pending Payment",
            PaymentStatus = "Unpaid"
        };

        int orderId =
            orderRepository.AddOrder(order);

        OrderItem mouseItem = new OrderItem
        {
            OrderId = orderId,
            ProductId = savedMouse.ProductId,
            ProductCode = savedMouse.ProductCode,
            ProductName = savedMouse.Name,
            Quantity = 2,
            UnitPrice = 500.00m,
            LineTotal = 1000.00m
        };

        OrderItem keyboardItem = new OrderItem
        {
            OrderId = orderId,
            ProductId = savedKeyboard.ProductId,
            ProductCode = savedKeyboard.ProductCode,
            ProductName = savedKeyboard.Name,
            Quantity = 1,
            UnitPrice = 1000.00m,
            LineTotal = 1000.00m
        };

        orderItemRepository.AddOrderItem(
            mouseItem
        );

        orderItemRepository.AddOrderItem(
            keyboardItem
        );

        // Act
        List<OrderItem> items =
            orderItemRepository
                .GetOrderItemsByOrderId(
                    orderId
                );

        // Assert
        Assert.Equal(
            2,
            items.Count
        );

        Assert.Contains(
            items,
            item =>
                item.ProductCode ==
                "TEST-MOUSE-001"
        );

        Assert.Contains(
            items,
            item =>
                item.ProductCode ==
                "TEST-KEYBOARD-001"
        );

        Assert.All(
            items,
            item =>
                Assert.Equal(
                    orderId,
                    item.OrderId
                )
        );
    }

    [Fact]
    public void GetOrderItemsByOrderId_NoItems_ReturnsEmptyList()
    {
        // Arrange
        using TestDatabase testDatabase =
            new TestDatabase();

        OrderRepository orderRepository =
            new OrderRepository(
                testDatabase.DatabaseConnectionService
            );

        OrderItemRepository orderItemRepository =
            new OrderItemRepository(
                testDatabase.DatabaseConnectionService
            );

        Order order = new Order
        {
            OrderNumber = "TEST-ORD-EMPTY-001",
            OrderDate = DateTime.Now,
            TotalAmount = 0m,
            OrderStatus = "Pending Payment",
            PaymentStatus = "Unpaid"
        };

        int orderId =
            orderRepository.AddOrder(order);

        // Act
        List<OrderItem> items =
            orderItemRepository
                .GetOrderItemsByOrderId(
                    orderId
                );

        // Assert
        Assert.Empty(items);
    }

    [Fact]
    public void GetOrderItemsByOrderId_TwoOrders_ReturnsOnlyRequestedOrderItems()
    {
        // Arrange
        using TestDatabase testDatabase =
            new TestDatabase();

        ProductRepository productRepository =
            new ProductRepository(
                testDatabase.DatabaseConnectionService
            );

        OrderRepository orderRepository =
            new OrderRepository(
                testDatabase.DatabaseConnectionService
            );

        OrderItemRepository orderItemRepository =
            new OrderItemRepository(
                testDatabase.DatabaseConnectionService
            );

        // Create first Product
        Product mouse = new Product
        {
            ProductCode = "TEST-MOUSE-A",
            Name = "Test Mouse",
            Category = "Accessories",
            UnitPrice = 500.00m,
            QuantityInStock = 10,
            ReorderLevel = 2,
            IsActive = true
        };

        // Create second Product
        Product keyboard = new Product
        {
            ProductCode = "TEST-KEYBOARD-B",
            Name = "Test Keyboard",
            Category = "Accessories",
            UnitPrice = 1000.00m,
            QuantityInStock = 10,
            ReorderLevel = 2,
            IsActive = true
        };

        productRepository.AddProduct(mouse);
        productRepository.AddProduct(keyboard);

        Product? savedMouse =
            productRepository.FindProductByCode(
                "TEST-MOUSE-A"
            );

        Product? savedKeyboard =
            productRepository.FindProductByCode(
                "TEST-KEYBOARD-B"
            );

        Assert.NotNull(savedMouse);
        Assert.NotNull(savedKeyboard);

        // Create Order A
        Order orderA = new Order
        {
            OrderNumber = "TEST-ORDER-A",
            OrderDate = DateTime.Now,
            TotalAmount = 500.00m,
            OrderStatus = "Pending Payment",
            PaymentStatus = "Unpaid"
        };

        // Create Order B
        Order orderB = new Order
        {
            OrderNumber = "TEST-ORDER-B",
            OrderDate = DateTime.Now,
            TotalAmount = 1000.00m,
            OrderStatus = "Pending Payment",
            PaymentStatus = "Unpaid"
        };

        int orderAId =
            orderRepository.AddOrder(orderA);

        int orderBId =
            orderRepository.AddOrder(orderB);

        // Create child for Order A
        OrderItem mouseItem = new OrderItem
        {
            OrderId = orderAId,
            ProductId = savedMouse.ProductId,
            ProductCode = savedMouse.ProductCode,
            ProductName = savedMouse.Name,
            Quantity = 1,
            UnitPrice = 500.00m,
            LineTotal = 500.00m
        };

        // Create child for Order B
        OrderItem keyboardItem = new OrderItem
        {
            OrderId = orderBId,
            ProductId = savedKeyboard.ProductId,
            ProductCode = savedKeyboard.ProductCode,
            ProductName = savedKeyboard.Name,
            Quantity = 1,
            UnitPrice = 1000.00m,
            LineTotal = 1000.00m
        };

        orderItemRepository.AddOrderItem(mouseItem);
        orderItemRepository.AddOrderItem(keyboardItem);

        // Act
        List<OrderItem> orderAItems =
            orderItemRepository
                .GetOrderItemsByOrderId(
                    orderAId
                );

        // Assert
        Assert.Single(orderAItems);

        Assert.Equal(
            orderAId,
            orderAItems[0].OrderId
        );

        Assert.Equal(
            "TEST-MOUSE-A",
            orderAItems[0].ProductCode
        );

        Assert.DoesNotContain(
            orderAItems,
            item =>
                item.ProductCode ==
                "TEST-KEYBOARD-B"
        );
    }
}