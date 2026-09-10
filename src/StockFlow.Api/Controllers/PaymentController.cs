using Microsoft.AspNetCore.Mvc;
using StockFlow.Models;

namespace StockFlow.Api.Controllers;

[ApiController]
[Route ("api/payments")]
public class PaymentController : ControllerBase
{
    [HttpGet]
    public IActionResult GetPayments()
    {
        List<SamplePayment> payments = GetSamplePayments();

        return Ok(payments);
    }

    [HttpGet("{paymentNumber}")]
    public IActionResult GetPaymentByNumber(string paymentNumber)
    {
        if (string.IsNullOrWhiteSpace(paymentNumber))
        {
            return BadRequest("Payment number is required");
        }

        paymentNumber = paymentNumber.Trim();

        List<SamplePayment> payments = GetSamplePayments();

        SamplePayment? payment = payments.FirstOrDefault(payment => 
            payment.PaymentNumber.Equals(paymentNumber, StringComparison.OrdinalIgnoreCase)
        );

        if(payment == null)
        {
            return NotFound($"Payment with number {paymentNumber} was not found");
        }
        return Ok(payment);
    }

    private static List<SamplePayment> GetSamplePayments()
    {
        return new List<SamplePayment>
        {
            new SamplePayment(
                "PAY-001",
                "ORD-001",
                DateTime.Now.AddDays(-2),
                "Cash",
                1000.00m,
                1000.00m,
                0.00m,
                "Paid"
            ),
            new SamplePayment(
                "PAY-002",
                "ORD-002",
                DateTime.Now.AddDays(-1),
                "GCash",
                750.00m,
                800.00m,
                50.00m,
                "Paid"
            ),
            new SamplePayment(
                "PAY-003",
                "ORD-003",
                DateTime.Now,
                "Pending",
                1500.00m,
                0.00m,
                0.00m,
                "Unpaid"
            )
        };
    }

    private record SamplePayment(
        string PaymentNumber,
        string OrderNumber,
        DateTime PaymentDate,
        string PaymentMethod,
        decimal AmountDue,
        decimal AmountPaid,
        decimal ChangeAmount,
        string PaymentStatus
    );
}