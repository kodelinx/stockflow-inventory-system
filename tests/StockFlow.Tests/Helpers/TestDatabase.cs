using Microsoft.Data.Sqlite;
using StockFlow.Database;

namespace StockFlow.Tests.Helpers;

// TestDatabase creates a temporary SQLite database for automated tests.
// This prevents tests from modifying the actual StockFlow database.
//
// IDisposable allows the class to define cleanup logic through Dispose().
// When used with "using", Dispose() runs automatically when the scope ends.
public class TestDatabase : IDisposable
{
     // Stores the full path of the temporary test database file.
    public string DatabasePath { get; }

    // Provides the database connection configuration to repositories.
    // Unlike the normal application, this instance points to the test database.
    public DatabaseConnectionService
        DatabaseConnectionService { get; }

    // Constructor: automatically runs when a new TestDatabase is created.
    // Its responsibility is to prepare a fresh database for testing.
    public TestDatabase()
    {
        // STEP 1: Determine where the temporary database will be stored.

        // Path.GetTempPath() gets the operating system's temporary folder.
        // Path.Combine() safely joins it with the StockFlowTests folder name.
        string testFolder =
            Path.Combine(
                // returns the operating system's temp location (C:\Users\[User]\AppData\Local\Temp\StockFlowTests)
                Path.GetTempPath(),
                "StockFlowTests"
            );

        Directory.CreateDirectory(
            testFolder
        );

        // STEP 2: Generate a unique database filename.
        DatabasePath =
            Path.Combine(
                testFolder,
                //Guid.NewGuid() creates a practically unique identifier.
                //N format removes removes hyphens
                $"stockflow-test-{Guid.NewGuid():N}.db"
            );

        // STEP 3: Configure the database connection.
        // It points to the test database instead of the normal one. Repositories receiving this service will use the test database.
        DatabaseConnectionService =
            new DatabaseConnectionService(
                DatabasePath
            );

        DatabaseConnectionService.InitializeDatabase();
    }

    // Defines the cleanup
    // Clear pooled connection > Delete test DB > Delete WAL/SHM if present
    // CLEANUP: Called automatically when a TestDatabase created
    // with "using" reaches the end of its scope.
    // This removes the temporary database after testing.
    public void Dispose()
    {
        // STEP 1: Clear SQLite connection pools.

        // SQLite may keep idle database connections available for reuse.
        // Clearing the pools helps release connections before deleting files.
        //
        // Note: This affects all SQLite connection pools in the process,
        // and does not forcibly close connections still being used.
        SqliteConnection.ClearAllPools();

        // STEP 2: Delete the main temporary database file.
        DeleteIfExists(DatabasePath);

        // STEP 3: Delete SQLite auxiliary files if they exist.

        // -wal = Write-Ahead Log, used to record database changes.
        // -shm = Shared Memory, used for coordination in WAL mode.
        //
        // These files are not always created.
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