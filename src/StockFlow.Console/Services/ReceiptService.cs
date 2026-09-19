using System.Runtime.CompilerServices;
using StockFlow.Models;
using StockFlow.Utilities;
using StockFlow.Repositories;

namespace StockFlow.Services;

public class ReceiptService
{
    private readonly InputValidationService _inputValidationService;
    private readonly PaymentRepository _paymentRepository;
    private readonly OrderRepository _orderRepository;
    private readonly ReceiptRepository _receiptRepository;
    
    public ReceiptService(
        InputValidationService inputValidationService,
        PaymentRepository paymentRepository,
        OrderRepository orderRepository,
        ReceiptRepository receiptRepository
    )
    {
        _inputValidationService = inputValidationService;
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _receiptRepository = receiptRepository;
    }

    public void GenerateReceipt(List<Receipt> receipts)
    {
        /*if(orders.Count == 0)
        {
            Console.WriteLine("There are no Orders available to generate receipt.");
            return;
        }
        if(payments.Count == 0)
        {
            Console.WriteLine("There are no Payments available to generate receipt.");
            return;
        }*/

        string paymentNumber = _inputValidationService.GetRequiredText("Enter payment number: ");

        Payment? payment = _paymentRepository.FindPaymentByNumber(paymentNumber);

        if (payment == null)
        {
            Console.WriteLine("Payment not found.\n");
            return;
        }

        Order? order = _orderRepository.FindOrderByNumber(payment.OrderNumber);

        if (order == null)
        {
            Console.WriteLine("Related order not found.\n");
            return;
        }

        string receiptNumber = GenerateNextReceiptNumber();

        Receipt receipt = new Receipt
        {
            ReceiptNumber = receiptNumber,
            OrderNumber = order.OrderNumber,
            PaymentNumber = payment.PaymentNumber,
            ReceiptDate = DateTime.Now,
            TotalAmount = payment.AmountDue,
            PaymentMethod = payment.PaymentMethod,
            AmountPaid = payment.AmountPaid,
            ChangeAmount = payment.ChangeAmount
        };


        //receipts.Add(receipt);
        _receiptRepository.AddReceipt(order.OrderId, payment.PaymentId, receipt);

        Receipt? savedReceipt = _receiptRepository.FindReceiptByNumber(receipt.ReceiptNumber);

        if (savedReceipt != null)
        {
            receipts.Add(savedReceipt);
            Console.WriteLine("Receipt generated successfully.\n");
            PrintReceipt(order, payment, savedReceipt);
            return;
        }

        Console.WriteLine("Receipt was saved, but could not be reloaded from SQLite.\n");

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

    public void ExportReceiptToTextFile(List<Order> orders, List<Payment> payments, List<Receipt> receipts)
    {
        if (receipts.Count == 0)
        {
            Console.WriteLine("There are no receipts to export.");
            return;
        }
        string orderNumber = _inputValidationService.GetRequiredText("Input Order Number to export receipt: ");

        Receipt? receipt = receipts.FirstOrDefault(receipt 
            => receipt.OrderNumber.Equals(orderNumber, StringComparison.OrdinalIgnoreCase)
        );

        if(receipt == null)
        {
            Console.WriteLine("Receipt is not found for this order.");
            return;
        }

        Order? order = orders.FirstOrDefault(order 
            => order.OrderNumber.Equals(orderNumber, StringComparison.OrdinalIgnoreCase)
        );

        if(order == null)
        {
            Console.WriteLine("The Order is not found for this receipt.");
            return;
        }

        Payment? payment = payments.FirstOrDefault(payment 
            => payment.OrderNumber.Equals(orderNumber, StringComparison.OrdinalIgnoreCase)
        );

        if(payment == null)
        {
            Console.WriteLine("The Payment record is not found for this receipt.");
            return;
        }

        string folderPath = "Receipts";

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string filePath = Path.Combine(folderPath, $"{receipt.ReceiptNumber}.txt");

        try
        {
            string receiptContent = BuildReceiptContent(order, payment, receipt);

            File.WriteAllText(filePath, receiptContent);

            Console.WriteLine($"Receipt exported successfully to {filePath}.\n");

        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occured while exporting the receipt.");
            Console.WriteLine($"Error details: {ex.Message}");
        }
    }

    public string BuildReceiptContent(Order order, Payment payment, Receipt receipt)
    {
        string receiptContent = "";

        receiptContent += "=================================\n";
        receiptContent += "          STOCKFLOW RECEIPT       \n";
        receiptContent += "=================================\n";
        receiptContent += $"Receipt Number: {receipt.ReceiptNumber}\n";
        receiptContent += $"Order Number: {receipt.OrderNumber}\n";
        receiptContent += $"Payment Number: {receipt.PaymentNumber}\n";
        receiptContent += $"Receipt Date: {receipt.ReceiptDate}\n";
        receiptContent += "---------------------------------\n";
        receiptContent += "Items:\n";

        foreach (OrderItem item in order.Items)
        {
            receiptContent += $"{item.ProductName} x {item.Quantity}\n";
            receiptContent += $"  Unit Price: {item.UnitPrice:C}\n";
            receiptContent += $"  Line Total: {item.LineTotal:C}\n";
        }

        receiptContent += "---------------------------------\n";
        receiptContent += $"Total Amount: {receipt.TotalAmount:C}\n";
        receiptContent += $"Payment Method: {receipt.PaymentMethod}\n";
        receiptContent += $"Amount Paid: {receipt.AmountPaid:C}\n";
        receiptContent += $"Change: {receipt.ChangeAmount:C}\n";
        receiptContent += "=================================\n";

        return receiptContent;
    }

    public string GenerateNextReceiptNumber()
    {
        List<Receipt> savedReceipts = _receiptRepository.GetAllReceipts();

        if(savedReceipts.Count() == 0)
        {
            return "REC-001";
        }

        int nextReceiptNNumber = savedReceipts.Max(receipt => receipt.ReceiptId) + 1;

        return $"REC-{nextReceiptNNumber:D3}";
        
    }
    public void PrintReceipt(Order order, Payment payment, Receipt receipt)
    {
        Console.WriteLine(BuildReceiptContent(order, payment,  receipt));
    }
}