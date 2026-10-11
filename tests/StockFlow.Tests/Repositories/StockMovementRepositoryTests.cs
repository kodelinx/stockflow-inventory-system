using StockFlow.Models;
using StockFlow.Repositories;
using StockFlow.Tests.Helpers;

namespace StockFlow.Tests.Repositories;

public class StockMovementRepositoryTests
{
    [Fact]
    public void AddStockMovement_ValidMovement_PersistsMovement()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        ProductRepository productRepository =
            new ProductRepository(
                testDatabase.DatabaseConnectionService
            );

        StockMovementRepository stockMovementRepository =
            new StockMovementRepository(
                testDatabase.DatabaseConnectionService
            );

        Product product = new Product
        {
            ProductCode = "TEST-STOCK-001",
            Name = "Test Mouse",
            Category = "Accessories",
            UnitPrice = 500.00m,
            QuantityInStock = 10,
            ReorderLevel = 2,
            IsActive = true
        };

        productRepository.AddProduct(product);

        Product? savedProduct =
            productRepository.FindProductByCode(
                "TEST-STOCK-001"
            );

        Assert.NotNull(savedProduct);

        StockMovement movement =
            new StockMovement
            {
                ProductId = savedProduct.ProductId,
                ProductCode = savedProduct.ProductCode,
                ProductName = savedProduct.Name,
                MovementType = "Stock In",
                QuantityChanged = 5,
                StockBefore = 10,
                StockAfter = 15,
                Reason = "Integration test stock in",
                MovementDate = DateTime.Now,
                ReferenceNumber = "TEST-REF-001"
            };

        stockMovementRepository
            .AddStockMovement(movement);

        List<StockMovement> movements =
            stockMovementRepository
                .GetStockMovementsByProductCode(
                    "TEST-STOCK-001"
                );

        Assert.Single(movements);

        StockMovement savedMovement =
            movements[0];

        Assert.True(
            savedMovement.StockMovementId > 0
        );

        Assert.Equal(
            savedProduct.ProductId,
            savedMovement.ProductId
        );

        Assert.Equal(
            "TEST-STOCK-001",
            savedMovement.ProductCode
        );

        Assert.Equal(
            "Stock In",
            savedMovement.MovementType
        );

        Assert.Equal(
            5,
            savedMovement.QuantityChanged
        );

        Assert.Equal(
            10,
            savedMovement.StockBefore
        );

        Assert.Equal(
            15,
            savedMovement.StockAfter
        );

        Assert.Equal(
            "TEST-REF-001",
            savedMovement.ReferenceNumber
        );
    }

    [Fact]
    public void AddStockMovement_StockOut_PersistsNegativeQuantity()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        ProductRepository productRepository =
            new ProductRepository(
                testDatabase.DatabaseConnectionService
            );

        StockMovementRepository stockMovementRepository =
            new StockMovementRepository(
                testDatabase.DatabaseConnectionService
            );

        Product product = new Product
        {
            ProductCode = "TEST-STOCK-002",
            Name = "Keyboard",
            Category = "Accessories",
            UnitPrice = 1000.00m,
            QuantityInStock = 10,
            ReorderLevel = 2,
            IsActive = true
        };

        productRepository.AddProduct(product);

        Product? savedProduct =
            productRepository.FindProductByCode(
                "TEST-STOCK-002"
            );

        Assert.NotNull(savedProduct);

        StockMovement movement =
            new StockMovement
            {
                ProductId = savedProduct.ProductId,
                ProductCode = savedProduct.ProductCode,
                ProductName = savedProduct.Name,
                MovementType = "Stock Out",
                QuantityChanged = -3,
                StockBefore = 10,
                StockAfter = 7,
                Reason = "Test sale",
                MovementDate = DateTime.Now,
                ReferenceNumber = "TEST-ORD-001"
            };

        stockMovementRepository
            .AddStockMovement(movement);

        List<StockMovement> movements =
            stockMovementRepository
                .GetStockMovementsByProductCode(
                    "TEST-STOCK-002"
                );

        Assert.Single(movements);

        Assert.Equal(
            -3,
            movements[0].QuantityChanged
        );

        Assert.Equal(
            7,
            movements[0].StockAfter
        );
    }

    [Fact]
    public void GetStockMovementsByProductCode_TwoProducts_ReturnsOnlyRequestedProductMovements()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        ProductRepository productRepository =
            new ProductRepository(
                testDatabase.DatabaseConnectionService
            );

        StockMovementRepository stockMovementRepository =
            new StockMovementRepository(
                testDatabase.DatabaseConnectionService
            );

        Product mouse = new Product
        {
            ProductCode = "TEST-MOVE-MOUSE",
            Name = "Mouse",
            Category = "Accessories",
            UnitPrice = 500.00m,
            QuantityInStock = 10,
            ReorderLevel = 2,
            IsActive = true
        };

        Product keyboard = new Product
        {
            ProductCode = "TEST-MOVE-KEYBOARD",
            Name = "Keyboard",
            Category = "Accessories",
            UnitPrice = 1000.00m,
            QuantityInStock = 10,
            ReorderLevel = 2,
            IsActive = true
        };

        productRepository.AddProduct(mouse);
        productRepository.AddProduct(keyboard);

        Product? savedMouse =
            productRepository.FindProductByCode(
                "TEST-MOVE-MOUSE"
            );

        Product? savedKeyboard =
            productRepository.FindProductByCode(
                "TEST-MOVE-KEYBOARD"
            );

        Assert.NotNull(savedMouse);
        Assert.NotNull(savedKeyboard);

        StockMovement mouseMovement =
            new StockMovement
            {
                ProductId = savedMouse.ProductId,
                ProductCode = savedMouse.ProductCode,
                ProductName = savedMouse.Name,
                MovementType = "Stock Out",
                QuantityChanged = -2,
                StockBefore = 10,
                StockAfter = 8,
                Reason = "Mouse sale",
                MovementDate = DateTime.Now,
                ReferenceNumber = "TEST-ORD-A"
            };

        StockMovement keyboardMovement =
            new StockMovement
            {
                ProductId = savedKeyboard.ProductId,
                ProductCode = savedKeyboard.ProductCode,
                ProductName = savedKeyboard.Name,
                MovementType = "Stock Out",
                QuantityChanged = -1,
                StockBefore = 10,
                StockAfter = 9,
                Reason = "Keyboard sale",
                MovementDate = DateTime.Now,
                ReferenceNumber = "TEST-ORD-B"
            };

        stockMovementRepository
            .AddStockMovement(mouseMovement);

        stockMovementRepository
            .AddStockMovement(keyboardMovement);

        List<StockMovement> mouseMovements =
            stockMovementRepository
                .GetStockMovementsByProductCode(
                    "TEST-MOVE-MOUSE"
                );

        Assert.Single(mouseMovements);

        Assert.Equal(
            "TEST-MOVE-MOUSE",
            mouseMovements[0].ProductCode
        );

        Assert.DoesNotContain(
            mouseMovements,
            movement =>
                movement.ProductCode ==
                "TEST-MOVE-KEYBOARD"
        );
    }

    [Fact]
    public void GetStockMovementsByReferenceNumber_TwoReferences_ReturnsOnlyRequestedReference()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        ProductRepository productRepository =
            new ProductRepository(
                testDatabase.DatabaseConnectionService
            );

        StockMovementRepository stockMovementRepository =
            new StockMovementRepository(
                testDatabase.DatabaseConnectionService
            );

        Product product = new Product
        {
            ProductCode = "TEST-REF-P001",
            Name = "Monitor",
            Category = "Display",
            UnitPrice = 5000.00m,
            QuantityInStock = 20,
            ReorderLevel = 3,
            IsActive = true
        };

        productRepository.AddProduct(product);

        Product? savedProduct =
            productRepository.FindProductByCode(
                "TEST-REF-P001"
            );

        Assert.NotNull(savedProduct);

        StockMovement movementA =
            new StockMovement
            {
                ProductId = savedProduct.ProductId,
                ProductCode = savedProduct.ProductCode,
                ProductName = savedProduct.Name,
                MovementType = "Stock Out",
                QuantityChanged = -2,
                StockBefore = 20,
                StockAfter = 18,
                Reason = "Order A",
                MovementDate = DateTime.Now,
                ReferenceNumber = "TEST-ORD-A"
            };

        StockMovement movementB =
            new StockMovement
            {
                ProductId = savedProduct.ProductId,
                ProductCode = savedProduct.ProductCode,
                ProductName = savedProduct.Name,
                MovementType = "Stock Out",
                QuantityChanged = -1,
                StockBefore = 18,
                StockAfter = 17,
                Reason = "Order B",
                MovementDate = DateTime.Now,
                ReferenceNumber = "TEST-ORD-B"
            };

        stockMovementRepository
            .AddStockMovement(movementA);

        stockMovementRepository
            .AddStockMovement(movementB);

        List<StockMovement> movements =
            stockMovementRepository
                .GetStockMovementsByReferenceNumber(
                    "TEST-ORD-A"
                );

        Assert.Single(movements);

        Assert.Equal(
            "TEST-ORD-A",
            movements[0].ReferenceNumber
        );

        Assert.DoesNotContain(
            movements,
            movement =>
                movement.ReferenceNumber ==
                "TEST-ORD-B"
        );
    }

    // Tests repository when storing a blank ReferenceNumber as database NULL and converts database NULL back to an empty string when mapping the model.
    [Fact]
    public void AddStockMovement_NoReference_PersistsEmptyReferenceAfterRead()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        ProductRepository productRepository =
            new ProductRepository(
                testDatabase.DatabaseConnectionService
            );

        StockMovementRepository stockMovementRepository =
            new StockMovementRepository(
                testDatabase.DatabaseConnectionService
            );

        Product product = new Product
        {
            ProductCode = "TEST-NO-REF-001",
            Name = "USB Hub",
            Category = "Accessories",
            UnitPrice = 700.00m,
            QuantityInStock = 5,
            ReorderLevel = 1,
            IsActive = true
        };

        productRepository.AddProduct(product);

        Product? savedProduct =
            productRepository.FindProductByCode(
                "TEST-NO-REF-001"
            );

        Assert.NotNull(savedProduct);

        StockMovement movement =
            new StockMovement
            {
                ProductId = savedProduct.ProductId,
                ProductCode = savedProduct.ProductCode,
                ProductName = savedProduct.Name,
                MovementType = "Adjustment",
                QuantityChanged = 2,
                StockBefore = 5,
                StockAfter = 7,
                Reason = "Manual adjustment",
                MovementDate = DateTime.Now,
                ReferenceNumber = string.Empty
            };

        stockMovementRepository
            .AddStockMovement(movement);

        List<StockMovement> movements =
            stockMovementRepository
                .GetStockMovementsByProductCode(
                    "TEST-NO-REF-001"
                );

        Assert.Single(movements);

        Assert.Equal(
            string.Empty,
            movements[0].ReferenceNumber
        );
    }
}