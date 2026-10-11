using Microsoft.AspNetCore.Mvc;
using StockFlow.Models;
using StockFlow.Repositories;
using StockFlow.Services;

namespace StockFlow.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly ProductRepository _productRepository;
    private readonly OrderRepository _orderRepository;
    private readonly PaymentRepository _paymentRepository;
    private readonly ProductManager _productManager;

    public DashboardController(
        ProductRepository productRepository,
        OrderRepository orderRepository,
        PaymentRepository paymentRepository,
        ProductManager productManager
    )
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
        _productManager = productManager;
    }

    [HttpGet("summary")]
    public IActionResult GetDashboardSummary()
    {
        List<Product> products = _productRepository.GetAllProducts();
        List<Order> orders = _orderRepository.GetAllOrders();
        List<Payment> payments = _paymentRepository.GetAllPayments();

        int totalProducts = products.Count();

        int lowStockProducts = products.Count(product => 
            product.IsActive &&
            _productManager.IsLowStock(product)
        );

        int totalOrders = orders.Count();

        int completedOrders = orders.Count(order => 
            order.OrderStatus.Equals(
                "Completed", 
                StringComparison.OrdinalIgnoreCase
            )
        );

        int pendingOrders = orders.Count(order => 
            !order.OrderStatus.Equals(
                "Completed",
                StringComparison.OrdinalIgnoreCase
            )
        );

        List<Payment> paidPayments = payments.Where(payment => 
            payment.PaymentStatus.Equals(
                "Paid", 
                StringComparison.OrdinalIgnoreCase
            )
        ).ToList();

        int totalPayments = paidPayments.Count;

        decimal totalSalesIncome = paidPayments.Sum(payment => payment.AmountDue);

        var summary = new
        {
            TotalProducts = totalProducts,
            LowStockProducts = lowStockProducts,
            TotalOrders = totalOrders,
            CompletedOrders = completedOrders,
            PendingOrders = pendingOrders,
            TotalPayments = totalPayments,
            TotalSalesIncome = totalSalesIncome
        };

        return Ok(summary);
    }
}