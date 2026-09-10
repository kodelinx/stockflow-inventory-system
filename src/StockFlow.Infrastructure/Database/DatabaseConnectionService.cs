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
                ProductCode INTEGER NOT NULL UNIQUE,
                Name TEXT NOT NULL,
                Category TEXT NOT NULL,
                UnitPrice DECIMAL(10,2) NOT NULL,
                QuantityInStock INTEGER NOT NULL,
                ReorderLevel INTEGER NOT NULL,
                IsActive INTEGER NOT NULL
            );
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = createProductsTableSql;
        //Runs SQL commands that do not return rows.
        command.ExecuteNonQuery();

        SeedProducts();
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
            4,
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