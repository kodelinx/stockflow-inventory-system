using Microsoft.AspNetCore.Mvc;
using StockFlow.Models;
using StockFlow.Repositories;
using StockFlow.Services;

namespace StockFlow.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductRepository _productRepository;
    private readonly ProductManager _productManager;

    public ProductsController(
        ProductRepository productRepository,
        ProductManager productManager
    )
    {
        _productRepository = productRepository;
        _productManager = productManager;
    }

    [HttpGet]
    public IActionResult GetProducts()
    {
        
        List<Product> products = _productRepository.GetActiveProducts();

        return Ok(products);
    }

    [HttpGet("{productCode}")]
    public IActionResult GetProductByCode(string productCode)
    {
        if (string.IsNullOrWhiteSpace(productCode))
        {
            return BadRequest("Product code is required");
        }

        productCode = productCode.Trim();

        Product? product = _productRepository.FindProductByCode(productCode);

        if (product == null)
        {
            return NotFound($"Product with code {productCode} was not found");
        }

        bool isLowStock = _productManager.IsLowStock(product);

        var response = new
        {
            product.ProductId,
            product.ProductCode,
            product.Name,
            product.Category,
            product.UnitPrice,
            product.QuantityInStock,
            product.ReorderLevel,
            product.IsActive,
            IsLowStock = isLowStock
        };

        return Ok(response);
    }
}