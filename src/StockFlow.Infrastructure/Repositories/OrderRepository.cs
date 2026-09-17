using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;
using StockFlow.Database;
using StockFlow.Models;

namespace StockFlow.Repositories;

public class OrderRepository
{
    private readonly DatabaseConnectionService _databaseConnectionService;

    public OrderRepository(DatabaseConnectionService databaseConnectionService)
    {
        _databaseConnectionService = databaseConnectionService;
    }

    public void AddOrder(Order order)
    {
        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        string sql = @"
            INSERT INTO Orders(
                OrderNumber,
                OrderDate,
                TotalAmount,
                OrderStatus,
                PaymentStatus
            )
            VALUES(
                @OrderNumber,
                @OrderDate,
                @TotalAmount,
                @OrderStatus,
                @PaymentStatus
            );
        ";
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        command.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);
        command.Parameters.AddWithValue("@OrderDate", order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
        command.Parameters.AddWithValue("@OrderStatus", order.OrderStatus);
        command.Parameters.AddWithValue("@PaymentStatus", order.PaymentStatus);

        command.ExecuteNonQuery();
    }

    public List<Order> GetAllOrders()
    {
        List<Order> orders = new List<Order>();

        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        string sql = @"
            SELECT
                OrderId,
                OrderNumber,
                OrderDate,
                TotalAmount,
                OrderStatus,
                PaymentStatus
            FROM Orders
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Order order = MapReaderToOrder(reader);
            orders.Add(order); 
        }

        return orders;
    }

    public Order? FindOrderByNumber(string orderNumber)
    {
        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        string sql = @"
            SELECT
                OrderId,
                OrderNumber,
                OrderDate,
                TotalAmount,
                OrderStatus,
                PaymentStatus
            FROM Orders
            WHERE OrderNumber = @OrderNumber;
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;
        command.Parameters.AddWithValue("@OrderNumber", orderNumber);

        using SqliteDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            return MapReaderToOrder(reader);
        }

        return null;
    }

    public void UpdateOrderStatus(string orderNumber, string orderStatus)
    {
        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );
        connection.Open();

        // Updates only the order status.
        string sql = @"
            UPDATE Orders
            SET OrderStatus = @OrderStatus
            WHERE OrderNumber = @OrderNumber;
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        command.Parameters.AddWithValue("@OrderNumber", orderNumber);
        command.Parameters.AddWithValue("@OrderStatus", orderStatus);

        command.ExecuteNonQuery();
    }

    public void UpdatePaymentStatus(string orderNumber, string paymentStatus)
    {
        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );
        connection.Open();

        string sql = @"
            UPDATE Orders
            SET PaymentStatus = @PaymentStatus
            WHERE OrderNumber = @OrderNumber;
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        command.Parameters.AddWithValue("@PaymentStatus", paymentStatus);
        command.Parameters.AddWithValue("@OrderNumber", orderNumber);

        command.ExecuteNonQuery();
    }



    private Order MapReaderToOrder(SqliteDataReader reader)
    {
        // Converts database column values into an Order model.
        return new Order
        {
            OrderId = reader.GetInt32(0),
            OrderNumber = reader.GetString(1),
            OrderDate = DateTime.Parse(reader.GetString(2)),
            TotalAmount = reader.GetDecimal(3),
            OrderStatus = reader.GetString(4),
            PaymentStatus = reader.GetString(5)
        };
    }
}