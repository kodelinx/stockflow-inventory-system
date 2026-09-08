using Microsoft.AspNetCore.Mvc;

namespace StockFlow.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    [HttpGet("summary")]
    public IActionResult GetDashboardSummary()
    {
        SampleDashboardSummary summary = GetSampleDashboardSummary();

        return Ok(summary);
    }

    private static SampleDashboardSummary GetSampleDashboardSummary()
    {
        return new SampleDashboardSummary(
            TotalProducts: 3,
            LowStockProducts: 1,
            TotalOrders: 3,
            CompletedOrders: 2,
            PendingOrders: 1,
            TotalPayments: 3,
            TotalSalesIncome: 1750.00m
        );
    }

    private record SampleDashboardSummary(
        int TotalProducts,
        int LowStockProducts,
        int TotalOrders,
        int CompletedOrders,
        int PendingOrders,
        int TotalPayments,
        decimal TotalSalesIncome
    );
}