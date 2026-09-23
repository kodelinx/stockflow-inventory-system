using StockFlow.Models;
using StockFlow.Repositories;

namespace StockFlow.Services;

public class SalesReportService
{
    private readonly OrderRepository _orderRepository;
    private readonly PaymentRepository _paymentRepository;
    public SalesReportService(
        OrderRepository orderRepository,
        PaymentRepository paymentRepository
    )
    {
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
    }
    public void ShowSalesSummary()
    {
        Console.WriteLine("\nSales Summary Report");
        Console.WriteLine("====================");

        ShowOrderSalesSummary();
        ShowCompletedOrderDetails();
        ShowPaymentSalesSummary();
        ShowSalesByPaymentMethod();
        
        Console.WriteLine();
    }

    public void ShowOrderSalesSummary()
    {
        List<Order> orders = _orderRepository.GetAllOrders();
        int totalOrders = orders.Count;

        int completedOrders = orders.Count(order => 
            order.OrderStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase)
        );

        int pendingOrders = orders.Count(order => 
            !order.OrderStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase)
        );

        Console.WriteLine("\nOrder Sales Summary");
        Console.WriteLine("-------------------");
        Console.WriteLine($"Total Orders: {totalOrders}");
        Console.WriteLine($"Completed Orders: {completedOrders}");
        Console.WriteLine($"Pending Orders: {pendingOrders}");
    }

    public void ShowCompletedOrderDetails()
    {
        List<Order> orders = _orderRepository.GetAllOrders();
        Console.WriteLine("\nCompleted Orders Details");
        Console.WriteLine("------------------------");
        List<Order> completedOrders = orders
            .Where(order => order.OrderStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase))
            .ToList();
        
        if (completedOrders.Count == 0)
        {
            Console.WriteLine("No completed orders available");
            return;
        }

        foreach (Order order in completedOrders)
        {
            Console.WriteLine($"Order Number: {order.OrderNumber}");
            Console.WriteLine($"Order Date: {order.OrderDate}");
            Console.WriteLine($"Total Amount: {order.TotalAmount:C}");
            Console.WriteLine($"Payment Status: {order.PaymentStatus}");
            Console.WriteLine("-----------------------");
        }
    }

    public void ShowPaymentSalesSummary()
    {
        List<Payment> payments = _paymentRepository.GetAllPayments();
        int totalPayments = payments.Count;

        decimal totalSalesIncome = payments
            .Where(payment => payment.PaymentStatus.Equals("Paid", StringComparison.OrdinalIgnoreCase))
            .Sum(payment => payment.AmountDue);
        
        decimal totalCashReceived = payments
            .Where(payment => payment.PaymentStatus.Equals("Paid", StringComparison.OrdinalIgnoreCase))
            .Sum(payment => payment.AmountPaid);
        
        decimal totalChangeGiven = payments
            .Where(payment => payment.PaymentStatus.Equals("Paid", StringComparison.OrdinalIgnoreCase))
            .Sum(payment => payment.ChangeAmount);

        Console.WriteLine("\n---------------------");
        Console.WriteLine("Payment Sales Summary");
        Console.WriteLine("---------------------");
        Console.WriteLine($"Total Payments: {totalPayments}");
        Console.WriteLine($"Total Sales Income: {totalSalesIncome:C}");
        Console.WriteLine($"Total Cash Received: {totalCashReceived:C}");
        Console.WriteLine($"Total Change Given: {totalChangeGiven:C}");

    }

    public void ShowSalesByPaymentMethod()
    {
        List<Payment> payments = _paymentRepository.GetAllPayments();

        List<Payment> paidPayments = payments
            .Where(payment => payment.PaymentStatus.Equals("Paid", StringComparison.OrdinalIgnoreCase))
            .ToList();
        
        if(paidPayments.Count == 0)
        {
            Console.WriteLine("\nNo paid payments available");
            return;
        }

        List<string> paymentMethods = paidPayments
            .Select(payment => payment.PaymentMethod)
            .Distinct()
            .ToList();
        
        
        Console.WriteLine("\nSales per Payment Method Summary");
        Console.WriteLine("---------------------------------");

        foreach(string paymentMethod in paymentMethods)
        {
            decimal methodTotal = paidPayments
                .Where(payment => payment.PaymentMethod.Equals(paymentMethod))
                .Sum(payment => payment.AmountDue);
            
            int methodCount = paidPayments
                .Count(payment => payment.PaymentMethod.Equals(paymentMethod)
            );

            Console.WriteLine($"{paymentMethod}: {methodCount} payment(s), {methodTotal:C}");
        }
    }


}