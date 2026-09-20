using System.Security.Authentication.ExtendedProtection;
using StockFlow.Models;
using StockFlow.Utilities;
using StockFlow.Repositories;

namespace StockFlow.Services;

public class PaymentService
{
    private readonly InputValidationService _inputValidationService;
    private readonly PaymentRepository _paymentRepository;
    private readonly OrderRepository _orderRepository;

    public PaymentService(
        InputValidationService inputValidationService,
        PaymentRepository paymentRepository,
        OrderRepository orderRepository
    )
    {
        _inputValidationService = inputValidationService;
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
    }

    public void ProcessPayment(List<Order> orders, List<Payment> payments)
    {
        /*if(orders.Count == 0)
        {
            Console.WriteLine("There are no Orders available for payment.");
            return;
        }*/
        
        string orderNumber = _inputValidationService.GetRequiredText("Input Order Number to pay: ");

        /*Order? order = orders.FirstOrDefault(order => 
            order.OrderNumber.Equals(orderNumber, StringComparison.OrdinalIgnoreCase));*/

        Order? order = _orderRepository.FindOrderByNumber(orderNumber);

        if(order == null)
        {
            Console.WriteLine("The order is not existing.");
            return;
        }

        if(order.PaymentStatus.Equals("Paid", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("The order is already paid.");
            return;
        }

        string paymentMethod = GetPaymentMethod();

        decimal amountPaid = _inputValidationService.GetValidDecimal(
            "Amount Paid: ", 
            order.TotalAmount, 
            1000000m);

        //int paymentId = payments.Count + 1;
        //string paymentNumber = $"PAY-{paymentId:000}";
        string paymentNumber = GenerateNextPaymentNumber();

        decimal changeAmount = CalculateChange(order.TotalAmount, amountPaid);

        Payment payment = new Payment
        {
            //paymentId,
            PaymentNumber = paymentNumber,
            OrderNumber = order.OrderNumber,
            PaymentDate = DateTime.Now,
            PaymentMethod = paymentMethod,
            AmountDue = order.TotalAmount,
            AmountPaid = amountPaid,
            ChangeAmount = changeAmount,
            PaymentStatus = "Paid"
        };

        //payments.Add(payment);
        _paymentRepository.AddPayment(order.OrderId, payment);

        //order.PaymentStatus = "Paid";
        _orderRepository.UpdatePaymentStatus(order.OrderNumber, "Paid");
        //order.OrderStatus = "Completed";
        _orderRepository.UpdateOrderStatus(order.OrderNumber, "completed");

        // Reloads the updated order from SQLite.
        Order? updatedOrder = _orderRepository.FindOrderByNumber(order.OrderNumber);

        if (updatedOrder != null)
        {
            Order? existingOrder = orders.FirstOrDefault(currentOrder =>
                currentOrder.OrderNumber.Equals(updatedOrder.OrderNumber, StringComparison.OrdinalIgnoreCase)
            );

            if (existingOrder != null)
            {
                existingOrder.OrderStatus = updatedOrder.OrderStatus;
                existingOrder.PaymentStatus = updatedOrder.PaymentStatus;
                existingOrder.TotalAmount = updatedOrder.TotalAmount;
                existingOrder.OrderDate = updatedOrder.OrderDate;
            }
            else
            {
                orders.Add(updatedOrder);
            }
        }

        Payment? savedPayment = _paymentRepository.FindPaymentByNumber(payment.PaymentNumber);

        if(savedPayment != null)
        {
            payments.Add(savedPayment);
        }

        Console.WriteLine($"Payment {payment.PaymentNumber} processed successfully.");
        Console.WriteLine($"Order Number: {payment.OrderNumber}");
        Console.WriteLine($"Payment Method: {payment.PaymentMethod}");
        Console.WriteLine($"Amount Due: {payment.AmountDue:C}");
        Console.WriteLine($"Amount Paid: {payment.AmountPaid:C}");
        Console.WriteLine($"Change: {payment.ChangeAmount:C}");
        Console.WriteLine($"Payment Status: {payment.PaymentStatus}\n");
    }

    public string GenerateNextPaymentNumber()
    {
        List<Payment> savedPayments = _paymentRepository.GetAllPayments();

        if(savedPayments.Count == 0)
        {
            return "PAY - 001";
        }

        int nextPaymentNumber = savedPayments.Max(payment => payment.PaymentId) + 1;

        return $"PAY-{nextPaymentNumber:D3}";


    }
    public string GetPaymentMethod()
    {
        while (true)
        {
            Console.WriteLine("1. Cash");
            Console.WriteLine("2. QR Payment");
            Console.WriteLine("3. Bank Transfer");
            Console.WriteLine("4. Card\n");

            int option = _inputValidationService.GetValidInt("Choose your payment method: ", 1, 4);

            switch (option)
            {
                case 1: 
                    return "Cash";
                case 2: 
                    return "QR Payment";
                case 3: 
                    return "Bank Transfer";
                case 4: 
                    return "Card";
                default:
                    Console.WriteLine("Invalid payment method.\n");
                    break;
            } 
        }
    }
    public void DisplayPayment(Payment payment)
    {
        Console.WriteLine($"Payment ID: {payment.PaymentId}");
        Console.WriteLine($"Payment Number: {payment.PaymentNumber}");
        Console.WriteLine($"Order Number: {payment.OrderNumber}");
        Console.WriteLine($"Payment Date: {payment.PaymentDate}");
        Console.WriteLine($"Payment Method: {payment.PaymentMethod}");
        Console.WriteLine($"Amount Due: {payment.AmountDue:C}");
        Console.WriteLine($"Amount Paid: {payment.AmountPaid:C}");
        Console.WriteLine($"Change: {payment.ChangeAmount:C}");
        Console.WriteLine($"Payment Status: {payment.PaymentStatus}");
        Console.WriteLine("--------");
    }
    public void ViewPayments(List<Payment> payments)
    {
        if(payments.Count == 0)
        {
            Console.WriteLine("No payments available.\n");
            return;
        }

        Console.WriteLine("\nPAYMENTS");
        Console.WriteLine("--------");

        foreach(Payment payment in payments)
        {
            DisplayPayment(payment);
        }
    }
    public decimal CalculateChange(decimal amountDue, decimal amountPaid)
    {
        return amountPaid - amountDue;
    }
}