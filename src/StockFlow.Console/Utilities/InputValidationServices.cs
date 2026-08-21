
namespace StockFlow.Utilities;

public class InputValidationService
{
    public string GetRequiredText(string input)
    {
        while (true)
        {
            Console.Write(input);
            string value = Console.ReadLine() ?? "";

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }

            Console.WriteLine("Input is required. Please try again. \n");
        }
    }

    public int GetValidInt(string message, int min, int max)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int number) && number >= min && number <= max)
            {
                return number;
            }
            Console.WriteLine($"Please enter a valid number from {min} to {max}.\n"); 
        }

    }

    public decimal GetValidDecimal(string message, decimal min, decimal max)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();
            if(decimal.TryParse(input, out decimal number) && number >= min && number <= max)
            {
                return number;
            }
            
            Console.WriteLine($"Please enter a valid number from {min} to {max}.\n");
        }
    }
    public int GetValidMenuOption(string message, int min, int max)
    {
        return GetValidInt(message, min, max);
    }
}