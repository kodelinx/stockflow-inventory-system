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
    }
}