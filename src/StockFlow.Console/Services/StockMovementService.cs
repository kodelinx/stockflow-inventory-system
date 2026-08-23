using System.Net.Http.Headers;
using StockFlow.Models;
using StockFlow.Utilities;

namespace StockFlow.Services;

public class StockMovementService
{
    private readonly InputValidationService _inputValidationService;

    public StockMovementService(InputValidationService inputValidationService)
    {
        _inputValidationService = inputValidationService;
    }

    public void AddStock(List<Product> products, List<StockMovement> stockMovements)
    {
        string productCode = _inputValidationService.GetRequiredText("Enter product code: ");

        Product? product = products.FirstOrDefault(product => 
            product.ProductCode.Equals(productCode, StringComparison.OrdinalIgnoreCase)
        );

        if(product == null)
        {
            Console.WriteLine("The product does not exist.");
            return;
        }

        int quantityChanged = _inputValidationService.GetValidInt("Quantity to add: ", 1, 1000000);
        string reason = _inputValidationService.GetRequiredText("Reason: ");

        int stockBefore = product.QuantityInStock;
        CalculateStockChange(product, quantityChanged);
        int stockAfter = product.QuantityInStock;

        RecordMovement(
            stockMovements,
            product,
            "Stock In",
            stockBefore,
            quantityChanged,
            stockAfter,
            reason
        );

        Console.WriteLine("The stock has been added successfully.");
    }
    public void AdjustStock(List<Product> products, List<StockMovement> stockMovements)
    {
        string productCode = _inputValidationService.GetRequiredText("Enter product code: ");

        Product? product = products.FirstOrDefault(product => 
            product.ProductCode.Equals(productCode, StringComparison.OrdinalIgnoreCase)
        );

        if (product == null)
        {
            Console.WriteLine("The product does not exist.");
            return;
        }

        int newStockQuantity = _inputValidationService.GetValidInt("Input new stock quantity: ", 0, 1000000);
        string reason = _inputValidationService.GetRequiredText("Reason: ");

        int stockBefore = product.QuantityInStock;
        int quantityChanged = newStockQuantity - stockBefore;
        product.QuantityInStock = newStockQuantity;
        int stockAfter = product.QuantityInStock;

        RecordMovement(
            stockMovements,
            product,
            "Adjustment",
            stockBefore,
            quantityChanged,
            stockAfter,
            reason
        );

        Console.WriteLine("The stock has been adjusted successfully.");
    }

    public void RecordSaleStockOut(
        List<StockMovement> stockMovements,
        Product product,
        int quantitySold,
        int stockBefore,
        int stockAfter,
        string orderNumber
    )
    {
        RecordMovement(
            stockMovements,
            product,
            "Stock Out",
            stockBefore,
            -quantitySold,
            stockAfter,
            $"Sold through order {orderNumber}"
        );
    }
    public void ViewStockMovements(List<StockMovement> stockMovements)
    {
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
            Console.WriteLine($"Date: {movement.MovementDate}");
            Console.WriteLine("----------------------");
        }
    }

    public void RecordMovement(
        List<StockMovement> stockMovements,
        Product product,
        string movementType,
        int stockBefore,
        int quantityChanged,
        int stockAfter,
        string reason
    )
    {
        int stockMovementId  = stockMovements.Count + 1;

        StockMovement stockMovement = new StockMovement(
            stockMovementId,
            product.ProductCode,
            product.Name,
            movementType,
            quantityChanged,
            stockBefore,
            stockAfter,
            reason,
            DateTime.Now
        );

        stockMovements.Add(stockMovement);
    }

    public void CalculateStockChange(Product product, int quantityChanged)
    {
        product.QuantityInStock += quantityChanged;
    }
}