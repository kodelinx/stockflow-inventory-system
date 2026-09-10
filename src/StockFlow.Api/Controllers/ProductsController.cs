using Microsoft.AspNetCore.Mvc;
using StockFlow.Models;
using StockFlow.Repositories;

namespace StockFlow.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductRepository _productRepository;

    public ProductsController(ProductRepository productRepository)
    {
        _productRepository = productRepository;
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

        return Ok(product);
    }
}