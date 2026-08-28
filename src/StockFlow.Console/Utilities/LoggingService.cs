using System.Runtime.CompilerServices;
using StockFlow.Models;

namespace StockFlow.Utilities;

public class LoggingService
{
    private readonly string _logFolderPath = "Logs";
    private readonly string _logFilePath = "Logs/app-log.txt";
    public void LogInfo(string message)
    {
        WriteLog("INFO", message);
    }
    public void LogError(string message, Exception ex)
    {
        string errorMessage = $"{message} | Exception: {ex.Message}";
        WriteLog("Error", errorMessage);
    }
    private void WriteLog(string loglevel, string message)
    {
        try
        {
            if (!Directory.Exists(_logFolderPath))
            {
                Directory.CreateDirectory(_logFolderPath);
            }

            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{loglevel} {message}]";

            File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
        }
        catch
        {
            //Avoid crashing the app if logging itself fails
        }
    }


}