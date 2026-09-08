using Microsoft.AspNetCore.Mvc;

namespace StockFlow.Api.Contorllers;

[ApiController]
[Route("api/orders")]
public class OrderControllers : ControllerBase
{
    [HttpGet]
    public IActionResult GetOrders()
    {
        var orders = GetSampleOrders();

        return Ok(orders);
    }

    [HttpGet("{orderNumber}")]
    public IActionResult GetOrderByNumber(string orderNumber)
    {
        var orders = GetSampleOrders();

        var order = orders.FirstOrDefault(order => 
            order.OrderNumber.Equals(orderNumber, StringComparison.OrdinalIgnoreCase)
        );

        if (order == null)
        {
            return NotFound($"Order with number {orderNumber} was not found");
        }

        return Ok(order);
    }

    private static List<SampleOrder> GetSampleOrders()
    {
        return new List<SampleOrder>
        {
            new SampleOrder(
                "ORD-001",
                DateTime.Now.AddDays(-2),
                1000.00m,
                "Completed",
                "Paid"
            ),
            new SampleOrder(
                "ORD-002",
                DateTime.Now.AddDays(-1),
                750.00m,
                "Pending",
                "Unpaid"
            )

        };
    }

    private record SampleOrder(
        string OrderNumber,
        DateTime OrderDate,
        decimal TotalAmount,
        string OrderStatus,
        string PaymentStatus
    );
}