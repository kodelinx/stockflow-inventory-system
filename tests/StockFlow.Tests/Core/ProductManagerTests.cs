using StockFlow.Models;
using StockFlow.Services;

namespace StockFlow.Tests.Core;

public class ProductManagerTests
{
    [Fact]
    public void CreateProduct_ValidValues_CreatesActiveProductAndTrimsText()
    {
        // Arrange
        ProductManager productManager =
            new ProductManager();

        // Act
        Product product =
            productManager.CreateProduct(
                "  PRD-001  ",
                "  Wireless Mouse  ",
                "  Accessories  ",
                500.00m,
                10,
                3
            );

        // Assert
        Assert.Equal(
            "PRD-001",
            product.ProductCode
        );

        Assert.Equal(
            "Wireless Mouse",
            product.Name
        );

        Assert.Equal(
            "Accessories",
            product.Category
        );

        Assert.Equal(
            500.00m,
            product.UnitPrice
        );

        Assert.Equal(
            10,
            product.QuantityInStock
        );

        Assert.Equal(
            3,
            product.ReorderLevel
        );

        Assert.True(
            product.IsActive
        );
    }

    [Theory]
    [InlineData(10, 5, false)]
    [InlineData(6, 5, false)]
    [InlineData(5, 5, true)]
    [InlineData(4, 5, true)]
    [InlineData(0, 5, true)]
    public void IsLowStock_VariousStockLevels_ReturnsExpected(
        int quantityInStock,
        int reorderLevel,
        bool expected)
    {
        // Arrange
        ProductManager productManager =
            new ProductManager();

        Product product =
            new Product
            {
                QuantityInStock =
                    quantityInStock,

                ReorderLevel =
                    reorderLevel
            };

        // Act
        bool result =
            productManager.IsLowStock(
                product
            );

        // Assert
        Assert.Equal(
            expected,
            result
        );
    }

    [Theory]
    [InlineData(10, 5, true)]
    [InlineData(10, 10, true)]
    [InlineData(10, 11, false)]
    [InlineData(1, 1, true)]
    [InlineData(0, 1, false)]
    public void HasEnoughStock_VariousRequestedQuantities_ReturnsExpected(
        int quantityInStock,
        int requestedQuantity,
        bool expected)
    {
        // Arrange
        ProductManager productManager =
            new ProductManager();

        Product product =
            new Product
            {
                QuantityInStock =
                    quantityInStock
            };

        // Act
        bool result =
            productManager.HasEnoughStock(
                product,
                requestedQuantity
            );

        // Assert
        Assert.Equal(
            expected,
            result
        );
    }

    [Fact]
    public void DeactiveProduct_ActiveProduct_SetsIsActiveFalse()
    {
        // Arrange
        ProductManager productManager =
            new ProductManager();

        Product product =
            new Product
            {
                ProductCode = "PRD-001",
                Name = "Wireless Mouse",
                IsActive = true
            };

        // Act
        productManager.DeactiveProduct(
            product
        );

        // Assert
        Assert.False(
            product.IsActive
        );
    }
}