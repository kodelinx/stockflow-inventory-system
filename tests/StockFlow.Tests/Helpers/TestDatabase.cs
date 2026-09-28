using Microsoft.Data.Sqlite;
using StockFlow.Database;

namespace StockFlow.Tests.Helpers;

// IDisposable means that this object owns resources that require cleanup.
public class TestDatabase : IDisposable
{
    public string DatabasePath { get; }

    public DatabaseConnectionService
        DatabaseConnectionService { get; }

    public TestDatabase()
    {
        string testFolder =
            Path.Combine(
                // returns the operating system's temp location (C:\Users\[User]\AppData\Local\Temp\StockFlowTests)
                Path.GetTempPath(),
                "StockFlowTests"
            );

        Directory.CreateDirectory(
            testFolder
        );

        DatabasePath =
            Path.Combine(
                testFolder,
                //Guid.NewGuid() creates a practically unique identifier.
                //N format removes removes hyphens
                $"stockflow-test-{Guid.NewGuid():N}.db"
            );

        DatabaseConnectionService =
            new DatabaseConnectionService(
                DatabasePath
            );

        DatabaseConnectionService.InitializeDatabase();
    }

    // Defines the cleanup
    // Clear pooled connection > Delete test DB > Delete WAL/SHM if present
    public void Dispose()
    {
        SqliteConnection.ClearAllPools();

        DeleteIfExists(DatabasePath);
        DeleteIfExists($"{DatabasePath}-wal");
        DeleteIfExists($"{DatabasePath}-shm");
    }

    private static void DeleteIfExists(
        string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}