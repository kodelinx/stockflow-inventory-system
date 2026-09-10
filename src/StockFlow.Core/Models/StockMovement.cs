namespace StockFlow.Models;

public class StockMovement
{
    public int StockMovementId { get; set; }
    public string ProductCode { get; set; } = String.Empty;
    public string ProductName { get; set; } = String.Empty;
    public string MovementType { get; set; } = String.Empty;
    public int QuantityChanged { get; set; }
    public int StockBefore { get; set; }
    public int StockAfter { get; set; }
    public string Reason { get; set; } = String.Empty;
    public DateTime MovementDate { get; set; }

    public StockMovement()
    {
        
    }

    public StockMovement(
        int stockMovementId,
        string productCode,
        string productName,
        string movementType,
        int quantityChanged,
        int stockBefore,
        int stockAfter,
        string reason,
        DateTime movementDate 
    )
    {
        StockMovementId = stockMovementId;
        ProductCode = productCode;
        ProductName = productName;
        MovementType = movementType;
        QuantityChanged = quantityChanged;
        StockBefore = stockBefore;
        StockAfter = stockAfter;
        Reason = reason;
        MovementDate = movementDate;
    }
}