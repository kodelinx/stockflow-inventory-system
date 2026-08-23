using System.Text.Json;

namespace StockFlow.Data;

public class JsonStorageService
{
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonStorageService()
    {
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

            Console.WriteLine($"Data saved successfully to {filePath}.\n");
        }
        catch(Exception ex)
        {
            Console.WriteLine("An error occured while saving data.");
            Console.WriteLine($"Error details: {ex.Message}\n");
        }
    }

    public List<T> LoadData<T>(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"No saved file found at {filePath}.\n");
                return new List<T>();
            }

            string json = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                Console.WriteLine($"Saved file is empty: {filePath}.\n");
                return new List<T>();
            }

            List<T>? loadedData = JsonSerializer.Deserialize<List<T>>(json);

            if (loadedData == null)
            {
                Console.WriteLine($"No data loaded from {filePath}.\n");
                return new List<T>();
            }

            Console.WriteLine($"Data loaded successfully from {filePath}.\n");
            return loadedData;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while loading data.");
            Console.WriteLine($"Error details: {ex.Message}\n");
            return new List<T>();
        }
        }
    
}