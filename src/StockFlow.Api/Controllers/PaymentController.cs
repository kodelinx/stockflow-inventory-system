using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Api.Security;
using StockFlow.Models;
using StockFlow.Repositories;

namespace StockFlow.Api.Controllers;

[ApiController]
[Route ("api/payments")]
[Authorize(Policy = AuthorizationPolicies.StaffOrAdmin)]
public class PaymentController : ControllerBase
{
    private readonly PaymentRepository _paymentRepository;

    public PaymentController(PaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    [HttpGet]
    public IActionResult GetPayments()
    {
        List<Payment> payments = _paymentRepository.GetAllPayments();

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

        List<Payment> payments = _paymentRepository.GetAllPayments();

        Payment? payment = payments.FirstOrDefault(payment => 
            payment.PaymentNumber.Equals(paymentNumber, StringComparison.OrdinalIgnoreCase)
        );

        if(payment == null)
        {
            return NotFound($"Payment with number {paymentNumber} was not found");
        }
        return Ok(payment);
    }
}