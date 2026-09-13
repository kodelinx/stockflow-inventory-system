using System.Data;
using Microsoft.Data.Sqlite;
using StockFlow.Database;
using StockFlow.Models;

namespace StockFlow.Repositories;

public class ProductRepository
{
    private readonly DatabaseConnectionService _databaseConnectionService;

    public ProductRepository(DatabaseConnectionService databaseConnectionService)
    {
        _databaseConnectionService = databaseConnectionService;
    }

    public void AddProduct(Product product)
    {
        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        string sql = @"
            INSERT  INTO Products(
                ProductCode,
                Name,
                Category,
                UnitPrice,
                QuantityInStock,
                ReorderLevel,
                IsActive
            )
            VALUES (
                @ProductCode,
                @Name,
                @Category,
                @UnitPrice,
                @QuantityInStock,
                @ReorderLevel,
                @IsActive
            );
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        command.Parameters.AddWithValue("@ProductCode", product.ProductCode);
        command.Parameters.AddWithValue("@Name", product.Name);
        command.Parameters.AddWithValue("@Category", product.Category);
        command.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
        command.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);
        command.Parameters.AddWithValue("@ReorderLevel", product.ReorderLevel);
        command.Parameters.AddWithValue("@IsActive", product.IsActive ? 1 : 0);

        command.ExecuteNonQuery();
    }

    public List<Product> GetAllProducts()
    {
        List<Product> products = new List<Product>();

        // Opens a connection to the SQLite database.
        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        // Gets all products, whether active or inactive.
        string sql = @"
            SELECT
                ProductId,
                ProductCode,
                Name,
                Category,
                UnitPrice,
                QuantityInStock,
                ReorderLevel,
                IsActive
            FROM Products;
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        // Reads the rows returned by the SELECT query.
        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            // Converts the current database row into a Product object.
            Product product = new Product
            {
                ProductId = reader.GetInt32(0),
                ProductCode = reader.GetString(1),
                Name = reader.GetString(2),
                Category = reader.GetString(3),
                UnitPrice = reader.GetDecimal(4),
                QuantityInStock = reader.GetInt32(5),
                ReorderLevel = reader.GetInt32(6),

                // SQLite stores bool as 1 or 0, so convert it to true/false.
                IsActive = reader.GetInt32(7) == 1  
            };

            products.Add(product);
        }

        return products;
    }

    public List<Product> GetActiveProducts()
    {
        List<Product> products = new List<Product>();

        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        string sql = @"
            SELECT
                ProductId,
                ProductCode,
                Name,
                Category,
                UnitPrice,
                QuantityInStock,
                ReorderLevel,
                IsActive
            FROM Products
            WHERE IsActive = 1;
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Product product = MapReaderToProduct(reader);
            products.Add(product);
        }

        return products;
    }

    public Product? FindProductByCode(string productCode)
    {
        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        string sql = @"
            SELECT
                ProductId,
                ProductCode,
                Name,
                Category,
                UnitPrice,
                QuantityInStock,
                ReorderLevel,
                IsActive
            FROM Products
            WHERE ProductCode = @ProductCode;
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;
        command.Parameters.AddWithValue("@ProductCode", productCode);

        using SqliteDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Product
            {
                ProductId = reader.GetInt32(0),
                ProductCode = reader.GetString(1),
                Name = reader.GetString(2),
                Category = reader.GetString(3),
                UnitPrice = reader.GetDecimal(4),
                QuantityInStock = reader.GetInt32(5),
                ReorderLevel = reader.GetInt32(6),
                IsActive = reader.GetInt32(7) == 1
            };
        }

        return null;
    }

    public void UpdateProduct(Product product)
    {
        using SqliteConnection connection = new SqliteConnection(_databaseConnectionService.GetConnectionString());
        connection.Open();

        string updateSql = @"
        
            UPDATE Products
            SET
                Name = @Name,
                Category = @Category,
                UnitPrice = @UnitPrice,
                QuantityInStock = @QuantityInStock,
                ReorderLevel = @ReorderLevel,
                IsActive = @IsActive
            WHERE ProductCode = @ProductCode;
        ";

        using SqliteCommand command = new SqliteCommand(updateSql, connection);

        command.Parameters.AddWithValue("@ProductCode", product.ProductCode);
        command.Parameters.AddWithValue("@Name", product.Name);
        command.Parameters.AddWithValue("@Category", product.Category);
        command.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
        command.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);
        command.Parameters.AddWithValue("@ReorderLevel", product.ReorderLevel);
        command.Parameters.AddWithValue("@IsActive", product.IsActive);

        command.ExecuteNonQuery();
    }

    public void DeactivateProduct(string productCode)
    {
        using SqliteConnection connection = new SqliteConnection(_databaseConnectionService.GetConnectionString());
        connection.Open();

        string deactivateSql = @"
            UPDATE Products
            SET IsActive = @IsActive
            WHERE ProductCode = @ProductCode; 
        ";

        using SqliteCommand command = new SqliteCommand(deactivateSql, connection);

        command.Parameters.AddWithValue("@ProductCode", productCode);
        command.Parameters.AddWithValue("@IsActive", false);

        command.ExecuteNonQuery();
        
    }

    public void DeleteProduct(string productCode)
    {
        using SqliteConnection connection = new SqliteConnection(_databaseConnectionService.GetConnectionString());
        connection.Open();

        string deleteSql = @"
            DELETE FROM Products
            WHERE ProductCode = @ProductCode;
        ";

        using SqliteCommand command = new SqliteCommand(deleteSql, connection);

        command.Parameters.AddWithValue("@ProductCode", productCode);

        command.ExecuteNonQuery();
    }

    private Product MapReaderToProduct(SqliteDataReader reader)
    {
        return new Product
        {
            ProductId = reader.GetInt32(0),
            ProductCode = reader.GetString(1),
            Name = reader.GetString(2),
            Category = reader.GetString(3),
            UnitPrice = reader.GetDecimal(4),
            QuantityInStock = reader.GetInt32(5),
            ReorderLevel = reader.GetInt32(6),
            IsActive = reader.GetInt32(7) == 1
        };
    }
}