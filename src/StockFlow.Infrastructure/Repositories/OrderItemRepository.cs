using Microsoft.Data.Sqlite;
using StockFlow.Models;
using StockFlow.Database;

namespace StockFlow.Repositories;

public class OrderItemRepository
{
    private readonly DatabaseConnectionService _databaseConnectionService;

    public OrderItemRepository(DatabaseConnectionService databaseConnectionService)
    {
        _databaseConnectionService = databaseConnectionService;
    }

    public void AddOrderItem(int orderId, OrderItem orderItem)
    {
        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );
        connection.Open();

        string sql = @"
            INSERT INTO OrderItems(
                OrderId,
                ProductId,
                ProductCode,
                ProductName,
                Quantity,
                UnitPrice,
                LineTotal
            )
            VALUES(
                @OrderId,
                @ProductId,
                @ProductCode,
                @ProductName,
                @Quantity,
                @UnitPrice,
                @LineTotal
            );
        ";    
        using SqliteCommand command = new SqliteCommand(sql, connection);

        command.Parameters.AddWithValue("@OrderId", orderId);
        command.Parameters.AddWithValue("@ProductId", orderItem.ProductId);
        command.Parameters.AddWithValue("@ProductCode", orderItem.ProductCode);
        command.Parameters.AddWithValue("@ProductName", orderItem.ProductName);
        command.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
        command.Parameters.AddWithValue("@UnitPrice", orderItem.UnitPrice);
        command.Parameters.AddWithValue("@LineTotal", orderItem.LineTotal);

        command.ExecuteNonQuery();
    }

    public List<OrderItem> GetOrderItemsByOrderId(int orderId)
    {
        List<OrderItem> orderItems = new List<OrderItem>();

        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );
        connection.Open();

        string sql = @"
            SELECT
                OrderItemId,
                OrderId,
                ProductId,
                ProductCode,
                ProductName,
                Quantity,
                UnitPrice,
                LineTotal
            FROM Orderitems
            WHERE OrderId = @OrderId;
        ";

        using SqliteCommand command = new SqliteCommand(sql, connection);

        command.Parameters.AddWithValue("@OrderId", orderId);

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            OrderItem orderItem = MapReaderToOrderItem(reader);

            orderItems.Add(orderItem);
        }

        return orderItems;
    }

    private OrderItem MapReaderToOrderItem(SqliteDataReader reader)
    {
        return new OrderItem
        {
            OrderItemId = reader.GetInt32(0),
            OrderId = reader.GetInt32(1),
            ProductId = reader.GetInt32(2),
            ProductCode = reader.GetString(3),
            ProductName = reader.GetString(4),
            Quantity = reader.GetInt32(5),
            UnitPrice = reader.GetDecimal(6),
            LineTotal = reader.GetDecimal(7)
        };
    }

}