using System.Text.Json;
using StockFlow.Utilities;

namespace StockFlow.Data;

public class JsonStorageService
{
    private readonly JsonSerializerOptions _jsonOptions;

    private readonly LoggingService _loggingService;

    public JsonStorageService(LoggingService loggingService)
    {
        _loggingService = loggingService;

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
    }

    public void SaveData<T>(List<T> data, string filePath)
    {
        try
        {
            string json = JsonSerializer.Serialize(data, _jsonOptions);
            File.WriteAllText(filePath, json);

            string message = $"Data saved successfully to {filePath}.\n";

            Console.WriteLine($"{message}");
            _loggingService.LogInfo(message);
            
        }
        catch(Exception ex)
        {
            Console.WriteLine("An error occured while saving data.");
            Console.WriteLine($"Error details: {ex.Message}\n");

            _loggingService.LogError($"Failed to save data to {filePath}.", ex);
            
        }


    }

    public List<T> LoadData<T>(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"No saved file found at {filePath}.\n");
                _loggingService.LogInfo($"No saved file found at {filePath}.\n");
                return new List<T>();
            }

            string json = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                Console.WriteLine($"Saved file is empty: {filePath}.\n");
                _loggingService.LogInfo($"Saved file is empty: {filePath}.\n");
                return new List<T>();
            }

            List<T>? loadedData = JsonSerializer.Deserialize<List<T>>(json);

            if (loadedData == null)
            {
                Console.WriteLine($"No data loaded from {filePath}.\n");
                _loggingService.LogInfo($"No data loaded from {filePath}.\n");
                return new List<T>();
            }

            Console.WriteLine($"Data loaded successfully from {filePath}.\n");
            _loggingService.LogInfo($"Data loaded successfully from {filePath}.\n");
            return loadedData;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while loading data.");
            Console.WriteLine($"Error details: {ex.Message}\n");

            _loggingService.LogError($"Failed to load data from {filePath}", ex);

            return new List<T>();
        }
        }
    
}