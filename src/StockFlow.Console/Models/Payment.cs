using StockFlow.Models;
namespace StockFlow.Models;

public class Payment
{
    public int PaymentId { get; set; }
    public string PaymentNumber { get; set; }
    public string OrderNumber { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; }
    public decimal AmountDue { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal ChangeAmount { get; set; }
    public string PaymentStatus { get; set; }

    public Payment(
        int paymentId,
        string paymentNumber,
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
        OrderNumber = orderNumber;
        PaymentDate = paymentDate;
        PaymentMethod = paymentMethod;
        AmountDue = amountDue;
        AmountPaid = amountPaid;
        ChangeAmount = changeAmount;
        PaymentStatus = paymentStatus; 
    }
}