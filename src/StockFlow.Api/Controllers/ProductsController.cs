using Microsoft.AspNetCore.Mvc;

namespace StockFlow.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProducts()
    {
        var products = new[]
        {
            new
            {
                ProductCode = "P001",
                Name = "Mouse",
                Category = "Accessories",
                UnitPrice = 250.00,
                QuantityInStock = 20,
                ReorderLevel = 5,
                IsActive = true
            },
            new
            {
                ProductCode = "P002",
                Name = "Keyboard",
                Category = "Accessories",
                UnitPrice = 750.00,
                QuantityInStock = 10,
                ReorderLevel = 3,
                IsActive = true
            }
        };

        return Ok(products);
    }

    [HttpGet("{productCode}")]
    public IActionResult GetProductByCode(string productCode)
    {
        var products = new[]
        {
            new
            {
                ProductCode = "P001",
                Name = "Mouse",
                Category = "Accessories",
                UnitPrice = 250.00,
                QuantityInStock = 20,
                ReorderLevel = 5,
                IsActive = true
            },
            new
            {
                ProductCode = "P002",
                Name = "Keyboard",
                Category = "Accessories",
                UnitPrice = 750.00,
                QuantityInStock = 10,
                ReorderLevel = 3,
                IsActive = true
            }
        };

        var product = products.FirstOrDefault(product =>
            product.ProductCode.Equals(productCode, StringComparison.OrdinalIgnoreCase)
        );

        if (product == null)
        {
            return NotFound($"Product with code {productCode} was not found.");
        }

        return Ok(product);
    }
}