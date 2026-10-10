using StockFlow.Models;
using StockFlow.Repositories;
using StockFlow.Tests.Helpers;

namespace StockFlow.Tests.Repositories;

public class ProductRepositoryTests
{
    // [Fact] is an xUnit attribute that identifies this method as a test.
    // When "dotnet test" runs, xUnit discovers and executes this method.
    //
    // Test naming convention: MethodBeingTested_Scenario_ExpectedResult
    [Fact]
    public void AddProduct_ValidProduct_PersistsProduct()
    {
        // Arrange - Prepare everything needed
        // Creates a new temporary database with the StockFlow tables.
        //
        // "using" ensures Dispose() is called when this method ends,
        // even if the test fails by throwing an exception.
        using TestDatabase testDatabase =
            new TestDatabase();

        // Creates a ProductRepository instance.
        //
        // The repository receives the temporary database connection
        // service rather than the normal application connection.
        ProductRepository repository =
            new ProductRepository(
                testDatabase.DatabaseConnectionService
            );

        Product product = new Product
        {
            ProductCode = "TEST-P001",
            Name = "Test Mouse",
            Category = "Accessories",
            UnitPrice = 500.00m,
            QuantityInStock = 15,
            ReorderLevel = 5,
            IsActive = true
        };

        // Act - Perform the behavior being tested
        repository.AddProduct(product);

        Product? savedProduct =
            repository.FindProductByCode(
                "TEST-P001"
            );

        // Assert - Verify the result
        Assert.NotNull(savedProduct);

        Assert.Equal(
            "TEST-P001",
            savedProduct.ProductCode
        );

        Assert.Equal(
            "Test Mouse",
            savedProduct.Name
        );

        Assert.Equal(
            500.00m,
            savedProduct.UnitPrice
        );

        Assert.Equal(
            15,
            savedProduct.QuantityInStock
        );

        Assert.True(
            savedProduct.IsActive
        );
    }

    [Fact]
    public void FindProductByCode_UnknownCode_ReturnsNull()
    {
        // Arrange
        using TestDatabase testDatabase = new TestDatabase();

        ProductRepository repository = new ProductRepository(testDatabase.DatabaseConnectionService);

        // Act
        Product? product = repository.FindProductByCode("DOES-NOT-EXIST");

        // Assert
        Assert.Null(product);
    }

    [Fact]
    public void UpdateProduct_ExistingProduct_PersistsChange()
    {
        //Arrange
        using TestDatabase testDatabase = new TestDatabase();

        ProductRepository repository = new ProductRepository(
            testDatabase.DatabaseConnectionService
        );

        Product product = new Product
        {
            ProductCode = "TEST-P002",
            Name = "Keyboard",
            Category = "Accessories",
            UnitPrice = 750.00m,
            QuantityInStock = 10,
            ReorderLevel = 3,
            IsActive = true
        };

        repository.AddProduct(product);

        Product? savedProduct = repository.FindProductByCode("TEST-P002");

        Assert.NotNull(savedProduct);

        savedProduct.Name = "Mechanical Keyboard";

        savedProduct.UnitPrice = 1500.00m;

        savedProduct.QuantityInStock = 20;

        //Act
        repository.UpdateProduct(
        savedProduct
    );

    Product? updatedProduct =
        repository.FindProductByCode(
            "TEST-P002"
        );

    // Assert
    Assert.NotNull(updatedProduct);

    Assert.Equal(
        "Mechanical Keyboard",
        updatedProduct.Name
    );

    Assert.Equal(
        1500.00m,
        updatedProduct.UnitPrice
    );

    Assert.Equal(
        20,
        updatedProduct.QuantityInStock
    );

    }

    [Fact]
    public void DeactivateProduct_ActiveProduct_SetsIsActiveFalse()
    {
        // Arrange
        using TestDatabase testDatabase = new TestDatabase();

        ProductRepository repository =
            new ProductRepository(
                testDatabase.DatabaseConnectionService
            );

        Product product = new Product
        {
            ProductCode = "TEST-P003",
            Name = "Webcam",
            Category = "Accessories",
            UnitPrice = 1200.00m,
            QuantityInStock = 8,
            ReorderLevel = 2,
            IsActive = true
        };

        repository.AddProduct(product);

        // Act
        repository.DeactivateProduct(
            "TEST-P003"
        );

        Product? deactivatedProduct =
            repository.FindProductByCode(
                "TEST-P003"
            );

        // Assert
        Assert.NotNull(
            deactivatedProduct
        );

        Assert.False(
            deactivatedProduct.IsActive
        );
    }

    [Fact]
    public void GetActiveProducts_InactiveProduct_ExcludesProduct()
    {
        // Arrange
        using TestDatabase testDatabase =
            new TestDatabase();

        ProductRepository repository =
            new ProductRepository(
                testDatabase.DatabaseConnectionService
            );

        Product product = new Product
        {
            ProductCode = "TEST-P004",
            Name = "Inactive Test Product",
            Category = "Testing",
            UnitPrice = 100.00m,
            QuantityInStock = 5,
            ReorderLevel = 1,
            IsActive = true
        };

        repository.AddProduct(product);

        repository.DeactivateProduct(
            "TEST-P004"
        );

        // Act
        List<Product> activeProducts =
            repository.GetActiveProducts();

        bool inactiveProductWasReturned =
            activeProducts.Any(product =>
                product.ProductCode == "TEST-P004"
            );

        // Assert
        Assert.False(
            inactiveProductWasReturned
        );
    }

    [Fact]
    public void ReactivateProduct_InactiveProduct_SetsIsActiveTrue()
    {
        // Arrange
        using TestDatabase testDatabase =
            new TestDatabase();

        ProductRepository repository =
            new ProductRepository(
                testDatabase.DatabaseConnectionService
            );

        Product product = new Product
        {
            ProductCode = "TEST-P005",
            Name = "Test Monitor",
            Category = "Display",
            UnitPrice = 4000.00m,
            QuantityInStock = 4,
            ReorderLevel = 1,
            IsActive = true
        };

        repository.AddProduct(product);

        repository.DeactivateProduct(
            "TEST-P005"
        );

        // Act
        repository.ReactivateProduct(
            "TEST-P005"
        );

        Product? reactivatedProduct =
            repository.FindProductByCode(
                "TEST-P005"
            );

        // Assert
        Assert.NotNull(
            reactivatedProduct
        );

        Assert.True(
            reactivatedProduct.IsActive
        );
    }
    
    // does not yet test deleting Products with transaction history.
    [Fact]
    public void DeleteProduct_ExistingProduct_RemovesProduct()
    {
        // Arrange
        using TestDatabase testDatabase =
            new TestDatabase();

        ProductRepository repository =
            new ProductRepository(
                testDatabase.DatabaseConnectionService
            );

        Product product = new Product
        {
            ProductCode = "TEST-P006",
            Name = "Delete Me",
            Category = "Testing",
            UnitPrice = 1.00m,
            QuantityInStock = 1,
            ReorderLevel = 0,
            IsActive = true
        };

        repository.AddProduct(product);

        Assert.NotNull(
            repository.FindProductByCode(
                "TEST-P006"
            )
        );

        // Act
        repository.DeleteProduct(
            "TEST-P006"
        );

        Product? deletedProduct =
            repository.FindProductByCode(
                "TEST-P006"
            );

        // Assert
        Assert.Null(
            deletedProduct
        );
    }

    [Fact]
    public void GetAllProducts_AddedProduct_IncludesProduct()
    {
        // Arrange
        using TestDatabase testDatabase =
            new TestDatabase();

        ProductRepository repository =
            new ProductRepository(
                testDatabase.DatabaseConnectionService
            );

        Product product = new Product
        {
            ProductCode = "TEST-P007",
            Name = "USB Hub",
            Category = "Accessories",
            UnitPrice = 900.00m,
            QuantityInStock = 7,
            ReorderLevel = 2,
            IsActive = true
        };

        repository.AddProduct(product);

        // Act
        List<Product> products =
            repository.GetAllProducts();

        bool productExists =
            products.Any(product =>
                product.ProductCode == "TEST-P007"
            );

        // Assert
        Assert.True(productExists);
    }

}