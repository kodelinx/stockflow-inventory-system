using System.Globalization;
using StockFlow.Models;
using StockFlow.Utilities;

namespace StockFlow.Services;

public class BasketService
{
    public InputValidationService _inputValidationService;

    public BasketService(InputValidationService inputValidationService)
    {
        _inputValidationService = inputValidationService;
    }

    public void AddItemToBasket(List<Product> products, List<BasketItem> basketItems)
    {
        string productCode = _inputValidationService.GetRequiredText("Input Product Code to add to Basket: ");

        Product? product = products.FirstOrDefault(
        product => product.IsActive && 
        product.ProductCode.Equals(productCode, StringComparison.OrdinalIgnoreCase));

        if(product == null)
        {
            Console.WriteLine("The product is not available or inactive.");
            return;
        }

        int quantity = _inputValidationService.GetValidInt("How many products you wish to add? ", 1, product.QuantityInStock);

        BasketItem? existingBasketItem = basketItems.FirstOrDefault(
            basketItem => basketItem.ProductCode.Equals(productCode, StringComparison.OrdinalIgnoreCase));

        if(existingBasketItem != null)
        {
            int newQuantity = existingBasketItem.Quantity + quantity;

            if(newQuantity > product.QuantityInStock)
            {
                Console.WriteLine("There's not enough stock available.");
                return;
            }

            existingBasketItem.Quantity = newQuantity;
            existingBasketItem.LineTotal = existingBasketItem.Quantity * existingBasketItem.UnitPrice;

            Console.WriteLine("The Item in the basket has been updated.\n");
            return;
        }

        BasketItem newBasketItem = new BasketItem(
            product.ProductId,
            product.ProductCode,
            product.Name,
            quantity,
            product.UnitPrice
        );

        basketItems.Add(newBasketItem);

        Console.WriteLine("The product has been successfully added into the basket.");
        return;
    }
    public void RemoveIteminBasket(List<BasketItem> basketItems)
    {
        string productCode = _inputValidationService.GetRequiredText("Enter Product Code you wish to remove in the basket: ");

        BasketItem? basketItem = basketItems.FirstOrDefault(
            basketItem => basketItem.ProductCode.Equals(productCode, StringComparison.OrdinalIgnoreCase));

        if (basketItem == null)
        {
            Console.WriteLine("The item doesn't exist in the basket.");
            return;
        }

        basketItems.Remove(basketItem);

        Console.WriteLine("The item has been removed from basket.");
    }
    public void ViewBasket(List<BasketItem> basketItems)
    {
        if (basketItems.Count == 0)
        {
            Console.WriteLine("The basket is currently empty.\n");
            return;
        }

        foreach(BasketItem item in basketItems)
        {
            DisplayBasketItem(item);
        }

        Console.WriteLine($"BASKET TOTAL PRICE: {CalculateLinePrice(basketItems):C}\n");
    }
    public void ClearBasket(List<BasketItem> basketItems)
    {
        if(basketItems.Count == 0)
        {
            Console.WriteLine("There are no items in the basket.");
            return;
        }

        basketItems.Clear();
        Console.WriteLine("The basket is now empty.");
    }
    public void DisplayBasketItem(BasketItem item)
    {
        Console.WriteLine("-------------------\n");
        Console.WriteLine($"Product Id: {item.ProductId}");
        Console.WriteLine($"Product Code: {item.ProductCode}");
        Console.WriteLine($"Product Name: {item.ProductName}");
        Console.WriteLine($"Quantity to Buy: {item.Quantity}");
        Console.WriteLine($"Unit Price: {item.UnitPrice:C}");
        Console.WriteLine($"Total Price: {item.LineTotal:C}\n");

    }
    public decimal CalculateLinePrice(List<BasketItem> basketItems)
    {
        decimal total = 0;

        foreach(BasketItem item in basketItems)
        {
            total += item.LineTotal;
        }

        return total;
    }
}