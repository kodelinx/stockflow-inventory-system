using Microsoft.Data.Sqlite;
using StockFlow.Database;
using StockFlow.Models;

namespace StockFlow.Repositories;

public class PaymentRepository
{
    private readonly DatabaseConnectionService _databaseConnectionService;

    public PaymentRepository(DatabaseConnectionService databaseConnectionService)
    {
        _databaseConnectionService = databaseConnectionService;
    }

    public int AddPayment(Payment payment)
    {
        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );
        connection.Open();

        string sql = @"
            INSERT INTO Payments
            (
                PaymentNumber,
                OrderId,
                OrderNumber,
                PaymentDate,
                PaymentMethod,
                AmountDue,
                AmountPaid,
                ChangeAmount,
                PaymentStatus
            )
            VALUES
            (
                @PaymentNumber,
                @OrderId,
                @OrderNumber,
                @PaymentDate,
                @PaymentMethod,
                @AmountDue,
                @AmountPaid,
                @ChangeAmount,
                @PaymentStatus
            )
            RETURNING PaymentId;
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        command.Parameters.AddWithValue("@PaymentNumber", payment.PaymentNumber);
        command.Parameters.AddWithValue("@OrderId", payment.OrderId);
        command.Parameters.AddWithValue("@OrderNumber", payment.OrderNumber);
        command.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@PaymentMethod", payment.PaymentMethod);
        command.Parameters.AddWithValue("@AmountDue", payment.AmountDue);
        command.Parameters.AddWithValue("@AmountPaid", payment.AmountPaid);
        command.Parameters.AddWithValue("@ChangeAmount", payment.ChangeAmount);
        command.Parameters.AddWithValue("@PaymentStatus", payment.PaymentStatus);

        object? result = command.ExecuteScalar();

        if (result == null)
        {
            throw new InvalidOperationException(
                "Payment was inserted but no PaymentId was returned."
            );
        }

        return Convert.ToInt32(result);
    }

    public List<Payment> GetAllPayments()
    {
        List<Payment> payments = new List<Payment>();

        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        string sql = @"
            SELECT
                PaymentId,
                PaymentNumber,
                OrderId,
                OrderNumber,
                PaymentDate,
                PaymentMethod,
                AmountDue,
                AmountPaid,
                ChangeAmount,
                PaymentStatus
            FROM Payments;
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        using SqliteDataReader reader = command.ExecuteReader();
        
        while (reader.Read())
        {
            Payment payment = MapReaderToPayment(reader);

            payments.Add(payment);
        }

        return payments;
    }

    public Payment? FindPaymentByNumber(string paymentNumber)
    {
        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );
        connection.Open();

        string sql = @"
            SELECT 
                PaymentId,
                PaymentNumber,
                OrderId,
                OrderNumber,
                PaymentDate,
                PaymentMethod,
                AmountDue,
                AmountPaid,
                ChangeAmount,
                PaymentStatus
            FROM Payments
            WHERE PaymentNumber = @PaymentNumber;
        ";

        using SqliteCommand command = new SqliteCommand(sql, connection);

        command.Parameters.AddWithValue("@PaymentNumber", paymentNumber);

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Payment payment = MapReaderToPayment(reader);

            return payment;
        }

        return null;
    }

    public List<Payment> GetPaymentsByOrderNumber(string orderNumber)
    {
        List<Payment> payments = new List<Payment>();

        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );
        connection.Open();

        string sql = @"
            SELECT 
                PaymentId,
                PaymentNumber,
                OrderId,
                OrderNumber,
                PaymentDate,
                PaymentMethod,
                AmountDue,
                AmountPaid,
                ChangeAmount,
                PaymentStatus
            FROM Payments
            WHERE OrderNumber = @OrderNumber;
        ";

        using SqliteCommand command = new SqliteCommand(sql, connection);

        command.Parameters.AddWithValue("@OrderNumber", orderNumber);

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Payment payment = MapReaderToPayment(reader);

            payments.Add(payment);
        }

        return payments;
    }

    private Payment MapReaderToPayment(SqliteDataReader reader)
    {
        return new Payment
        {
            PaymentId = reader.GetInt32(0),
            PaymentNumber = reader.GetString(1),
            OrderId = reader.GetInt32(2),
            OrderNumber = reader.GetString(3),
            PaymentDate = DateTime.Parse(reader.GetString(4)),
            PaymentMethod = reader.GetString(5),
            AmountDue = reader.GetDecimal(6),
            AmountPaid = reader.GetDecimal(7),
            ChangeAmount = reader.GetDecimal(8),
            PaymentStatus = reader.GetString(9)
        };
    }
}