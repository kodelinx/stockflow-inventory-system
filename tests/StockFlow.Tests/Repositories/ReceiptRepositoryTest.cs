using StockFlow.Models;
using StockFlow.Repositories;
using StockFlow.Tests.Helpers;

namespace StockFlow.Tests.Repositories;

public class ReceiptRepositoryTests
{
    [Fact]
    public void AddReceipt_ValidReceipt_PersistsReceipt()
    {
        // Arrange
        using TestDatabase testDatabase =
            new TestDatabase();

        OrderRepository orderRepository =
            new OrderRepository(
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

        Order order = new Order
        {
            OrderNumber = "TEST-ORD-REC-001",
            OrderDate = DateTime.Now,
            TotalAmount = 1500.00m,
            OrderStatus = "Completed",
            PaymentStatus = "Paid"
        };

        int orderId =
            orderRepository.AddOrder(order);

        Payment payment = new Payment
        {
            PaymentNumber = "TEST-PAY-REC-001",
            OrderId = orderId,
            OrderNumber = order.OrderNumber,
            PaymentDate = DateTime.Now,
            PaymentMethod = "Cash",
            AmountDue = 1500.00m,
            AmountPaid = 2000.00m,
            ChangeAmount = 500.00m,
            PaymentStatus = "Paid"
        };

        paymentRepository.AddPayment(
            payment
        );

        Payment? savedPayment =
            paymentRepository.FindPaymentByNumber(
                "TEST-PAY-REC-001"
            );

        Assert.NotNull(savedPayment);

        Receipt receipt = new Receipt
        {
            ReceiptNumber = "TEST-REC-001",
            OrderId = orderId,
            PaymentId = savedPayment.PaymentId,
            OrderNumber = order.OrderNumber,
            PaymentNumber = savedPayment.PaymentNumber,
            ReceiptDate = DateTime.Now,
            TotalAmount = 1500.00m,
            PaymentMethod = "Cash",
            AmountPaid = 2000.00m,
            ChangeAmount = 500.00m
        };

        // Act
        receiptRepository.AddReceipt(receipt);

        Receipt? savedReceipt =
            receiptRepository.FindReceiptByNumber(
                "TEST-REC-001"
            );

        // Assert
        Assert.NotNull(savedReceipt);

        Assert.True(
            savedReceipt.ReceiptId > 0
        );

        Assert.Equal(
            orderId,
            savedReceipt.OrderId
        );

        Assert.Equal(
            savedPayment.PaymentId,
            savedReceipt.PaymentId
        );

        Assert.Equal(
            "TEST-ORD-REC-001",
            savedReceipt.OrderNumber
        );

        Assert.Equal(
            "TEST-PAY-REC-001",
            savedReceipt.PaymentNumber
        );

        Assert.Equal(
            1500.00m,
            savedReceipt.TotalAmount
        );

        Assert.Equal(
            2000.00m,
            savedReceipt.AmountPaid
        );

        Assert.Equal(
            500.00m,
            savedReceipt.ChangeAmount
        );
    }

    [Fact]
    public void FindReceiptByPaymentNumber_ExistingReceipt_ReturnsReceipt()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        OrderRepository orderRepository =
            new OrderRepository(
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

        Order order = new Order
        {
            OrderNumber = "TEST-ORD-REC-002",
            OrderDate = DateTime.Now,
            TotalAmount = 900.00m,
            OrderStatus = "Completed",
            PaymentStatus = "Paid"
        };

        int orderId =
            orderRepository.AddOrder(order);

        Payment payment = new Payment
        {
            PaymentNumber = "TEST-PAY-REC-002",
            OrderId = orderId,
            OrderNumber = order.OrderNumber,
            PaymentDate = DateTime.Now,
            PaymentMethod = "Cash",
            AmountDue = 900.00m,
            AmountPaid = 1000.00m,
            ChangeAmount = 100.00m,
            PaymentStatus = "Paid"
        };

        paymentRepository.AddPayment(
            payment
        );

        Payment? savedPayment =
            paymentRepository.FindPaymentByNumber(
                "TEST-PAY-REC-002"
            );

        Assert.NotNull(savedPayment);

        Receipt receipt = new Receipt
        {
            ReceiptNumber = "TEST-REC-002",
            OrderId = orderId,
            PaymentId = savedPayment.PaymentId,
            OrderNumber = order.OrderNumber,
            PaymentNumber = savedPayment.PaymentNumber,
            ReceiptDate = DateTime.Now,
            TotalAmount = 900.00m,
            PaymentMethod = "Cash",
            AmountPaid = 1000.00m,
            ChangeAmount = 100.00m
        };

        receiptRepository.AddReceipt(receipt);

        // Act
        Receipt? savedReceipt =
            receiptRepository
                .FindReceiptByPaymentNumber(
                    "TEST-PAY-REC-002"
                );

        // Assert
        Assert.NotNull(savedReceipt);

        Assert.Equal(
            "TEST-REC-002",
            savedReceipt.ReceiptNumber
        );

        Assert.Equal(
            savedPayment.PaymentId,
            savedReceipt.PaymentId
        );
    }

    [Fact]
    public void FindReceiptByPaymentNumber_UnknownPayment_ReturnsNull()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        ReceiptRepository receiptRepository =
            new ReceiptRepository(
                testDatabase.DatabaseConnectionService
            );

        Receipt? receipt =
            receiptRepository
                .FindReceiptByPaymentNumber(
                    "DOES-NOT-EXIST"
                );

        Assert.Null(receipt);
    }

    [Fact]
    public void GetReceiptsByOrderNumber_TwoOrders_ReturnsOnlyRequestedOrderReceipts()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        OrderRepository orderRepository =
            new OrderRepository(
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

        // Order A
        Order orderA = new Order
        {
            OrderNumber = "TEST-REC-ORDER-A",
            OrderDate = DateTime.Now,
            TotalAmount = 500.00m,
            OrderStatus = "Completed",
            PaymentStatus = "Paid"
        };

        int orderAId =
            orderRepository.AddOrder(orderA);

        Payment paymentA = new Payment
        {
            PaymentNumber = "TEST-REC-PAY-A",
            OrderId = orderAId,
            OrderNumber = orderA.OrderNumber,
            PaymentDate = DateTime.Now,
            PaymentMethod = "Cash",
            AmountDue = 500.00m,
            AmountPaid = 500.00m,
            ChangeAmount = 0.00m,
            PaymentStatus = "Paid"
        };

        paymentRepository.AddPayment(
            paymentA
        );

        Payment? savedPaymentA =
            paymentRepository.FindPaymentByNumber(
                "TEST-REC-PAY-A"
            );

        Assert.NotNull(savedPaymentA);

        Receipt receiptA = new Receipt
        {
            ReceiptNumber = "TEST-REC-A",
            OrderId = orderAId,
            PaymentId = savedPaymentA.PaymentId,
            OrderNumber = orderA.OrderNumber,
            PaymentNumber = savedPaymentA.PaymentNumber,
            ReceiptDate = DateTime.Now,
            TotalAmount = 500.00m,
            PaymentMethod = "Cash",
            AmountPaid = 500.00m,
            ChangeAmount = 0.00m
        };

        // Order B
        Order orderB = new Order
        {
            OrderNumber = "TEST-REC-ORDER-B",
            OrderDate = DateTime.Now,
            TotalAmount = 1000.00m,
            OrderStatus = "Completed",
            PaymentStatus = "Paid"
        };

        int orderBId =
            orderRepository.AddOrder(orderB);

        Payment paymentB = new Payment
        {
            PaymentNumber = "TEST-REC-PAY-B",
            OrderId = orderBId,
            OrderNumber = orderB.OrderNumber,
            PaymentDate = DateTime.Now,
            PaymentMethod = "Cash",
            AmountDue = 1000.00m,
            AmountPaid = 1000.00m,
            ChangeAmount = 0.00m,
            PaymentStatus = "Paid"
        };

        paymentRepository.AddPayment(
            paymentB
        );

        Payment? savedPaymentB =
            paymentRepository.FindPaymentByNumber(
                "TEST-REC-PAY-B"
            );

        Assert.NotNull(savedPaymentB);

        Receipt receiptB = new Receipt
        {
            ReceiptNumber = "TEST-REC-B",
            OrderId = orderBId,
            PaymentId = savedPaymentB.PaymentId,
            OrderNumber = orderB.OrderNumber,
            PaymentNumber = savedPaymentB.PaymentNumber,
            ReceiptDate = DateTime.Now,
            TotalAmount = 1000.00m,
            PaymentMethod = "Cash",
            AmountPaid = 1000.00m,
            ChangeAmount = 0.00m
        };

        receiptRepository.AddReceipt(receiptA);
        receiptRepository.AddReceipt(receiptB);

        // Act
        List<Receipt> orderAReceipts =
            receiptRepository
                .GetReceiptsByOrderNumber(
                    "TEST-REC-ORDER-A"
                );

        // Assert
        Assert.Single(orderAReceipts);

        Assert.Equal(
            "TEST-REC-A",
            orderAReceipts[0].ReceiptNumber
        );

        Assert.DoesNotContain(
            orderAReceipts,
            receipt =>
                receipt.ReceiptNumber ==
                "TEST-REC-B"
        );
    }
}