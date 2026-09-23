using StockFlow.Models;
using StockFlow.Repositories;
using StockFlow.Utilities;

namespace StockFlow.Services;

public class StockMovementService
{
    private readonly InputValidationService _inputValidationService;
    private readonly StockMovementRepository _stockMovementRepository;
    private readonly ProductRepository _productRepository;

    public StockMovementService(
        InputValidationService inputValidationService,
        StockMovementRepository stockMovementRepository,
        ProductRepository productRepository
    )
    {
        _inputValidationService = inputValidationService;
        _stockMovementRepository = stockMovementRepository;
        _productRepository = productRepository;
    }

    public void AddStock()
    {
        string productCode = _inputValidationService.GetRequiredText("Enter product code: ");

        Product? product = _productRepository.FindProductByCode(productCode);

        if (product == null)
        {
            Console.WriteLine("The product does not exist or is deactivated.");
            return;
        }

        int quantityChanged = _inputValidationService.GetValidInt("Quantity to add: ", 1, 1000000);
        string reason = _inputValidationService.GetRequiredText("Reason: ");

        int stockBefore = product.QuantityInStock;

        // Adds the entered quantity to the current product stock.
        CalculateStockChange(product, quantityChanged);

        int stockAfter = product.QuantityInStock;

        _productRepository.UpdateProduct(product);

        // Records this stock increase in the movement history.
        RecordMovement(
            product,
            "Stock In",
            stockBefore,
            quantityChanged,
            stockAfter,
            reason,
            string.Empty
        );

        Console.WriteLine("The stock has been added successfully.");
    }

    public void AdjustStock()
    {
        string productCode = _inputValidationService.GetRequiredText("Enter product code: ");

        Product? product = _productRepository.FindProductByCode(productCode);

        if (product == null)
        {
            Console.WriteLine("The product does not exist or is deactivated.");
            return;
        }

        int newStockQuantity = _inputValidationService.GetValidInt("Input new stock quantity: ", 0, 1000000);
        string reason = _inputValidationService.GetRequiredText("Reason: ");

        int stockBefore = product.QuantityInStock;

        // Calculates the difference between old stock and new stock.
        int quantityChanged = newStockQuantity - stockBefore;

        product.QuantityInStock = newStockQuantity;

        int stockAfter = product.QuantityInStock;

        // Saves the updated product stock to SQLite.
        _productRepository.UpdateProduct(product);

        // Records the stock adjustment in the movement history.
        RecordMovement(
            product,
            "Adjustment",
            stockBefore,
            quantityChanged,
            stockAfter,
            reason,
            string.Empty
        );

        Console.WriteLine("The stock has been adjusted successfully.");
    }

    public void RecordSaleStockOut(
        Product product,
        int quantitySold,
        int stockBefore,
        int stockAfter,
        string orderNumber)
    {
        // Records stock decrease caused by a completed sale/order.
        RecordMovement(
            product,
            "Stock Out",
            stockBefore,
            -quantitySold,
            stockAfter,
            $"Sold through order {orderNumber}",
            orderNumber
        );
    }

    public void ViewStockMovements()
    {
        List<StockMovement> stockMovements = _stockMovementRepository.GetAllStockMovements();
        if (stockMovements.Count == 0)
        {
            Console.WriteLine("No stock movements available.\n");
            return;
        }

        Console.WriteLine("\nStock Movement History");
        Console.WriteLine("----------------------");

        foreach (StockMovement movement in stockMovements)
        {
            Console.WriteLine($"ID: {movement.StockMovementId}");
            Console.WriteLine($"Product Code: {movement.ProductCode}");
            Console.WriteLine($"Product Name: {movement.ProductName}");
            Console.WriteLine($"Movement Type: {movement.MovementType}");
            Console.WriteLine($"Quantity Changed: {movement.QuantityChanged}");
            Console.WriteLine($"Stock Before: {movement.StockBefore}");
            Console.WriteLine($"Stock After: {movement.StockAfter}");
            Console.WriteLine($"Reason: {movement.Reason}");
            Console.WriteLine($"Reference Number: {movement.ReferenceNumber}");
            Console.WriteLine($"Date: {movement.MovementDate}");
            Console.WriteLine("----------------------");
        }
    }

    public void RecordMovement(
        Product product,
        string movementType,
        int stockBefore,
        int quantityChanged,
        int stockAfter,
        string reason,
        string referenceNumber)
    {
        // Creates the next temporary movement ID for list/JSON flow.
        // int stockMovementId = stockMovements.Count + 1;

        // Creates one stock movement history record.
        StockMovement stockMovement = new StockMovement
        {
            ProductId = product.ProductId,
            ProductCode = product.ProductCode,
            ProductName = product.Name,
            MovementType = movementType,
            QuantityChanged = quantityChanged,
            StockBefore = stockBefore,
            StockAfter = stockAfter,
            Reason = reason,
            MovementDate = DateTime.Now,
            ReferenceNumber = referenceNumber
        };

        _stockMovementRepository.AddStockMovement(stockMovement);
    }

    public void CalculateStockChange(Product product, int quantityChanged)
    {
        // Updates the product's current stock quantity.
        product.QuantityInStock += quantityChanged;
    }
}