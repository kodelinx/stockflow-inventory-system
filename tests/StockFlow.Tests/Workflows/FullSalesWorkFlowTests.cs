using StockFlow.Models;
using StockFlow.Repositories;
using StockFlow.Tests.Helpers;

namespace StockFlow.Tests.Workflows;

public class FullSalesWorkflowTests
{
    [Fact]
    public void FullSalesWorkflow_ValidSale_PersistsExpectedFinalState()
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

        PaymentRepository paymentRepository =
            new PaymentRepository(
                testDatabase.DatabaseConnectionService
            );

        ReceiptRepository receiptRepository =
            new ReceiptRepository(
                testDatabase.DatabaseConnectionService
            );

        StockMovementRepository stockMovementRepository =
            new StockMovementRepository(
                testDatabase.DatabaseConnectionService
            );

        Product product = new Product
        {
            ProductCode = "TEST-SALE-P001",
            Name = "Wireless Mouse",
            Category = "Accessories",
            UnitPrice = 500.00m,
            QuantityInStock = 10,
            ReorderLevel = 2,
            IsActive = true
        };

        productRepository.AddProduct(product);

        Product? savedProduct =
            productRepository.FindProductByCode(
                "TEST-SALE-P001"
            );

        Assert.NotNull(savedProduct);

        int stockBefore =
            savedProduct.QuantityInStock;

        int quantitySold = 2;

        decimal lineTotal =
            savedProduct.UnitPrice *
            quantitySold;

        // -------------------------------------------------
        // ACT 1: CREATE ORDER
        // -------------------------------------------------

        Order order = new Order
        {
            OrderNumber = "TEST-ORD-SALE-001",
            OrderDate = DateTime.Now,
            TotalAmount = lineTotal,
            OrderStatus = "Pending Payment",
            PaymentStatus = "Unpaid"
        };

        int orderId =
            orderRepository.AddOrder(order);

        // -------------------------------------------------
        // ACT 2: CREATE ORDER ITEM SNAPSHOT
        // -------------------------------------------------

        OrderItem orderItem = new OrderItem
        {
            OrderId = orderId,
            ProductId = savedProduct.ProductId,
            ProductCode = savedProduct.ProductCode,
            ProductName = savedProduct.Name,
            Quantity = quantitySold,
            UnitPrice = savedProduct.UnitPrice,
            LineTotal = lineTotal
        };

        orderItemRepository.AddOrderItem(
            orderItem
        );

        // -------------------------------------------------
        // ACT 3: REDUCE PRODUCT STOCK
        // -------------------------------------------------

        savedProduct.QuantityInStock -= quantitySold;

        int stockAfter = savedProduct.QuantityInStock;

        productRepository.UpdateProduct(savedProduct);

        // -------------------------------------------------
        // ACT 4: RECORD STOCK OUT
        // -------------------------------------------------

        StockMovement stockMovement = new StockMovement
        {
            ProductId = savedProduct.ProductId,
            ProductCode = savedProduct.ProductCode,
            ProductName = savedProduct.Name,
            MovementType = "Stock Out",
            QuantityChanged = -quantitySold,
            StockBefore = stockBefore,
            StockAfter = stockAfter,
            Reason = $"Sold through order {order.OrderNumber}",
            MovementDate = DateTime.Now,
            ReferenceNumber = order.OrderNumber
        };

        stockMovementRepository.AddStockMovement(stockMovement);

        // -------------------------------------------------
        // ACT 5: CREATE PAYMENT
        // -------------------------------------------------

        Payment payment = new Payment
        {
            OrderId = orderId,
            PaymentNumber = "TEST-PAY-SALE-001",
            OrderNumber = order.OrderNumber,
            PaymentDate = DateTime.Now,
            PaymentMethod = "Cash",
            AmountDue = lineTotal,
            AmountPaid = 1200.00m,
            ChangeAmount = 1200.00m - lineTotal,
            PaymentStatus = "Paid"
        };

        paymentRepository.AddPayment(
            payment
        );

        orderRepository.UpdatePaymentStatus(
            order.OrderNumber,
            "Paid"
        );

        orderRepository.UpdateOrderStatus(
            order.OrderNumber,
            "Completed"
        );

        Payment? savedPayment =
            paymentRepository
                .FindPaymentByNumber(
                    "TEST-PAY-SALE-001"
                );

        Assert.NotNull(savedPayment);

        // -------------------------------------------------
        // ACT 6: CREATE RECEIPT
        // -------------------------------------------------

        Receipt receipt = new Receipt
        {
            ReceiptNumber =
                "TEST-REC-SALE-001",

            OrderId =
                orderId,

            PaymentId =
                savedPayment.PaymentId,

            OrderNumber =
                order.OrderNumber,

            PaymentNumber =
                savedPayment.PaymentNumber,

            ReceiptDate =
                DateTime.Now,

            TotalAmount =
                savedPayment.AmountDue,

            PaymentMethod =
                savedPayment.PaymentMethod,

            AmountPaid =
                savedPayment.AmountPaid,

            ChangeAmount =
                savedPayment.ChangeAmount
        };

        receiptRepository.AddReceipt(
            receipt
        );

        // -------------------------------------------------
        // ASSERT: VERIFY FINAL SYSTEM STATE
        // -------------------------------------------------

        Product? finalProduct =
            productRepository
                .FindProductByCode(
                    "TEST-SALE-P001"
                );

        Order? finalOrder =
            orderRepository
                .FindOrderByNumber(
                    "TEST-ORD-SALE-001"
                );

        List<OrderItem> finalOrderItems =
            orderItemRepository
                .GetOrderItemsByOrderId(
                    orderId
                );

        List<StockMovement> finalMovements =
            stockMovementRepository
                .GetStockMovementsByReferenceNumber(
                    "TEST-ORD-SALE-001"
                );

        Payment? finalPayment =
            paymentRepository
                .FindPaymentByNumber(
                    "TEST-PAY-SALE-001"
                );

        Receipt? finalReceipt =
            receiptRepository
                .FindReceiptByNumber(
                    "TEST-REC-SALE-001"
                );

        // PRODUCT
        Assert.NotNull(finalProduct);

        Assert.Equal(
            8,
            finalProduct.QuantityInStock
        );

        // ORDER
        Assert.NotNull(finalOrder);

        Assert.True(
            finalOrder.OrderId > 0
        );

        Assert.Equal(
            1000.00m,
            finalOrder.TotalAmount
        );

        Assert.Equal(
            "Completed",
            finalOrder.OrderStatus
        );

        Assert.Equal(
            "Paid",
            finalOrder.PaymentStatus
        );

        // ORDER ITEM
        Assert.Single(
            finalOrderItems
        );

        Assert.Equal(
            orderId,
            finalOrderItems[0].OrderId
        );

        Assert.Equal(
            "TEST-SALE-P001",
            finalOrderItems[0].ProductCode
        );

        Assert.Equal(
            2,
            finalOrderItems[0].Quantity
        );

        Assert.Equal(
            500.00m,
            finalOrderItems[0].UnitPrice
        );

        Assert.Equal(
            1000.00m,
            finalOrderItems[0].LineTotal
        );

        // STOCK MOVEMENT
        Assert.Single(
            finalMovements
        );

        Assert.Equal(
            "Stock Out",
            finalMovements[0].MovementType
        );

        Assert.Equal(
            -2,
            finalMovements[0].QuantityChanged
        );

        Assert.Equal(
            10,
            finalMovements[0].StockBefore
        );

        Assert.Equal(
            8,
            finalMovements[0].StockAfter
        );

        Assert.Equal(
            "TEST-ORD-SALE-001",
            finalMovements[0].ReferenceNumber
        );

        // PAYMENT
        Assert.NotNull(
            finalPayment
        );

        Assert.True(
            finalPayment.PaymentId > 0
        );

        Assert.Equal(
            orderId,
            finalPayment.OrderId
        );

        Assert.Equal(
            1000.00m,
            finalPayment.AmountDue
        );

        Assert.Equal(
            1200.00m,
            finalPayment.AmountPaid
        );

        Assert.Equal(
            200.00m,
            finalPayment.ChangeAmount
        );

        Assert.Equal(
            "Paid",
            finalPayment.PaymentStatus
        );

        // RECEIPT
        Assert.NotNull(
            finalReceipt
        );

        Assert.True(
            finalReceipt.ReceiptId > 0
        );

        Assert.Equal(
            orderId,
            finalReceipt.OrderId
        );

        Assert.Equal(
            finalPayment.PaymentId,
            finalReceipt.PaymentId
        );

        Assert.Equal(
            finalPayment.PaymentNumber,
            finalReceipt.PaymentNumber
        );

        Assert.Equal(
            1000.00m,
            finalReceipt.TotalAmount
        );

        Assert.Equal(
            1200.00m,
            finalReceipt.AmountPaid
        );

        Assert.Equal(
            200.00m,
            finalReceipt.ChangeAmount
        );
    }
}