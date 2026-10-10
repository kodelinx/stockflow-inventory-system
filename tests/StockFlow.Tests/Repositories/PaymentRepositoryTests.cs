using StockFlow.Models;
using StockFlow.Repositories;
using StockFlow.Tests.Helpers;

namespace StockFlow.Tests.Repositories;

public class PaymentRepositoryTests
{
    [Fact]
    public void AddPayment_ValidPayment_PersistsPayment()
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

        Order order = new Order
        {
            OrderNumber = "TEST-ORD-PAY-001",
            OrderDate = DateTime.Now,
            TotalAmount = 1500.00m,
            OrderStatus = "Completed",
            PaymentStatus = "Paid"
        };

        int orderId =
            orderRepository.AddOrder(order);

        Payment payment = new Payment
        {
            PaymentNumber = "TEST-PAY-001",
            OrderId = orderId,
            OrderNumber = "TEST-ORD-PAY-001",
            PaymentDate = DateTime.Now,
            PaymentMethod = "Cash",
            AmountDue = 1500.00m,
            AmountPaid = 2000.00m,
            ChangeAmount = 500.00m,
            PaymentStatus = "Paid"
        };

        // Act
        paymentRepository.AddPayment(
            payment
        );

        Payment? savedPayment =
            paymentRepository.FindPaymentByNumber(
                "TEST-PAY-001"
            );

        // Assert
        Assert.NotNull(savedPayment);

        Assert.True(
            savedPayment.PaymentId > 0
        );

        Assert.Equal(
            orderId,
            savedPayment.OrderId
        );

        Assert.Equal(
            "TEST-ORD-PAY-001",
            savedPayment.OrderNumber
        );

        Assert.Equal(
            1500.00m,
            savedPayment.AmountDue
        );

        Assert.Equal(
            2000.00m,
            savedPayment.AmountPaid
        );

        Assert.Equal(
            500.00m,
            savedPayment.ChangeAmount
        );

        Assert.Equal(
            "Cash",
            savedPayment.PaymentMethod
        );

        Assert.Equal(
            "Paid",
            savedPayment.PaymentStatus
        );
    }

    [Fact]
    public void FindPaymentByNumber_UnknownPayment_ReturnsNull()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        PaymentRepository paymentRepository =
            new PaymentRepository(
                testDatabase.DatabaseConnectionService
            );

        Payment? payment =
            paymentRepository.FindPaymentByNumber(
                "DOES-NOT-EXIST"
            );

        Assert.Null(payment);
    }

    [Fact]
    public void GetAllPayments_AddedPayment_IncludesPayment()
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

        Order order = new Order
        {
            OrderNumber = "TEST-ORD-PAY-002",
            OrderDate = DateTime.Now,
            TotalAmount = 750.00m,
            OrderStatus = "Completed",
            PaymentStatus = "Paid"
        };

        int orderId =
            orderRepository.AddOrder(order);

        Payment payment = new Payment
        {
            PaymentNumber = "TEST-PAY-002",
            OrderId = orderId,
            OrderNumber = order.OrderNumber,
            PaymentDate = DateTime.Now,
            PaymentMethod = "Cash",
            AmountDue = 750.00m,
            AmountPaid = 1000.00m,
            ChangeAmount = 250.00m,
            PaymentStatus = "Paid"
        };

        paymentRepository.AddPayment(
            payment
        );

        // Act
        List<Payment> payments =
            paymentRepository.GetAllPayments();

        // Assert
        Assert.Contains(
            payments,
            payment =>
                payment.PaymentNumber ==
                "TEST-PAY-002"
        );
    }

    [Fact]
    public void GetPaymentsByOrderNumber_TwoOrders_ReturnsOnlyRequestedOrderPayments()
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

        Order orderA = new Order
        {
            OrderNumber = "TEST-ORDER-A",
            OrderDate = DateTime.Now,
            TotalAmount = 500.00m,
            OrderStatus = "Completed",
            PaymentStatus = "Paid"
        };

        Order orderB = new Order
        {
            OrderNumber = "TEST-ORDER-B",
            OrderDate = DateTime.Now,
            TotalAmount = 1000.00m,
            OrderStatus = "Completed",
            PaymentStatus = "Paid"
        };

        int orderAId =
            orderRepository.AddOrder(orderA);

        int orderBId =
            orderRepository.AddOrder(orderB);

        Payment paymentA = new Payment
        {
            PaymentNumber = "TEST-PAY-A",
            OrderId = orderAId,
            OrderNumber = orderA.OrderNumber,
            PaymentDate = DateTime.Now,
            PaymentMethod = "Cash",
            AmountDue = 500.00m,
            AmountPaid = 500.00m,
            ChangeAmount = 0.00m,
            PaymentStatus = "Paid"
        };

        Payment paymentB = new Payment
        {
            PaymentNumber = "TEST-PAY-B",
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
            paymentA
        );

        paymentRepository.AddPayment(
            paymentB
        );

        // Act
        List<Payment> orderAPayments =
            paymentRepository
                .GetPaymentsByOrderNumber(
                    "TEST-ORDER-A"
                );

        // Assert
        Assert.Single(orderAPayments);

        Assert.Equal(
            "TEST-PAY-A",
            orderAPayments[0].PaymentNumber
        );

        Assert.Equal(
            orderAId,
            orderAPayments[0].OrderId
        );

        Assert.DoesNotContain(
            orderAPayments,
            payment =>
                payment.PaymentNumber ==
                "TEST-PAY-B"
        );
    }

    [Fact]
    public void GetPaymentsByOrderNumber_NoPayments_ReturnsEmptyList()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        PaymentRepository paymentRepository =
            new PaymentRepository(
                testDatabase.DatabaseConnectionService
            );

        List<Payment> payments =
            paymentRepository
                .GetPaymentsByOrderNumber(
                    "NO-PAYMENTS"
                );

        Assert.Empty(payments);
    }
}

