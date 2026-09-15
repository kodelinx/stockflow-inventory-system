using System.Data;
using Microsoft.Data.Sqlite;

namespace StockFlow.Database;

public class DatabaseConnectionService
{
    private readonly string _databaseFilePath = "Database/stockflow.db";
    public string GetConnectionString()
    {
            return $"Data Source={_databaseFilePath}";
    }

    public void InitializeDatabase()
    {
        Directory.CreateDirectory("Database");

        //Creates a database connection object
        // automatically closes and disposes the database connection after use
        using SqliteConnection connection = new SqliteConnection(GetConnectionString());
        //Starts the connection to the SQLite database file.
        connection.Open();

        string createProductsTableSql = @"
            CREATE TABLE IF NOT EXISTS Products (
                ProductId INTEGER PRIMARY KEY,
                ProductCode STRING NOT NULL UNIQUE,
                Name TEXT NOT NULL,
                Category TEXT NOT NULL,
                UnitPrice DECIMAL(10,2) NOT NULL,
                QuantityInStock INTEGER NOT NULL,
                ReorderLevel INTEGER NOT NULL,
                IsActive INTEGER NOT NULL
            );
        ";

        string createOrdersTableSql = @"
            CREATE TABLE IF NOT EXISTS Orders(
                OrderID INTEGER PRIMARY KEY,
                OrderNumber TEXT NOT NULL UNIQUE,
                OrderDate TEXT NOT NULL,
                TotalAmount DECIMAL(10,2) NOT NULL,
                OrderStatus TEXT NOT NULL,
                PaymentStatus TEXT NOT NULL
            );
        ";

        string createOrderItemsTableSql = @"
            CREATE TABLE IF NOT EXISTS OrderItems(
                OrderItemId INTEGER PRIMARY KEY,
                OrderId INTEGER NOT NULL,
                ProductId INTEGER NOT NULL,
                ProductCode INTEGER NOT NULL,
                ProductName STRING NOT NULL,
                Quantity INTEGER NUT NULL,
                UnitPrice DECIMAL(10,2) NOT NULL,
                LineTota DECIMAL(10,2) NOT NULL,
                FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
                FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
            );
        ";

        string createPaymentsTableSql = @"
            CREATE TABLE IF NOT EXISTS Payments(
                PaymentId INTEGER PRIMARY KEY,
                PaymentNumber INTEGER NOT NULL UNIQUE,
                OrderId INTEGER NOT NULL,
                OrderNumber TEXT NOT NULL,
                PaymentDate TEXT NOT NULL,
                PaymentMethod TEXT NOT NULL,
                AmountDue DECIMAL(10,2) NOT NULL,
                AmountPaid DECIMAL(10,2) NOT NULL,
                ChangeAmount DECIMAL(10,2) NOT NULL,
                PaymentStatus TEXT NOT NULL,
                FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
            );
        ";

        string createReceiptsTableSql = @"
            CREATE TABLE IF NOT EXISTS Receipts(
                ReceiptId INTEGER PRIMARY KEY,
                ReceiptNumber TEXT NOT NULL UNIQUE,
                OrderId INTEGER NOT NULL,
                PaymentId INTEGER NOT NULL,
                OrderNumber TEXT NOT NULL,
                PaymentNumber TEXT NOT NULL,
                ReceiptDate TEXT NOT NULL,
                TotalAmount DECIMAL(10,2) NOT NULL,
                PaymentMethod TEXT NOT NULL,
                AmountPaid DECIMAL(10,2) NOT NULL,
                ChangeAmount DECIMAL(10,2) NOT NULL,
                FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
                FOREIGN KEY (PaymentId) REFERENCES Payments(PaymentId)
            );
        ";

        string createStockMovementsTableSql = @"
            CREATE TABLE IF NOT EXISTS StockMovements (
                StockMovementId INTEGER PRIMARY KEY,
                ProductId INTEGER NOT NULL,
                ProductCode TEXT NOT NULL,
                ProductName TEXT NOT NULL,
                MovementType TEXT NOT NULL,
                QuantityChanged INTEGER NOT NULL,
                StockBefore INTEGER NOT NULL,
                StockAfter INTEGER NOT NULL,
                Reason TEXT NOT NULL,
                MovementDate TEXT NOT NULL,
                ReferenceNumber TEXT,
                FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
            );
        ";

        string createNotificationsTableSql = @"
            CREATE TABLE IF  NOT EXISTS Notification(
                NotificationId INTEGER PRIMARY KEY,
                NotificationType TEXT NOT NULL,
                Recipient TEXT NOT NULL,
                Subject TEXT NOT NULL,
                Title TEXT NOT NULL,
                Message TEXT NOT NULL,
                RelatedReference TEXT,
                IsRead INTEGER NOT NULL,
                CreatedAt TEXT NOT NULL,
                Status TEXT NOT NULL
            );
        ";

        // Runs each CREATE TABLE command.
        ExecuteNonQuery(connection, createProductsTableSql);
        ExecuteNonQuery(connection, createOrdersTableSql);
        ExecuteNonQuery(connection, createOrderItemsTableSql);
        ExecuteNonQuery(connection, createPaymentsTableSql);
        ExecuteNonQuery(connection, createReceiptsTableSql);
        ExecuteNonQuery(connection, createStockMovementsTableSql);
        ExecuteNonQuery(connection, createNotificationsTableSql);

        SeedProducts();
    }

    private void ExecuteNonQuery(SqliteConnection connection, string sql)
    {
        // Runs SQL commands that do not return rows.
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }

    private void SeedProducts()
    {
        using SqliteConnection connection = new SqliteConnection(GetConnectionString());
        connection.Open();

        string countSql = "SELECT COUNT(*) FROM Products;";

        using SqliteCommand countCommand = new SqliteCommand(countSql, connection);

        long productCount = (long)countCommand.ExecuteScalar()!;

        if (productCount > 0)
        {
            return;
        }

        string insertSql = @"
            INSERT INTO Products
            (
                ProductCode,
                Name,
                Category,
                UnitPrice,
                QuantityInStock,
                ReorderLevel,
                IsActive
            )
            VALUES
            (
                @ProductCode,
                @Name,
                @Category,
                @UnitPrice,
                @QuantityInStock,
                @ReorderLevel,
                @IsActive
            );
        ";

        InsertProduct(
            connection,
            insertSql,
            "P001",
            "Mouse",
            "Accessories",
            250.00m,
            20,
            5,
            true
        );

        InsertProduct(
            connection,
            insertSql,
            "P002",
            "Keyboard",
            "Accessories",
            750.00m,
            10,
            3,
            true
        );

        InsertProduct(
            connection,
            insertSql,
            "P003",
            "Monitor",
            "Display",
            5500.00m,
            1,
            2,
            true
        );
    }

    private void InsertProduct(
        SqliteConnection connection,
        string insertSql,
        string productCode,
        string name,
        string category,
        decimal unitPrice,
        int quantityInStock,
        int reorderLevel,
        bool isActive)
    {
        using SqliteCommand command = new SqliteCommand(insertSql, connection);

        command.Parameters.AddWithValue("@ProductCode", productCode);
        command.Parameters.AddWithValue("@Name", name);
        command.Parameters.AddWithValue("@Category", category);
        command.Parameters.AddWithValue("@UnitPrice", unitPrice);
        command.Parameters.AddWithValue("@QuantityInStock", quantityInStock);
        command.Parameters.AddWithValue("@ReorderLevel", reorderLevel);
        command.Parameters.AddWithValue("@IsActive", isActive);

        command.ExecuteNonQuery();
    }
}