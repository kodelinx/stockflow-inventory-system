using Microsoft.AspNetCore.Authorization;
using StockFlow.Api.Security;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Models;
using StockFlow.Repositories;

namespace StockFlow.Api.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize(Policy = AuthorizationPolicies.StaffOrAdmin)]
public class OrdersController : ControllerBase
{
    private readonly OrderRepository _orderRepository;
    
    public OrdersController(OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [HttpGet]
    public IActionResult GetOrders()
    {
        List<Order> orders = _orderRepository.GetAllOrders();

        return Ok(orders);
    }

    [HttpGet("{orderNumber}")]
    public IActionResult GetOrderByNumber(string orderNumber)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
        {
            return BadRequest("Order number is required");
        }

        orderNumber = orderNumber.Trim();

        var orders = _orderRepository.GetAllOrders();

        var order = orders.FirstOrDefault(order => 
            order.OrderNumber.Equals(orderNumber, StringComparison.OrdinalIgnoreCase)
        );

        if (order == null)
        {
            return NotFound($"Order with number {orderNumber} was not found");
        }

        return Ok(order);
    }
}