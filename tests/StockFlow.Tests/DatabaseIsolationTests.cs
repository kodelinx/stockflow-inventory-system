using StockFlow.Tests.Helpers;

namespace StockFlow.Tests;

public class DatabaseIsolationTests
{
    // Verifies that the helper creates a real SQLite file.
    [Fact]
    public void TestDatabase_CreatesIsolatedDatabase()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        bool databaseExists =
            File.Exists(
                testDatabase.DatabasePath
            );

        Assert.True(databaseExists);
    }
    // Proves different test databases don't share the same file.
    [Fact]
    public void TestDatabase_UsesUniquePath()
    {
        using TestDatabase firstDatabase = new TestDatabase();

        using TestDatabase secondDatabase = new TestDatabase();

        Assert.NotEqual(
            firstDatabase.DatabasePath,
            secondDatabase.DatabasePath
        );
    }

    //  Proves that the helper deletes the file after use.
    [Fact]
    public void TestDatabase_Dispose_RemovesDatabaseFile()
    {
        string databasePath;

        using (
            TestDatabase testDatabase =
                new TestDatabase()
        )
        {
            databasePath = testDatabase.DatabasePath;

            Assert.True(
                File.Exists(databasePath)
            );
        }

        Assert.False(
            File.Exists(databasePath)
        );
    }

    // Verify that test DB path is not your normal app database.
    [Fact]
    public void TestDatabase_DoesNotUseDevelopmentDatabasePath()
    {
        using TestDatabase testDatabase = new TestDatabase();

        string developmentDatabasePath =
            Path.GetFullPath(
                Path.Combine(
                    "Database",
                    "stockflow.db"
                )
            );

        Assert.NotEqual(
            developmentDatabasePath,
            testDatabase.DatabasePath
        );
}
}