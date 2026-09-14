using Microsoft.Data.Sqlite;
using StockFlow.Database;
using StockFlow.Models;

namespace StockFlow.Repositories;

public class StockMovementRepository
{
    private readonly DatabaseConnectionService _databaseConnectionService;

    public StockMovementRepository(DatabaseConnectionService databaseConnectionService)
    {
        _databaseConnectionService = databaseConnectionService;
    }

    public void AddStockMovement(StockMovement stockMovement)
    {
        // Opens a connection to the SQLite database.
        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        // Inserts one inventory movement record.
        string sql = @"
            INSERT INTO StockMovements
            (
                ProductId,
                ProductCode,
                ProductName,
                MovementType,
                QuantityChanged,
                StockBefore,
                StockAfter,
                Reason,
                MovementDate,
                ReferenceNumber
            )
            VALUES
            (
                @ProductId,
                @ProductCode,
                @ProductName,
                @MovementType,
                @QuantityChanged,
                @StockBefore,
                @StockAfter,
                @Reason,
                @MovementDate,
                @ReferenceNumber
            );
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        command.Parameters.AddWithValue("@ProductId", stockMovement.ProductId);
        command.Parameters.AddWithValue("@ProductCode", stockMovement.ProductCode);
        command.Parameters.AddWithValue("@ProductName", stockMovement.ProductName);
        command.Parameters.AddWithValue("@MovementType", stockMovement.MovementType);
        command.Parameters.AddWithValue("@QuantityChanged", stockMovement.QuantityChanged);
        command.Parameters.AddWithValue("@StockBefore", stockMovement.StockBefore);
        command.Parameters.AddWithValue("@StockAfter", stockMovement.StockAfter);
        command.Parameters.AddWithValue("@Reason", stockMovement.Reason);
        command.Parameters.AddWithValue("@MovementDate", stockMovement.MovementDate.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@ReferenceNumber", stockMovement.ReferenceNumber);

        command.ExecuteNonQuery();
    }

    public List<StockMovement> GetAllStockMovements()
    {
        // Stores all stock movement records from the database.
        List<StockMovement> stockMovements = new List<StockMovement>();

        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        // Gets all stock movement records.
        string sql = @"
            SELECT
                StockMovementId,
                ProductId,
                ProductCode,
                ProductName,
                MovementType,
                QuantityChanged,
                StockBefore,
                StockAfter,
                Reason,
                MovementDate,
                ReferenceNumber
            FROM StockMovements;
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            // Converts each database row into a StockMovement object.
            StockMovement stockMovement = MapReaderToStockMovement(reader);

            stockMovements.Add(stockMovement);
        }

        return stockMovements;
    }

    public List<StockMovement> GetStockMovementsByProductCode(string productCode)
    {
        // Stores movements connected to one product.
        List<StockMovement> stockMovements = new List<StockMovement>();

        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        // Gets all movement records for the selected product.
        string sql = @"
            SELECT
                StockMovementId,
                ProductId,
                ProductCode,
                ProductName,
                MovementType,
                QuantityChanged,
                StockBefore,
                StockAfter,
                Reason,
                MovementDate,
                ReferenceNumber
            FROM StockMovements
            WHERE ProductCode = @ProductCode;
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        command.Parameters.AddWithValue("@ProductCode", productCode);

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            // Converts each matching row into a StockMovement object.
            StockMovement stockMovement = MapReaderToStockMovement(reader);

            stockMovements.Add(stockMovement);
        }

        return stockMovements;
    }

    public List<StockMovement> GetStockMovementsByReferenceNumber(string referenceNumber)
    {
        // Stores movements connected to an order, receipt, or adjustment reference.
        List<StockMovement> stockMovements = new List<StockMovement>();

        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        // Gets movements linked to the selected reference number.
        string sql = @"
            SELECT
                StockMovementId,
                ProductId,
                ProductCode,
                ProductName,
                MovementType,
                QuantityChanged,
                StockBefore,
                StockAfter,
                Reason,
                MovementDate,
                ReferenceNumber
            FROM StockMovements
            WHERE ReferenceNumber = @ReferenceNumber;
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        command.Parameters.AddWithValue("@ReferenceNumber", referenceNumber);

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            // Converts each matching row into a StockMovement object.
            StockMovement stockMovement = MapReaderToStockMovement(reader);

            stockMovements.Add(stockMovement);
        }

        return stockMovements;
    }

    private StockMovement MapReaderToStockMovement(SqliteDataReader reader)
    {
        // Converts database column values into a StockMovement model.
        return new StockMovement
        {
            StockMovementId = reader.GetInt32(0),
            ProductId = reader.GetInt32(1),
            ProductCode = reader.GetString(2),
            ProductName = reader.GetString(3),
            MovementType = reader.GetString(4),
            QuantityChanged = reader.GetInt32(5),
            StockBefore = reader.GetInt32(6),
            StockAfter = reader.GetInt32(7),
            Reason = reader.GetString(8),
            MovementDate = DateTime.Parse(reader.GetString(9)),
            ReferenceNumber = reader.IsDBNull(10) ? string.Empty : reader.GetString(10)
        };
    }
}