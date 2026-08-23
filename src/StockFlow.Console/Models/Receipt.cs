using StockFlow.Models;

namespace StockFlow.Models;

public class Receipt
{
    public int ReceiptId { get; set; }
    public string ReceiptNumber { get; set; } = String.Empty; 
    public string OrderNumber { get; set; } = String.Empty; 
    public string PaymentNumber { get; set; } = String.Empty; 
    public DateTime ReceiptDate{ get; set; }

    public decimal TotalAmount { get; set; }
    public string PaymentMethod { get; set; } = String.Empty; 
    public decimal AmountPaid { get; set; } 
    public decimal ChangeAmount { get; set; }

    public Receipt(
        int receiptId,
        string receiptNumber,
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
        OrderNumber = orderNumber;
        PaymentNumber = paymentNumber;
        ReceiptDate = receiptDate;
        TotalAmount = totalAmount;
        PaymentMethod = paymentMethod;
        AmountPaid = amountPaid;
        ChangeAmount = changeAmount;
    }
    //Constructor to be utilized by JsonSerializer.Deserialize()
    public Receipt()
    {
        
    }
}