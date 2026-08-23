using StockFlow.Models;
using StockFlow.Service;
using StockFlow.Utilities;

namespace StockFlow.Services;

public class ReceiptService
{
    public InputValidationService _inputValidationService;
    
    public ReceiptService(InputValidationService inputValidationService)
    {
        _inputValidationService = inputValidationService;
    }

    public void GenerateReceipt(
        List<Order> orders, 
        List<Payment> payments, 
        List<Receipt> receipts)
    {
        if(orders.Count == 0)
        {
            Console.WriteLine("There are no Orders available to generate receipt.");
            return;
        }
        if(payments.Count == 0)
        {
            Console.WriteLine("There are no Payments available to generate receipt.");
            return;
        }

        string orderNumber = _inputValidationService.GetRequiredText("Input Order Number you need to generate receipt: ");

        Order? order = orders.FirstOrDefault(order => 
            order.OrderNumber.Equals(orderNumber, StringComparison.OrdinalIgnoreCase));

        if(order == null)
        {
            Console.WriteLine("The Order is not found.");
            return;
        }
        if(!order.PaymentStatus.Equals("Paid", StringComparison.Ordinal))
        {
            Console.WriteLine("The Order is not yet Paid.");
            return;
        }

        Payment? payment = payments.FirstOrDefault(payment => 
            payment.OrderNumber.Equals(orderNumber, StringComparison.OrdinalIgnoreCase));

        if (payment == null)
        {
            Console.WriteLine("The payment record is not found for this order");
            return;
        }

        Receipt? existingReceipt = receipts.FirstOrDefault(receipt => 
            receipt.OrderNumber.Equals(orderNumber, StringComparison.OrdinalIgnoreCase));
        
        if (existingReceipt != null)
        {
            Console.WriteLine("Receipt already exists for this order.\n");
            return;
        }

        int receiptId = receipts.Count + 1;
        string receiptNumber = $"{receiptId:000}";

        Receipt receipt = new Receipt(
            receiptId,
            receiptNumber,
            orderNumber,
            payment.PaymentNumber,
            DateTime.Now,
            order.TotalAmount,
            payment.PaymentMethod,
            payment.AmountPaid,
            payment.ChangeAmount
        );

        receipts.Add(receipt);

        Console.WriteLine("Receipt generated successfully.\n");
        PrintReceipt(order, payment, receipt);

    }

    public void ViewReceipts(List<Order> orders, List<Payment> payments, List<Receipt> receipts)
    {
        if(receipts.Count == 0)
        {
            Console.WriteLine("No receipts available to view.");
            return;
        }

        Console.WriteLine("\nRECEIPTS");
        Console.WriteLine("--------");

        foreach(Receipt receipt in receipts)
        {
            Order? order = orders.FirstOrDefault(order => 
                order.OrderNumber.Equals(receipt.OrderNumber, StringComparison.OrdinalIgnoreCase));

            Payment? payment = payments.FirstOrDefault(payment => 
                payment.PaymentNumber.Equals(receipt.PaymentNumber, StringComparison.OrdinalIgnoreCase));

            if(order != null && payment != null)
            {
                PrintReceipt(order, payment, receipt);
            }
        }

    }
    public void PrintReceipt(Order order, Payment payment, Receipt receipt)
    {
        Console.WriteLine("=================================");
        Console.WriteLine("          STOCKFLOW RECEIPT       ");
        Console.WriteLine("=================================");
        Console.WriteLine($"Receipt Number: {receipt.ReceiptNumber}");
        Console.WriteLine($"Order Number: {receipt.OrderNumber}");
        Console.WriteLine($"Payment Number: {receipt.PaymentNumber}");
        Console.WriteLine($"Receipt Date: {receipt.ReceiptDate}");
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Items:");

        foreach (OrderItem item in order.Items)
        {
            Console.WriteLine($"{item.ProductName} x {item.Quantity}");
            Console.WriteLine($"  Unit Price: {item.UnitPrice:C}");
            Console.WriteLine($"  Line Total: {item.LineTotal:C}");
        }

        Console.WriteLine("---------------------------------");
        Console.WriteLine($"Total Amount: {receipt.TotalAmount:C}");
        Console.WriteLine($"Payment Method: {receipt.PaymentMethod}");
        Console.WriteLine($"Amount Paid: {receipt.AmountPaid:C}");
        Console.WriteLine($"Change: {receipt.ChangeAmount:C}");
        Console.WriteLine("=================================\n");
    }
}