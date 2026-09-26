namespace StockFlow.Models;

public class Receipt
{
    public int ReceiptId { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;

    public int OrderId { get; set; }
    public int PaymentId { get; set; }

    public string OrderNumber { get; set; } = string.Empty;
    public string PaymentNumber { get; set; } = string.Empty;

    public DateTime ReceiptDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal AmountPaid { get; set; }
    public decimal ChangeAmount { get; set; }

    public Receipt(
        int receiptId,
        string receiptNumber,
        int orderId,
        int paymentId,
        string orderNumber,
        string paymentNumber,
        DateTime receiptDate,
        decimal totalAmount,
        string paymentMethod,
        decimal amountPaid,
        decimal changeAmount
    )
    {
        ReceiptId = receiptId;
        ReceiptNumber = receiptNumber;
        OrderId = orderId;
        PaymentId = paymentId;
        OrderNumber = orderNumber;
        PaymentNumber = paymentNumber;
        ReceiptDate = receiptDate;
        TotalAmount = totalAmount;
        PaymentMethod = paymentMethod;
        AmountPaid = amountPaid;
        ChangeAmount = changeAmount;
    }
    // Parameterless constructor for object initialization and data mapping.
    public Receipt()
    {
        
    }
}