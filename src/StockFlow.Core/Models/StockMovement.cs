namespace StockFlow.Models;

public class StockMovement
{
    public int StockMovementId { get; set; }
    public int ProductId { get; set; }
    public string ProductCode { get; set; } = String.Empty;
    public string ProductName { get; set; } = String.Empty;
    public string MovementType { get; set; } = String.Empty;
    public int QuantityChanged { get; set; }
    public int StockBefore { get; set; }
    public int StockAfter { get; set; }
    public string Reason { get; set; } = String.Empty;
    public DateTime MovementDate { get; set; }
    public string ReferenceNumber { get; set; } = String.Empty;

    public StockMovement()
    {
        
    }

    public StockMovement(
        int stockMovementId,
        int productId,
        string productCode,
        string productName,
        string movementType,
        int quantityChanged,
        int stockBefore,
        int stockAfter,
        string reason,
        DateTime movementDate, 
        string referenceNumber
    )
    {
        StockMovementId = stockMovementId;
        ProductId = productId;
        ProductCode = productCode;
        ProductName = productName;
        MovementType = movementType;
        QuantityChanged = quantityChanged;
        StockBefore = stockBefore;
        StockAfter = stockAfter;
        Reason = reason;
        MovementDate = movementDate;
        ReferenceNumber = referenceNumber;
    }
}