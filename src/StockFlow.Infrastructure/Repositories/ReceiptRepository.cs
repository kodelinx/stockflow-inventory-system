using Microsoft.Data.Sqlite;
using SQLitePCL;
using StockFlow.Database;
using StockFlow.Models;

namespace StockFlow.Repositories;

public class ReceiptRepository
{
    private readonly DatabaseConnectionService _databaseConnectionService;

    public ReceiptRepository(DatabaseConnectionService databaseConnectionService)
    {
        _databaseConnectionService = databaseConnectionService;
    }

    public void AddReceipt(int orderId, int paymentId, Receipt receipt)
    {
        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );
        connection.Open();

        string sql = @"
            INSERT INTO Receipts
            (
                ReceiptNumber,
                OrderId,
                PaymentId,
                OrderNumber,
                PaymentNumber,
                ReceiptDate,
                TotalAmount,
                PaymentMethod,
                AmountPaid,
                ChangeAmount
            )
            VALUES
            (
                @ReceiptNumber,
                @OrderId,
                @PaymentId,
                @OrderNumber,
                @PaymentNumber,
                @ReceiptDate,
                @TotalAmount,
                @PaymentMethod,
                @AmountPaid,
                @ChangeAmount
            )
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        command.Parameters.AddWithValue("@ReceiptNumber", receipt.ReceiptNumber);
        command.Parameters.AddWithValue("@OrderId", orderId);
        command.Parameters.AddWithValue("@PaymentId", paymentId);
        command.Parameters.AddWithValue("@OrderNumber", receipt.OrderNumber);
        command.Parameters.AddWithValue("@PaymentNumber", receipt.PaymentNumber);
        command.Parameters.AddWithValue("@ReceiptDate", receipt.ReceiptDate.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@TotalAmount", receipt.TotalAmount);
        command.Parameters.AddWithValue("@PaymentMethod", receipt.PaymentMethod);
        command.Parameters.AddWithValue("@AmountPaid", receipt.AmountPaid);
        command.Parameters.AddWithValue("@ChangeAmount", receipt.ChangeAmount);

        command.ExecuteNonQuery();
    }

    public List<Receipt> GetAllReceipts()
    {
        List<Receipt> receipts = new List<Receipt>();

        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );
        connection.Open();

        string sql = @"
            SELECT
                ReceiptId,
                ReceiptNumber,
                OrderId,
                PaymentId,
                OrderNumber,
                PaymentNumber,
                ReceiptDate,
                TotalAmount,
                PaymentMethod,
                AmountPaid,
                ChangeAmount
            FROM Receipts
        ";

        using SqliteCommand command = new SqliteCommand(sql,  connection);

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Receipt receipt = MapReaderToReceipt(reader);

            receipts.Add(receipt);
        }

        return receipts;
    }

    public Receipt? FindReceiptByNumber(string receiptNumber)
    {
        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );
        connection.Open();

        string sql = @"
            SELECT
                ReceiptId,
                ReceiptNumber,
                OrderId,
                PaymentId,
                OrderNumber,
                PaymentNumber,
                ReceiptDate,
                TotalAmount,
                PaymentMethod,
                AmountPaid,
                ChangeAmount
            FROM Receipts
            WHERE ReceiptNumber = @ReceiptNumber;
        ";

        using SqliteCommand command = new SqliteCommand(sql, connection);

        command.Parameters.AddWithValue("@ReceiptNumber", receiptNumber);

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Receipt receipt = MapReaderToReceipt(reader);

            return receipt;
        }

        return null;
    }

    public Receipt? FindReceiptByPaymentNumber(string paymentNumber)
    {
        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        string sql = @"
            SELECT
                ReceiptId,
                ReceiptNumber,
                OrderId,
                PaymentId,
                OrderNumber,
                PaymentNumber,
                ReceiptDate,
                TotalAmount,
                PaymentMethod,
                AmountPaid,
                ChangeAmount
            FROM Payments
            WHERE PaymentNumber = @PaymentNumber
        ";

        using SqliteCommand command = new SqliteCommand(sql, connection);

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            return MapReaderToReceipt(reader); 
        }

        return null;
    }
    public List<Receipt> GetReceiptsByOrderNumber(string orderNumber)
    {
        // Stores all receipts connected to one order.
        List<Receipt> receipts = new List<Receipt>();

        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        // Gets receipts linked to the selected order number.
        string sql = @"
            SELECT
                ReceiptId,
                ReceiptNumber,
                OrderNumber,
                PaymentNumber,
                ReceiptDate,
                TotalAmount,
                PaymentMethod,
                AmountPaid,
                ChangeAmount
            FROM Receipts
            WHERE OrderNumber = @OrderNumber;
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        command.Parameters.AddWithValue("@OrderNumber", orderNumber);

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            // Converts each matching row into a Receipt object.
            Receipt receipt = MapReaderToReceipt(reader);

            receipts.Add(receipt);
        }

        return receipts;
    }

    private Receipt MapReaderToReceipt(SqliteDataReader reader)
    {
        return new Receipt
        {
            ReceiptId = reader.GetInt32(0),
            ReceiptNumber = reader.GetString(1),
            OrderId = reader.GetInt32(2),
            PaymentId = reader.GetInt32(3),
            OrderNumber = reader.GetString(4),
            PaymentNumber = reader.GetString(5),
            ReceiptDate = DateTime.Parse(reader.GetString(6)),
            TotalAmount = reader.GetDecimal(7),
            PaymentMethod = reader.GetString(8),
            AmountPaid = reader.GetDecimal(9),
            ChangeAmount = reader.GetDecimal(10)
        };
    }

}