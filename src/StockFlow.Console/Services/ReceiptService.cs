using StockFlow.Models;
using StockFlow.Utilities;
using StockFlow.Repositories;

namespace StockFlow.Services;

public class ReceiptService
{
    private readonly InputValidationService _inputValidationService;
    private readonly PaymentRepository _paymentRepository;
    private readonly OrderRepository _orderRepository;

    // Needed to load the OrderItems that belong to an Order.
    // OrderRepository only loads the Orders table, so Items must be loaded separately.
    private readonly OrderItemRepository _orderItemRepository;

    private readonly ReceiptRepository _receiptRepository;

    public ReceiptService(
        InputValidationService inputValidationService,
        PaymentRepository paymentRepository,
        OrderRepository orderRepository,
        OrderItemRepository orderItemRepository,
        ReceiptRepository receiptRepository
    )
    {
        _inputValidationService = inputValidationService;
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _receiptRepository = receiptRepository;
    }

    public void GenerateReceipt()
    {
        string paymentNumber = _inputValidationService.GetRequiredText(
            "Enter payment number: "
        );

        // Retrieves the payment directly from SQLite.
        Payment? payment = _paymentRepository.FindPaymentByNumber(paymentNumber);

        if (payment == null)
        {
            Console.WriteLine("Payment not found.\n");
            return;
        }

        if (!payment.PaymentStatus.Equals(
            "Paid",
            StringComparison.OrdinalIgnoreCase
        ))
        {
            Console.WriteLine("A receipt can only be generated for a paid payment.");

            return;
        }

        // Prevents multiple receipts for the same payment.
        Receipt? existingReceipt = _receiptRepository.FindReceiptByPaymentNumber(payment.PaymentNumber);

        if (existingReceipt != null)
        {
            Console.WriteLine( $"Receipt {existingReceipt.ReceiptNumber} already exists for payment {payment.PaymentNumber}.\n"            );

            return;
        }

        // Retrieves the related order using the OrderNumber stored in the payment.
        Order? order = _orderRepository.FindOrderByNumber(payment.OrderNumber);

        if (order == null)
        {
            Console.WriteLine("Related order not found.\n");
            return;
        }

        // CHANGE:
        // FindOrderByNumber() only loads data from the Orders table.
        // The related OrderItems are stored in a separate table, so load them
        // using OrderId before printing the receipt.
        order.Items = _orderItemRepository.GetOrderItemsByOrderId(order.OrderId);

        if (order.Items.Count == 0)
{
            Console.WriteLine("Receipt cannot be generated because the order has no saved items.");
            return;
        }

        string receiptNumber = GenerateNextReceiptNumber();

        // Creates a receipt snapshot using the saved payment and order data.
        Receipt receipt = new Receipt
        {
            OrderId = order.OrderId,
            PaymentId = payment.PaymentId,
            ReceiptNumber = receiptNumber,
            OrderNumber = order.OrderNumber,
            PaymentNumber = payment.PaymentNumber,
            ReceiptDate = DateTime.Now,
            TotalAmount = payment.AmountDue,
            PaymentMethod = payment.PaymentMethod,
            AmountPaid = payment.AmountPaid,
            ChangeAmount = payment.ChangeAmount
        };

        // Saves the receipt to SQLite
        _receiptRepository.AddReceipt(receipt);

        // Reloads the saved receipt so the database version is used.
        Receipt? savedReceipt = _receiptRepository.FindReceiptByNumber(receipt.ReceiptNumber);

        if (savedReceipt != null)
        {
            Console.WriteLine("Receipt generated successfully.\n");

            // order.Items is already populated above, so the purchased
            // products can now be included when the receipt is printed.
            PrintReceipt(order, savedReceipt);
            return;
        }

        Console.WriteLine(
            "Receipt was saved, but could not be reloaded from SQLite.\n"
        );
    }

    public void ViewReceipts()
    {
        // Retrieves the current receipt records directly from SQLite.
        List<Receipt> receipts = _receiptRepository.GetAllReceipts();

        if (receipts.Count == 0)
        {
            Console.WriteLine("No receipts available to view.");
            return;
        }

        Console.WriteLine("\nRECEIPTS");
        Console.WriteLine("--------");

        foreach (Receipt receipt in receipts)
        {
            // Retrieves the order related to this receipt.
            Order? order =
                _orderRepository.FindOrderByNumber(receipt.OrderNumber);

            if (order == null)
            {
                Console.WriteLine(
                    $"Order {receipt.OrderNumber} could not be found."
                );
                continue;
            }

            // CHANGE:
            // The Order object does not automatically contain its OrderItems
            // when loaded from the Orders table. Load them before printing.
            order.Items =_orderItemRepository.GetOrderItemsByOrderId(order.OrderId);

            PrintReceipt(order, receipt);
        }
    }

    public void ExportReceiptToTextFile()
    {
        string orderNumber = _inputValidationService.GetRequiredText(
            "Input Order Number to export receipt: "
        );

        // Retrieves only receipts belonging to the requested order.
        List<Receipt> receipts =
            _receiptRepository.GetReceiptsByOrderNumber(orderNumber);

        if (receipts.Count == 0)
        {
            Console.WriteLine("Receipt is not found for this order.");
            return;
        }

        // Retrieves the order related to the receipt.
        Order? order =
            _orderRepository.FindOrderByNumber(orderNumber);

        if (order == null)
        {
            Console.WriteLine(
                "The order is not found for this receipt."
            );
            return;
        }

        // CHANGE:
        // BuildReceiptContent() loops through order.Items.
        // Load the OrderItems from SQLite before building the receipt text.
        order.Items =
            _orderItemRepository.GetOrderItemsByOrderId(order.OrderId);

        string folderPath = "Receipts";

        // Creates the Receipts folder if it does not already exist.
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        foreach (Receipt receipt in receipts)
        {
            string filePath = Path.Combine(
                folderPath,
                $"{receipt.ReceiptNumber}.txt"
            );

            try
            {
                string receiptContent = BuildReceiptContent(order, receipt);

                File.WriteAllText(filePath, receiptContent);

                Console.WriteLine(
                    $"Receipt exported successfully to {filePath}.\n"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "An error occurred while exporting the receipt."
                );

                Console.WriteLine(
                    $"Error details: {ex.Message}"
                );
            }
        }
    }

    public string GenerateNextReceiptNumber()
    {
        List<Receipt> savedReceipts =
            _receiptRepository.GetAllReceipts();

        if (savedReceipts.Count == 0)
        {
            return "REC-001";
        }

        int nextReceiptNumber =
            savedReceipts.Max(receipt => receipt.ReceiptId) + 1;

        return $"REC-{nextReceiptNumber:D3}";
    }

    public void PrintReceipt(Order order, Receipt receipt)
    {
        Console.WriteLine(BuildReceiptContent(order, receipt));
    }

    public string BuildReceiptContent(Order order, Receipt receipt)
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

        // order.Items must be loaded from OrderItemRepository before
        // this method is called.
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
}