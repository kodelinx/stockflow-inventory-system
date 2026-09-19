using StockFlow.Models;
namespace StockFlow.Models;

public class Payment
{
    public int PaymentId { get; set; }
    public string PaymentNumber { get; set; } = String.Empty; 
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = String.Empty; 
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = String.Empty; 
    public decimal AmountDue { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal ChangeAmount { get; set; }
    public string PaymentStatus { get; set; } = String.Empty; 

    public Payment(
        int paymentId,
        string paymentNumber,
        int orderId,
        string orderNumber,
        DateTime paymentDate,
        string paymentMethod,
        decimal amountDue,
        decimal amountPaid,
        decimal changeAmount,
        string paymentStatus
    )
    {
        PaymentId = paymentId;
        PaymentNumber = paymentNumber;
        OrderId = orderId;
        OrderNumber = orderNumber;
        PaymentDate = paymentDate;
        PaymentMethod = paymentMethod;
        AmountDue = amountDue;
        AmountPaid = amountPaid;
        ChangeAmount = changeAmount;
        PaymentStatus = paymentStatus; 
    }
    //Constructor to be utilized by JsonSerializer.Deserialize()
    public Payment()
    {
        
    }
}